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
