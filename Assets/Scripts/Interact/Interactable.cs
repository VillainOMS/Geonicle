using UnityEngine;
using UnityEngine.Events;

public class Interactable : MonoBehaviour
{
    [SerializeField] private UnityEvent onInteract; // �������� ��� ��������������
    private bool isPlayerNear = false;

    private void Update()
    {
        if (isPlayerNear && Input.GetKeyDown(KeyCode.E))
        {
            onInteract?.Invoke(); // �������� �������
            InteractPromptUI.Instance.HidePrompt(); // ������ ��������� ����� ��������������
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerNear = true;
            InteractPromptUI.Instance.ShowPrompt(); // ���������� ���������
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerNear = false;
            InteractPromptUI.Instance.HidePrompt(); // �������� ���������
        }
    }
}
