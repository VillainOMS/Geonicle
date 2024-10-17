using UnityEngine;

public class SettingsMenu : MonoBehaviour
{
    public GameObject mainMenu; // Ссылка на главное меню

    // Метод для возврата в главное меню
    public void BackToMainMenu()
    {
        mainMenu.SetActive(true); // Показываем главное меню
        gameObject.SetActive(false); // Скрываем меню настроек
    }
}
