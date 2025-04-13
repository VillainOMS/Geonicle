using System;
using System.Collections.Generic;
using UnityEngine;

public class TeleportDoor : MonoBehaviour
{
    [System.Serializable]
    public class ArenaTier
    {
        public string tierName;
        public Transform[] teleportPoints;
    }

    public event Action<Transform> OnPlayerTeleport;

    [SerializeField] private bool isArenaEntrance = true;
    [SerializeField] private bool skipToBossForTesting = false; // Новое поле
    [SerializeField] private ArenaTier[] arenaTiers;
    [SerializeField] private int wavesPerTier = 3;
    [SerializeField] private Transform bossTeleportPoint;

    private List<int> usedIndexesInTier = new List<int>();
    private int currentTier = 0;
    private int waveInTier = 0;
    private bool isBossFight = false;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            TeleportPlayer(other.transform);
        }
    }

    private void TeleportPlayer(Transform player)
    {
        if (!isArenaEntrance)
        {
            // Телепорт назад
            if (arenaTiers.Length == 0 || arenaTiers[0].teleportPoints.Length == 0)
            {
                Debug.LogWarning("Не указана точка возврата в комнату отдыха!");
                return;
            }

            Transform returnPoint = arenaTiers[0].teleportPoints[0];
            CharacterController controller = player.GetComponent<CharacterController>();

            if (controller != null)
            {
                controller.enabled = false;
                player.position = returnPoint.position;
                controller.enabled = true;
                OnPlayerTeleport?.Invoke(returnPoint);
            }

            Debug.Log("Игрок вернулся в комнату отдыха.");
            return;
        }

        // Новый: пропустить прямо к боссу, если включено
        if (skipToBossForTesting)
        {
            Debug.LogWarning("Тестовый режим: телепортируем сразу к БОССУ!");
            TeleportToBoss(player);
            return;
        }

        // Обычная логика: выбор арены по текущему прогрессу
        if (currentTier >= arenaTiers.Length)
        {
            TeleportToBoss(player);
            return;
        }

        ArenaTier tier = arenaTiers[currentTier];

        List<int> availableIndexes = new List<int>();
        for (int i = 0; i < tier.teleportPoints.Length; i++)
        {
            if (!usedIndexesInTier.Contains(i))
                availableIndexes.Add(i);
        }

        if (availableIndexes.Count == 0)
        {
            Debug.LogWarning("Нет доступных арен в текущем тиру!");
            return;
        }

        int chosenIndex = availableIndexes[UnityEngine.Random.Range(0, availableIndexes.Count)];
        usedIndexesInTier.Add(chosenIndex);
        Transform chosenPoint = tier.teleportPoints[chosenIndex];

        CharacterController characterController = player.GetComponent<CharacterController>();
        if (characterController != null)
        {
            characterController.enabled = false;
            player.position = chosenPoint.position;
            characterController.enabled = true;
            OnPlayerTeleport?.Invoke(chosenPoint);
        }

        waveInTier++;

        Debug.Log($"Игрок телепортирован на арену: {chosenPoint.name} (Тир: {currentTier + 1}, Волна: {waveInTier}/{wavesPerTier})");

        if (waveInTier >= wavesPerTier)
        {
            waveInTier = 0;
            currentTier++;
            usedIndexesInTier.Clear();
            Debug.Log("Переход к следующему тиру арен.");
        }
    }

    private void TeleportToBoss(Transform player)
    {
        isBossFight = true;

        CharacterController characterController = player.GetComponent<CharacterController>();
        if (characterController != null)
        {
            characterController.enabled = false;
            player.position = bossTeleportPoint.position;
            characterController.enabled = true;
            OnPlayerTeleport?.Invoke(bossTeleportPoint);
        }

        Debug.Log("Игрок телепортирован на арену с БОССОМ!");
    }

    public bool IsEliteWave()
    {
        return waveInTier == wavesPerTier - 1 && currentTier < arenaTiers.Length;
    }

    public bool IsBossFight()
    {
        return isBossFight;
    }
}
