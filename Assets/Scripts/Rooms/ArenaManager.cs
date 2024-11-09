using UnityEngine;

public class ArenaManager : MonoBehaviour
{
    [SerializeField] private SpawnPoint[] spawnPoints; // Массив точек спавна врагов на арене
    [SerializeField] private GameObject exitDoor; // Дверь выхода с арены
    [SerializeField] private TeleportDoor enterDoor; // Дверь входа на арену
    [SerializeField] private Transform teleportPoint; // Дверь входа на арену

    private BoxCollider exitCollider;

    private void Start()
    {
        // Получаем компонент BoxCollider двери и отключаем его
        exitCollider = exitDoor.GetComponent<BoxCollider>();
        if (exitCollider != null)
        {
            exitCollider.enabled = false; // Сначала отключаем выход
        }
    }

    private void ActivateArena()
    {
        // Активируем врагов на всех точках спавна
        foreach (var spawnPoint in spawnPoints)
        {
            spawnPoint.SpawnEnemy();
        }

        if (exitCollider != null)
        {
            exitCollider.enabled = false; // Закрываем выход
        }
        Debug.Log(exitCollider.enabled);
    }

    private void Update()
    {
        // Проверяем, остались ли активные враги на арене
        bool areEnemiesRemaining = false;

        foreach (var spawnPoint in spawnPoints)
        {
            if (spawnPoint.Enemy != null)
            {
                areEnemiesRemaining = true;
                break;
            }
        }

        // Активируем BoxCollider выхода, если все враги побеждены
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
