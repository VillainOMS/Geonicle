using UnityEngine;

public class ArenaManager : MonoBehaviour
{
    [SerializeField] private SpawnPoint[] spawnPoints; // ������ ����� ������ ������ �� �����
    [SerializeField] private GameObject exitDoor; // ����� ������ � �����
    [SerializeField] private TeleportDoor enterDoor; // ����� ����� �� �����
    [SerializeField] private Transform teleportPoint; // ����� ����� �� �����

    private BoxCollider exitCollider;

    private void Start()
    {
        // �������� ��������� BoxCollider ����� � ��������� ���
        exitCollider = exitDoor.GetComponent<BoxCollider>();
        if (exitCollider != null)
        {
            exitCollider.enabled = false; // ������� ��������� �����
        }
    }

    private void ActivateArena()
    {
        // ���������� ������ �� ���� ������ ������
        foreach (var spawnPoint in spawnPoints)
        {
            spawnPoint.SpawnEnemy();
        }

        if (exitCollider != null)
        {
            exitCollider.enabled = false; // ��������� �����
        }
        Debug.Log(exitCollider.enabled);
    }

    private void Update()
    {
        // ���������, �������� �� �������� ����� �� �����
        bool areEnemiesRemaining = false;

        foreach (var spawnPoint in spawnPoints)
        {
            if (spawnPoint.Enemy != null)
            {
                areEnemiesRemaining = true;
                break;
            }
        }

        // ���������� BoxCollider ������, ���� ��� ����� ���������
        if (!areEnemiesRemaining && exitCollider != null)
        {
            exitCollider.enabled = true;
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
