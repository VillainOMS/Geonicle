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
    [SerializeField] private Slider mouseSensitivitySlider;
    [SerializeField] private MouseLook mouseLook;

    private bool isPaused = false;
    private bool blockPause = false;

    private void Start()
    {
        pausePanel.SetActive(false);
        pauseMenu.SetActive(false);
        settingsMenu.SetActive(false);

        // Установка громкости через децибелы
        sfxVolume.onValueChanged.AddListener((value) => {
            audioMixer.SetFloat("SFXVolume", VolumeToDecibels(value));
        });

        musicVolume.onValueChanged.AddListener((value) => {
            audioMixer.SetFloat("MusicVolume", VolumeToDecibels(value));
        });

        mouseSensitivitySlider.onValueChanged.AddListener((value) => {
            if (mouseLook != null)
            {
                float mappedValue = Mathf.Lerp(50f, 300f, value); // Значение от 50 до 200
                mouseLook.SetSensitivity(mappedValue);
            }
        });

    }

    private float VolumeToDecibels(float volume)
    {
        return volume <= 0.0001f ? -80f : Mathf.Log10(volume) * 20f;
    }

    private void Update()
    {
        if (blockPause) return;

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (altarUI.activeSelf || weaponUI.activeSelf || reaperUI.activeSelf)
                return;

            if (isPaused)
                Resume();
            else
                Pause();
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

        StartCoroutine(EnableCursorWithDelay());
    }

    private IEnumerator EnableCursorWithDelay()
    {
        yield return null;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void Resume()
    {
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
