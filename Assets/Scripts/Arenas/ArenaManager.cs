using UnityEngine;
using System.Collections.Generic;

public class ArenaManager : MonoBehaviour
{
    [SerializeField] private SpawnPoint[] spawnPoints;
    [SerializeField] private GameObject exitDoor;
    [SerializeField] private TeleportDoor enterDoor;
    [SerializeField] private Transform teleportPoint;

    [Header("������ �������")]
    [SerializeField] private PortalVisual portalVisual;

    private BoxCollider exitCollider;
    private bool hasGivenAspectPoint = false;
    private bool hasGivenImplant = false;
    private bool arenaActivated = false;

    private void Start()
    {
        exitCollider = exitDoor.GetComponent<BoxCollider>();
        if (exitCollider != null)
        {
            exitCollider.enabled = false;
        }

        hasGivenAspectPoint = false;
        hasGivenImplant = false;
        arenaActivated = false;

        if (portalVisual != null)
        {
            portalVisual.SetPortalOpen(true); // ��� ������ � ������� ������ � ������
        }

    }

    private void ActivateArena()
    {
        bool isEliteWave = enterDoor.IsEliteWave();

        foreach (var spawnPoint in spawnPoints)
        {
            spawnPoint.SpawnEnemy(isEliteWave);
        }

        if (exitCollider != null)
            exitCollider.enabled = false;

        hasGivenAspectPoint = false;
        hasGivenImplant = false;
        arenaActivated = true;

        if (portalVisual != null)
        {
            portalVisual.SetPortalOpen(false); // ����� ������������ � ����� ����� �����
        }

    }

    private void Update()
    {
        if (!arenaActivated) return;

        bool areEnemiesRemaining = false;

        foreach (var spawnPoint in spawnPoints)
        {
            if (spawnPoint.Enemy != null)
            {
                areEnemiesRemaining = true;
                break;
            }
        }

        if (!areEnemiesRemaining)
        {
            if (exitCollider != null)
                exitCollider.enabled = true;

            if (!hasGivenAspectPoint)
            {
                PlayerAspects.Instance?.AddAspectPoint();
                hasGivenAspectPoint = true;
            }

            if (!hasGivenImplant)
            {
                GiveRandomImplantToPlayer();
                hasGivenImplant = true;
            }

            if (portalVisual != null)
            {
                portalVisual.SetPortalOpen(true); // ����� ����� � ���������
            }
        }
        else
        {
            if (exitCollider != null)
                exitCollider.enabled = false;

            if (portalVisual != null)
            {
                portalVisual.SetPortalOpen(false); // ����� ���� � �������
            }
        }
    }

    private void GiveRandomImplantToPlayer()
    {
        if (ImplantDatabase.Instance == null)
        {
            Debug.LogError("ImplantDatabase �� ������!");
            return;
        }
        if (PlayerInventory.Instance == null)
        {
            Debug.LogError("PlayerInventory �� ������!");
            return;
        }

        List<Implant> allImplants = ImplantDatabase.Instance.GetAllImplants();
        List<Implant> availableImplants = new List<Implant>();

        foreach (Implant implant in allImplants)
        {
            if (!PlayerInventory.Instance.HasImplantAnywhere(implant))
            {
                availableImplants.Add(implant);
            }
        }

        if (availableImplants.Count == 0)
        {
            Debug.Log("��� �������� ��� ������. ����� �������� �� ��������.");
            return;
        }

        Implant randomImplant = availableImplants[Random.Range(0, availableImplants.Count)];
        PlayerInventory.Instance.AddNewImplant(randomImplant);
        UIManager.Instance.ShowImplantNotification(randomImplant);

        Debug.Log($"����� ������� �������: {randomImplant.Name}");
    }

    private void OnEnable()
    {
        enterDoor.OnPlayerTeleport += OnPlayerTeleport;
    }

    private void OnPlayerTeleport(Transform obj)
    {
        if (obj == teleportPoint)
        {
            ActivateArena();
        }
    }

    private void OnDisable()
    {
        enterDoor.OnPlayerTeleport -= OnPlayerTeleport;
    }
}
