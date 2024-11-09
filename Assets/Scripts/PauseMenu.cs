using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] private GameObject pausePanel; // �������� Panel ��� �����
    [SerializeField] private GameObject pauseMenu; // ������ � �������� �����
    [SerializeField] private GameObject settingsMenu; // ������ � �����������
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
        pausePanel.SetActive(false); // ��������� �������� Panel
        pauseMenu.SetActive(false);  // ��������� ���� �����
        settingsMenu.SetActive(false); // ��������� ���� ��������
        Time.timeScale = 1f;
        Cursor.lockState = CursorLockMode.Locked; // �������� ������
        Cursor.visible = false;
        isPaused = false;
    }

    private void Pause()
    {
        pausePanel.SetActive(true); // ���������� �������� Panel
        pauseMenu.SetActive(true);  // ���������� ���� �����
        settingsMenu.SetActive(false); // ��������� ���� ��������
        Time.timeScale = 0f;
        Cursor.lockState = CursorLockMode.None; // ���������� ������
        Cursor.visible = true;
        isPaused = true;
    }

    public void OpenSettings()
    {
        pauseMenu.SetActive(false); // �������� ���� �����
        settingsMenu.SetActive(true); // ���������� ���� ��������
    }

    public void CloseSettings()
    {
        settingsMenu.SetActive(false); // �������� ���� ��������
        pauseMenu.SetActive(true); // ���������� ���� �����
    }

    public void LoadMainMenu()
    {
        Time.timeScale = 1f; // ���������� ���������� ��� ������� ����� ���������
        SceneManager.LoadScene("MainMenu");
    }
}
