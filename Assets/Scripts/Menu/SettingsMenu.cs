using UnityEngine;

public class SettingsMenu : MonoBehaviour
{
    public GameObject mainMenu; // ������ �� ������� ����

    // ����� ��� �������� � ������� ����
    public void BackToMainMenu()
    {
        mainMenu.SetActive(true); // ���������� ������� ����
        gameObject.SetActive(false); // �������� ���� ��������
    }
}
