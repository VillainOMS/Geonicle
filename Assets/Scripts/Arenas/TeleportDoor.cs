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
    [SerializeField] private ArenaTier[] arenaTiers;         // ���� ����
    [SerializeField] private int wavesPerTier = 3;           // ���� �� ���� ���
    [SerializeField] private Transform bossTeleportPoint;    // ����� ��������� � �����

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
            // ������ �������� ����� � ���
            if (arenaTiers.Length == 0 || arenaTiers[0].teleportPoints.Length == 0)
            {
                Debug.LogWarning("�� ������� ����� �������� � ������� ������!");
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

            Debug.Log("����� �������� � ������� ������.");
            return;
        }

        // ���� ��� ���� �������� � ������������� �� ����� �����
        if (currentTier >= arenaTiers.Length)
        {
            TeleportToBoss(player);
            return;
        }

        ArenaTier tier = arenaTiers[currentTier];

        // �������� ��������� �����, ������� ��� �� �������������� � ���� ����
        List<int> availableIndexes = new List<int>();
        for (int i = 0; i < tier.teleportPoints.Length; i++)
        {
            if (!usedIndexesInTier.Contains(i))
                availableIndexes.Add(i);
        }

        if (availableIndexes.Count == 0)
        {
            Debug.LogWarning("��� ��������� ���� � ������� ����!");
            return;
        }

        int chosenIndex = availableIndexes[UnityEngine.Random.Range(0, availableIndexes.Count)];
        usedIndexesInTier.Add(chosenIndex);

        Transform chosenPoint = tier.teleportPoints[chosenIndex];

        // ������������� ������
        CharacterController characterController = player.GetComponent<CharacterController>();
        if (characterController != null)
        {
            characterController.enabled = false;
            player.position = chosenPoint.position;
            characterController.enabled = true;
            OnPlayerTeleport?.Invoke(chosenPoint);
        }

        waveInTier++;

        Debug.Log($"����� �������������� �� �����: {chosenPoint.name} (���: {currentTier + 1}, �����: {waveInTier}/{wavesPerTier})");

        // ������� � ���������� ����, ���� ��� ����� � ���� ���� ���������
        if (waveInTier >= wavesPerTier)
        {
            waveInTier = 0;
            currentTier++;
            usedIndexesInTier.Clear();
            Debug.Log("������� � ���������� ���� ����.");
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

        Debug.Log("����� �������������� �� ����� � ������!");
    }

    // ����� ��� �������: ������� �� �����?
    public bool IsEliteWave()
    {
        return waveInTier == wavesPerTier - 1 && currentTier < arenaTiers.Length;
    }

    // ����� ��� �������: ��� ��� � ������?
    public bool IsBossFight()
    {
        return isBossFight;
    }
}
