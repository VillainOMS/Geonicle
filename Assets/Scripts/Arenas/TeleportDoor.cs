using System;
using UnityEngine;

public class TeleportDoor : MonoBehaviour
{
    public event Action<Transform> OnPlayerTeleport;
    [SerializeField] private Transform[] teleportationPoints; // Массив точек телепортации
    private void OnTriggerEnter(Collider other)
    {
        // Проверка, что объект, вошедший в триггер, является игроком
        if (other.CompareTag("Player"))
        {
            TeleportPlayer(other.transform);
        }
    }

    private void TeleportPlayer(Transform player)
    {
        // Логика для телепортации в случайную точку
        Transform randomTeleportPoint = teleportationPoints[UnityEngine.Random.Range(0, teleportationPoints.Length)];

        // Получаем компонент CharacterController игрока
        CharacterController characterController = player.GetComponent<CharacterController>();

        if (characterController != null)
        {
            // Телепортируем игрока, устанавливая его позицию
            characterController.enabled = false; // Отключаем CharacterController
            player.position = randomTeleportPoint.position; // Перемещаем игрока на случайную точку
            characterController.enabled = true; // Включаем CharacterController обратно
            OnPlayerTeleport?.Invoke(randomTeleportPoint);
        }
        else
        {
            Debug.LogError("CharacterController не найден на объекте игрока!");
        }
        
        Debug.Log("Teleported to: " + randomTeleportPoint.name);
    }

    

}