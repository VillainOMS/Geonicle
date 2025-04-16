using UnityEngine;
using System.Collections.Generic;

public class BossArenaManager : MonoBehaviour
{
    [Header("Boss")]
    [SerializeField] private GameObject bossPrefab;
    [SerializeField] private Transform bossSpawnPoint;
    private BossController boss;

    [Header("Spawn Points")]
    [SerializeField] private SpawnPoint[] spawnPointsPhase1; // 66%
    [SerializeField] private SpawnPoint[] spawnPointsPhase2; // 33%
    [SerializeField] private bool isEliteArena;

    [Header("Victory")]
    [SerializeField] private GameObject victoryInteractable; // Уже на сцене, но выключен

    [Header("Teleport Trigger")]
    [SerializeField] private TeleportDoor enterDoor;
    [SerializeField] private Transform teleportPoint;

    [Header("UI")]
    [SerializeField] private BossHealthUI bossHealthUI;

    private bool arenaActivated = false;
    private bool phase1Triggered = false;
    private bool phase2Triggered = false;
    private bool trophySpawned = false;

    private List<Enemy> activeEnemies = new List<Enemy>();

    private void OnEnable()
    {
        enterDoor.OnPlayerTeleport += OnPlayerTeleport;
    }

    private void OnDisable()
    {
        enterDoor.OnPlayerTeleport -= OnPlayerTeleport;
    }

    private void OnPlayerTeleport(Transform obj)
    {
        if (obj == teleportPoint && !arenaActivated)
        {
            ActivateBossArena();
        }
    }

    private void ActivateBossArena()
    {
        arenaActivated = true;

        if (bossHealthUI != null)
        {
            bossHealthUI.Show();
            Debug.Log("BossArenaManager: Прогрессбар босса активирован.");
        }
        else
        {
            Debug.LogWarning("BossArenaManager: bossHealthUI не назначен в инспекторе!");
        }

        GameObject bossGO = Instantiate(bossPrefab, bossSpawnPoint.position, Quaternion.identity);
        boss = bossGO.GetComponent<BossController>();
        boss.OnBossDamaged += OnBossDamaged;
        boss.OnBossDeath += OnBossDeath;

        Debug.Log("Босс создан и слушатели событий добавлены.");
    }

    private void OnBossDamaged(float hpPercent)
    {
        Debug.Log($"Босс получил урон, текущее ХП: {hpPercent * 100f:F1}%");

        if (!phase1Triggered && hpPercent <= 0.66f)
        {
            TriggerPhase(spawnPointsPhase1);
            phase1Triggered = true;
            Debug.Log("Фаза 1 активирована!");
        }
        if (!phase2Triggered && hpPercent <= 0.33f)
        {
            TriggerPhase(spawnPointsPhase2);
            phase2Triggered = true;
            Debug.Log("Фаза 2 активирована!");
        }

        CheckBossVulnerability();
    }

    private void TriggerPhase(SpawnPoint[] points)
    {
        foreach (var sp in points)
        {
            sp.SpawnEnemy(isEliteArena);
            if (sp.Enemy != null)
            {
                activeEnemies.Add(sp.Enemy);
                sp.Enemy.onDie += () =>
                {
                    activeEnemies.Remove(sp.Enemy);
                    CheckBossVulnerability();
                };
            }
        }

        boss.SetInvulnerable(true);
        Debug.Log("Босс временно неуязвим до победы над врагами.");
    }

    private void CheckBossVulnerability()
    {
        activeEnemies.RemoveAll(e => e == null);
        Debug.Log($"Проверка неуязвимости босса. Осталось врагов: {activeEnemies.Count}");

        if (activeEnemies.Count < 3)
        {
            boss.SetInvulnerable(false);
            Debug.Log("Босс снова уязвим.");
        }
    }

    private void OnBossDeath()
    {
        Debug.Log("Босс убит. Ожидаем уничтожения оставшихся врагов.");
        StartCoroutine(WaitForAllEnemiesAndActivateVictory());
    }

    private System.Collections.IEnumerator WaitForAllEnemiesAndActivateVictory()
    {
        while (activeEnemies.Exists(e => e != null))
        {
            Debug.Log("Ждём смерти оставшихся врагов...");
            yield return null;
        }

        Debug.Log("Все враги побеждены. Активируем победный объект.");

        if (!trophySpawned)
        {
            if (victoryInteractable != null)
            {
                victoryInteractable.SetActive(true);
                Debug.Log("VictoryInteractable активирован!");
            }
            else
            {
                Debug.LogError("VictoryInteractable НЕ ПРИВЯЗАН в инспекторе!");
            }

            trophySpawned = true;
        }
    }
}
