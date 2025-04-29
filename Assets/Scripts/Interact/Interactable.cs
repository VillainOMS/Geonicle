using UnityEngine;
using UnityEngine.Events;

public class Interactable : MonoBehaviour
{
    [SerializeField] private UnityEvent onInteract; // Действие при взаимодействии
    private bool isPlayerNear = false;

    private void Update()
    {
        if (isPlayerNear && Input.GetKeyDown(KeyCode.E))
        {
            onInteract?.Invoke(); // Вызываем событие
            InteractPromptUI.Instance.HidePrompt(); // Прячем подсказку после взаимодействия
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerNear = true;
            InteractPromptUI.Instance.ShowPrompt(); // Показываем подсказку
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerNear = false;
            InteractPromptUI.Instance.HidePrompt(); // Скрываем подсказку
        }
    }
}
