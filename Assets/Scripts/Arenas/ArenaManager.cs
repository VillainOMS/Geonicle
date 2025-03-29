using UnityEngine;
using System.Collections.Generic;

public class ArenaManager : MonoBehaviour
{
    [SerializeField] private SpawnPoint[] spawnPoints;
    [SerializeField] private GameObject exitDoor;
    [SerializeField] private TeleportDoor enterDoor;
    [SerializeField] private Transform teleportPoint;

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
    }

    private void ActivateArena()
    {
        bool isEliteWave = enterDoor.IsEliteWave(); // получаем от телепорта

        foreach (var spawnPoint in spawnPoints)
        {
            spawnPoint.SpawnEnemy(isEliteWave);
        }

        if (exitCollider != null)
            exitCollider.enabled = false;

        hasGivenAspectPoint = false;
        hasGivenImplant = false;
        arenaActivated = true;
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

        if (!areEnemiesRemaining && exitCollider != null)
        {
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
        }
        else
        {
            exitCollider.enabled = false;
        }
    }

    private void GiveRandomImplantToPlayer()
    {
        if (ImplantDatabase.Instance == null)
        {
            Debug.LogError("ImplantDatabase не найден!");
            return;
        }
        if (PlayerInventory.Instance == null)
        {
            Debug.LogError("PlayerInventory не найден!");
            return;
        }

        List<Implant> allImplants = ImplantDatabase.Instance.GetAllImplants();
        List<Implant> availableImplants = new List<Implant>();

        foreach (Implant implant in allImplants)
        {
            // правильная проверка
            if (!PlayerInventory.Instance.HasImplantAnywhere(implant))
            {
                availableImplants.Add(implant);
            }
        }

        if (availableImplants.Count == 0)
        {
            Debug.Log("Все импланты уже выданы. Новые импланты не выдаются.");
            return;
        }

        Implant randomImplant = availableImplants[Random.Range(0, availableImplants.Count)];
        PlayerInventory.Instance.AddNewImplant(randomImplant);
        UIManager.Instance.ShowImplantNotification(randomImplant);

        Debug.Log($"Игрок получил имплант: {randomImplant.Name}");
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
