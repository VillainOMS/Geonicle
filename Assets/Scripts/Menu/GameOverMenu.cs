using UnityEngine;
using UnityEngine.UI;

public class GameOverMenu : MonoBehaviour
{
    [SerializeField] private GameObject gameOverPanel;
    [SerializeField] private GameObject victoryText; // UI-������� "�� ��������!"

    private void Start()
    {
        gameOverPanel.SetActive(false);
        if (victoryText != null)
            victoryText.SetActive(false);
    }

    public void ShowGameOverScreen()
    {
        gameOverPanel.SetActive(true);
        if (victoryText != null)
            victoryText.SetActive(false); // ��������, ���� ��� ������

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        Time.timeScale = 0f;
    }

    public void ShowVictoryScreen()
    {
        gameOverPanel.SetActive(true);
        if (victoryText != null)
            victoryText.SetActive(true); // �������� �������� �����

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        Time.timeScale = 0f;
    }

    public void HideGameOverScreen()
    {
        gameOverPanel.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        Time.timeScale = 1f;
    }
}
