using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] private GameObject pausePanel; // Основной Panel для паузы
    [SerializeField] private GameObject pauseMenu; // Панель с кнопками паузы
    [SerializeField] private GameObject settingsMenu; // Панель с настройками
    [SerializeField] private Slider sfxVolume;
    [SerializeField] private Slider musicVolume;
    [SerializeField] private AudioMixer audioMixer;

    private bool isPaused = false;

    private void Start()
    {
        sfxVolume.onValueChanged.AddListener((Value) => { audioMixer.SetFloat("SFXVolume", -80 + Value * 80); });
        musicVolume.onValueChanged.AddListener((Value) => { audioMixer.SetFloat("MusicVolume", -80 + Value * 80); });
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused)
            {
                Resume();
            }
            else
            {
                Pause();
            }
        }
    }

    public void Resume()
    {
        pausePanel.SetActive(false); // Отключаем основной Panel
        pauseMenu.SetActive(false);  // Отключаем меню паузы
        settingsMenu.SetActive(false); // Отключаем меню настроек
        Time.timeScale = 1f;
        Cursor.lockState = CursorLockMode.Locked; // Скрываем курсор
        Cursor.visible = false;
        isPaused = false;
    }

    private void Pause()
    {
        pausePanel.SetActive(true); // Активируем основной Panel
        pauseMenu.SetActive(true);  // Активируем меню паузы
        settingsMenu.SetActive(false); // Отключаем меню настроек
        Time.timeScale = 0f;
        Cursor.lockState = CursorLockMode.None; // Показываем курсор
        Cursor.visible = true;
        isPaused = true;
    }

    public void OpenSettings()
    {
        pauseMenu.SetActive(false); // Скрываем меню паузы
        settingsMenu.SetActive(true); // Показываем меню настроек
    }

    public void CloseSettings()
    {
        settingsMenu.SetActive(false); // Скрываем меню настроек
        pauseMenu.SetActive(true); // Показываем меню паузы
    }

    public void LoadMainMenu()
    {
        Time.timeScale = 1f; // Возвращаем нормальный ход времени перед загрузкой
        SceneManager.LoadScene("MainMenu");
    }
}
