using System.Collections;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] private GameObject pausePanel;
    [SerializeField] private GameObject pauseMenu;
    [SerializeField] private GameObject settingsMenu;
    [SerializeField] private Slider sfxVolume;
    [SerializeField] private Slider musicVolume;
    [SerializeField] private AudioMixer audioMixer;
    [SerializeField] private GameObject altarUI;
    [SerializeField] private GameObject weaponUI;
    [SerializeField] private GameObject reaperUI;

    private bool isPaused = false;
    private bool blockPause = false;

    private void Start()
    {
        pausePanel.SetActive(false);
        pauseMenu.SetActive(false);
        settingsMenu.SetActive(false);

        sfxVolume.onValueChanged.AddListener((Value) => { audioMixer.SetFloat("SFXVolume", -80 + Value * 80); });
        musicVolume.onValueChanged.AddListener((Value) => { audioMixer.SetFloat("MusicVolume", -80 + Value * 80); });
    }

    private void Update()
    {
        if (blockPause) return;

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (altarUI.activeSelf || weaponUI.activeSelf || reaperUI.activeSelf)
            {
                return;
            }

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

    public void Pause()
    {
        Debug.Log($"PauseMenu (PAUSE): panel={pausePanel}, menu={pauseMenu}, settings={settingsMenu}");

        if (pausePanel == null || pauseMenu == null || settingsMenu == null)
        {
            Debug.LogError("PauseMenu: One or more UI elements are not assigned!");
            return;
        }

        isPaused = true;
        Time.timeScale = 0f;

        pausePanel.SetActive(true);
        pauseMenu.SetActive(true);
        settingsMenu.SetActive(false);

        // Принудительная задержка перед включением курсора
        StartCoroutine(EnableCursorWithDelay());

        Debug.Log($"PauseMenu (AFTER PAUSE): panel={pausePanel.activeSelf}, menu={pauseMenu.activeSelf}, settings={settingsMenu.activeSelf}, timeScale={Time.timeScale}");
    }

    private IEnumerator EnableCursorWithDelay()
    {
        yield return null; // Ждём кадр, чтобы убедиться, что пауза точно активирована
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        Debug.Log($"PauseMenu: Курсор активирован! lockState={Cursor.lockState}, visible={Cursor.visible}");
    }

    public void Resume()
    {
        Debug.Log($"PauseMenu (RESUME): panel={pausePanel}, menu={pauseMenu}, settings={settingsMenu}");

        if (pausePanel == null || pauseMenu == null || settingsMenu == null)
        {
            Debug.LogError("PauseMenu: One or more UI elements are not assigned!");
            return;
        }

        isPaused = false;
        Time.timeScale = 1f;

        pausePanel.SetActive(false);
        pauseMenu.SetActive(false);
        settingsMenu.SetActive(false);

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        Debug.Log($"PauseMenu (AFTER RESUME): panel={pausePanel.activeSelf}, menu={pauseMenu.activeSelf}, settings={settingsMenu.activeSelf}, timeScale={Time.timeScale}, lockState={Cursor.lockState}");
    }

    public void OpenSettings()
    {
        pauseMenu.SetActive(false);
        settingsMenu.SetActive(true);
    }

    public void CloseSettings()
    {
        settingsMenu.SetActive(false);
        pauseMenu.SetActive(true);
    }

    public void LoadMainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenu");
    }
}
