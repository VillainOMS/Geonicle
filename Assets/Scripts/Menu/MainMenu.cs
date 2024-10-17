using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    public GameObject settingsMenu; // ������ �� ���� ��������

    // ����� ��� ������ ����
    public void StartGame()
    {
        SceneManager.LoadScene("GameScene");
    }

    // ����� ��� �������� ���� ��������
    public void OpenSettings()
    {
        settingsMenu.SetActive(true); // ���������� ���� ��������
        gameObject.SetActive(false); // �������� ������� ����
    }

    // ����� ��� ������ �� ����
    public void QuitGame()
    {
        Application.Quit();
    }
}