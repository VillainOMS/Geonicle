using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    public GameObject tutorialPanel;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.None;  // ������������ ������
        Cursor.visible = true;  // ���������� ������
    }

    // ����� ��� ������ ����
    public void StartGame()
    {
        SceneManager.LoadScene("GameScene");
    }

    // ����� ��� ������ �� ����
    public void QuitGame()
    {
        Application.Quit();
    }

    public void OpenTutorial()
    {
        tutorialPanel.SetActive(true);     // ���������� ��������
        gameObject.SetActive(false);       // �������� ������� ����
    }
}