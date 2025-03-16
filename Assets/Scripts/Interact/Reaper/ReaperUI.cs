using UnityEngine;

public class ReaperUI : MonoBehaviour
{
    [SerializeField] private GameObject reaperPanel;
    [SerializeField] private InventoryManager inventoryManager;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            CloseReaper();
        }
    }

    public void OpenReaper()
    {
        reaperPanel.SetActive(true);
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        Time.timeScale = 0f;

        Debug.Log("������ ���� ������, ��������� UI ���������");
        inventoryManager.UpdateInventoryUI();
    }

    public void CloseReaper()
    {
        reaperPanel.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        Time.timeScale = 1f;
    }
}
