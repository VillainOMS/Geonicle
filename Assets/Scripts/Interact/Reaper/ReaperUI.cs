using UnityEngine;

public class ReaperUI : MonoBehaviour
{
    [SerializeField] private GameObject reaperPanel; // Панель интерфейса рипера
    [SerializeField] private InventoryManager inventoryManager;

    private bool isOpen = false;

    private void Start()
    {
        if (reaperPanel == null)
        {
            Debug.LogError("ReaperUI: Не назначен reaperPanel!");
            return;
        }

        // Убеждаемся, что ReaperUI активен, но панель скрыта
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
        if (GameState.IsUIOpen) return; // Проверяем, не открыто ли другое меню

        GameState.IsUIOpen = true;
        isOpen = true;

        reaperPanel.SetActive(true);
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        Time.timeScale = 0f;

        Debug.Log("Открыт стол рипера, обновляем UI инвентаря");
        inventoryManager.UpdateInventoryUI();
    }

    public void CloseReaper()
    {
        if (!isOpen) return; // Проверка, чтобы не закрывать уже выключенное меню

        isOpen = false;
        reaperPanel.SetActive(false);
        GameState.IsUIOpen = false;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        Time.timeScale = 1f;
    }
}
