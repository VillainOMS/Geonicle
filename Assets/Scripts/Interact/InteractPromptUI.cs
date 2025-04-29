using UnityEngine;
using UnityEngine.UI;

public class InteractPromptUI : MonoBehaviour
{
    public static InteractPromptUI Instance { get; private set; }

    [SerializeField] private GameObject promptPanel; 

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);

        promptPanel.SetActive(false);
    }

    public void ShowPrompt()
    {
        if (!GameState.IsUIOpen) // Только если интерфейс не открыт
            promptPanel.SetActive(true);
    }

    public void HidePrompt()
    {
        promptPanel.SetActive(false);
    }
}
