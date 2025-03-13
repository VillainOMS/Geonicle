using UnityEngine;

public class ArenaManager : MonoBehaviour
{
    [SerializeField] private SpawnPoint[] spawnPoints;
    [SerializeField] private GameObject exitDoor;
    [SerializeField] private TeleportDoor enterDoor;
    [SerializeField] private Transform teleportPoint;

    private BoxCollider exitCollider;
    private bool hasGivenAspectPoint = false; // ����, ����� ��������� ���� ���� ���
    private bool arenaActivated = false; // ����, ����� ����� �� ��������� ���� �����

    private void Start()
    {
        exitCollider = exitDoor.GetComponent<BoxCollider>();
        if (exitCollider != null)
        {
            exitCollider.enabled = false;
        }

        hasGivenAspectPoint = false;
        arenaActivated = false; // ����� �� ������������ ��� ������ �����
    }

    private void ActivateArena()
    {
        foreach (var spawnPoint in spawnPoints)
        {
            spawnPoint.SpawnEnemy();
        }

        if (exitCollider != null)
        {
            exitCollider.enabled = false;
        }

        hasGivenAspectPoint = false;
        arenaActivated = true; // ������ ����� ��������� ��������
    }

    private void Update()
    {
        if (!arenaActivated) return; // ���� ����� �� ������������, ������ �� ������

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
        }
        else
        {
            exitCollider.enabled = false;
        }
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
