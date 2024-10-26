using System;
using UnityEngine;

public class TeleportDoor : MonoBehaviour
{
    public event Action<Transform> OnPlayerTeleport;
    [SerializeField] private Transform[] teleportationPoints; // ������ ����� ������������
    private void OnTriggerEnter(Collider other)
    {
        // ��������, ��� ������, �������� � �������, �������� �������
        if (other.CompareTag("Player"))
        {
            TeleportPlayer(other.transform);
        }
    }

    private void TeleportPlayer(Transform player)
    {
        // ������ ��� ������������ � ��������� �����
        Transform randomTeleportPoint = teleportationPoints[UnityEngine.Random.Range(0, teleportationPoints.Length)];

        // �������� ��������� CharacterController ������
        CharacterController characterController = player.GetComponent<CharacterController>();

        if (characterController != null)
        {
            // ������������� ������, ������������ ��� �������
            characterController.enabled = false; // ��������� CharacterController
            player.position = randomTeleportPoint.position; // ���������� ������ �� ��������� �����
            characterController.enabled = true; // �������� CharacterController �������
            OnPlayerTeleport?.Invoke(randomTeleportPoint);
        }
        else
        {
            Debug.LogError("CharacterController �� ������ �� ������� ������!");
        }
        
        Debug.Log("Teleported to: " + randomTeleportPoint.name);
    }

    

}