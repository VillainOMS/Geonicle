using UnityEngine;

public class ReaperUI : MonoBehaviour
{
    [SerializeField] private GameObject reaperPanel; // ������ ���������� ������
    [SerializeField] private InventoryManager inventoryManager;

    private bool isOpen = false;

    private void Start()
    {
        if (reaperPanel == null)
        {
            Debug.LogError("ReaperUI: �� �������� reaperPanel!");
            return;
        }

        // ����������, ��� ReaperUI �������, �� ������ ������
        gameObject.SetActive(true);
        reaperPanel.SetActive(false);
    }

    private void Update()
    {
        if (isOpen && Input.GetKeyDown(KeyCode.Escape))
        {
            CloseReaper();
        }
    }

    public void OpenReaper()
    {
        if (GameState.IsUIOpen) return; // ���������, �� ������� �� ������ ����

        GameState.IsUIOpen = true;
        isOpen = true;

        reaperPanel.SetActive(true);
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        Time.timeScale = 0f;

        Debug.Log("������ ���� ������, ��������� UI ���������");
        inventoryManager.UpdateInventoryUI();
    }

    public void CloseReaper()
    {
        if (!isOpen) return; // ��������, ����� �� ��������� ��� ����������� ����

        isOpen = false;
        reaperPanel.SetActive(false);
        GameState.IsUIOpen = false;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        Time.timeScale = 1f;
    }
}
