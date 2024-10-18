using UnityEngine;

public class Door : MonoBehaviour
{
    private PlayerMovement player; // ������ �� ������ ������
    [SerializeField] private GameObject greenSign; // ������ �� ������� ��������
    [SerializeField] private GameObject redSign; // ������ �� ������� ��������
    [SerializeField] private Transform[] teleportPoints; // ������ ����� ������������

    private void OnTriggerEnter(Collider other)
    {
        // ��������, ��� ������, �������� � �������, �������� �������
        if (other.gameObject.TryGetComponent(out player))
        {
            OpenDoor();
        }
    }

    private void OpenDoor()
    {
        // ������ ��� ������������ � ��������� �����
        Transform randomTeleportPoint = teleportPoints[Random.Range(0, teleportPoints.Length)];
        player.transform.position = Vector3.zero;
        //player.transform.position = randomTeleportPoint.position; // ���������� ������ �� ��������� �����

        // ����� ����� �������� ������ ��� ��������� �����
        Debug.Log("Teleported to: " + randomTeleportPoint.name);
    }

    public void ShowRedSign()
    {
        // �������� ������� ��������
        redSign.SetActive(true);
        greenSign.SetActive(false);
    }

    public void ShowGreenSign()
    {
        // �������� ������� ��������
        greenSign.SetActive(true);
        redSign.SetActive(false);
    }
}
