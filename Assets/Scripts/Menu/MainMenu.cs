using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    public GameObject tutorialPanel;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.None;  // Разблокируем курсор
        Cursor.visible = true;  // Показываем курсор
    }

    // Метод для начала игры
    public void StartGame()
    {
        SceneManager.LoadScene("GameScene");
    }

    // Метод для выхода из игры
    public void QuitGame()
    {
        Application.Quit();
    }

    public void OpenTutorial()
    {
        tutorialPanel.SetActive(true);     // Показываем туториал
        gameObject.SetActive(false);       // Скрываем главное меню
    }
}