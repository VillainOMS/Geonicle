using UnityEngine;

public class Altar : MonoBehaviour
{
    [SerializeField] private GameObject altarUI; // ���� �������� ��������

    private void Start()
    {
        altarUI.SetActive(false); // �������� ��� ������
    }

    public void OpenAltarMenu()
    {
        GameState.IsUIOpen = true;
        InteractPromptUI.Instance.HidePrompt();
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        altarUI.SetActive(true);
        Time.timeScale = 0f;
    }

    public void CloseMenu()
    {
        GameState.IsUIOpen = false;
        altarUI.SetActive(false);
        Time.timeScale = 1f;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
}
