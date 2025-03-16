using UnityEngine;

public class Altar : MonoBehaviour
{
    [SerializeField] private GameObject altarUI; // Меню прокачки аспектов

    private void Start()
    {
        altarUI.SetActive(false); // Скрываем при старте
    }

    public void OpenAltarMenu()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        altarUI.SetActive(true);
        Time.timeScale = 0f;
    }

    public void CloseMenu()
    {
        altarUI.SetActive(false);
        Time.timeScale = 1f;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
}
