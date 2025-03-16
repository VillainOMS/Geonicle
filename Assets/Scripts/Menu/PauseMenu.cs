using System.Collections;
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
    [SerializeField] private GameObject altarUI;
    [SerializeField] private GameObject weaponUI;

    private void Start()
    {
        sfxVolume.onValueChanged.AddListener((Value) => { audioMixer.SetFloat("SFXVolume", -80 + Value * 80); });
        musicVolume.onValueChanged.AddListener((Value) => { audioMixer.SetFloat("MusicVolume", -80 + Value * 80); });
    }


    private bool blockPause = false;

    private void Update()
    {
        if (blockPause) return; 

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (altarUI.activeSelf || weaponUI.activeSelf)
            {
                return; // Если открыто меню алтаря или оружия — не вызываем паузу
            }

            if (pausePanel.activeSelf)
            {
                Resume();
            }
            else
            {
                Pause();
            }
        }
    }

    public void BlockPauseForOneFrame()
    {
        StartCoroutine(ResetPauseBlock());
    }

    private IEnumerator ResetPauseBlock()
    {
        blockPause = true;
        yield return null;
        blockPause = false;
    }

    public void Resume()
    {
        pausePanel.SetActive(false); // Отключаем основной Panel
        pauseMenu.SetActive(false);  // Отключаем меню паузы
        settingsMenu.SetActive(false); // Отключаем меню настроек
        Time.timeScale = 1f;
        Cursor.lockState = CursorLockMode.Locked; // Скрываем курсор
        Cursor.visible = false;
    }

    private void Pause()
    {
        pausePanel.SetActive(true); // Активируем основной Panel
        pauseMenu.SetActive(true);  // Активируем меню паузы
        settingsMenu.SetActive(false); // Отключаем меню настроек
        Time.timeScale = 0f;
        Cursor.lockState = CursorLockMode.None; // Показываем курсор
        Cursor.visible = true;
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
