using UnityEngine;

public class Door : MonoBehaviour
{
    private PlayerMovement player; // Ссылка на объект игрока
    [SerializeField] private GameObject greenSign; // Ссылка на зеленую табличку
    [SerializeField] private GameObject redSign; // Ссылка на красную табличку
    [SerializeField] private Transform[] teleportPoints; // Массив точек телепортации

    private void OnTriggerEnter(Collider other)
    {
        // Проверка, что объект, вошедший в триггер, является игроком
        if (other.gameObject.TryGetComponent(out player))
        {
            OpenDoor();
        }
    }

    private void OpenDoor()
    {
        // Логика для телепортации в случайную точку
        Transform randomTeleportPoint = teleportPoints[Random.Range(0, teleportPoints.Length)];
        player.transform.position = Vector3.zero;
        //player.transform.position = randomTeleportPoint.position; // Перемещаем игрока на случайную точку

        // Здесь можно добавить логику для активации арены
        Debug.Log("Teleported to: " + randomTeleportPoint.name);
    }

    public void ShowRedSign()
    {
        // Показать красную табличку
        redSign.SetActive(true);
        greenSign.SetActive(false);
    }

    public void ShowGreenSign()
    {
        // Показать зеленую табличку
        greenSign.SetActive(true);
        redSign.SetActive(false);
    }
}
