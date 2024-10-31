using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    public GameObject settingsMenu; // Ссылка на меню настроек

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

    // Метод для открытия меню настроек
    public void OpenSettings()
    {
        settingsMenu.SetActive(true); // Показываем меню настроек
        gameObject.SetActive(false); // Скрываем главное меню
    }

    // Метод для выхода из игры
    public void QuitGame()
    {
        Application.Quit();
    }
}