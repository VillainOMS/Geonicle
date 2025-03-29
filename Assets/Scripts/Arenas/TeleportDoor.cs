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
    [SerializeField] private ArenaTier[] arenaTiers;         // Тиры арен
    [SerializeField] private int wavesPerTier = 3;           // Боев на один тир
    [SerializeField] private Transform bossTeleportPoint;    // Точка телепорта к боссу

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
            // Просто телепорт назад в хаб
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

        // Если все тиры пройдены — телепортируем на арену босса
        if (currentTier >= arenaTiers.Length)
        {
            TeleportToBoss(player);
            return;
        }

        ArenaTier tier = arenaTiers[currentTier];

        // Выбираем случайную арену, которая ещё не использовалась в этом тиру
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

        // Телепортируем игрока
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

        // Переход к следующему тиру, если все волны в этом тиру завершены
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

    // Метод для запроса: элитная ли волна?
    public bool IsEliteWave()
    {
        return waveInTier == wavesPerTier - 1 && currentTier < arenaTiers.Length;
    }

    // Метод для запроса: это бой с боссом?
    public bool IsBossFight()
    {
        return isBossFight;
    }
}
