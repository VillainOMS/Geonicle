using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }

    [SerializeField] private GameObject implantNotificationPrefab;
    [SerializeField] private Transform notificationPanel;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void ShowImplantNotification(Implant implant)
    {
        GameObject notification = Instantiate(implantNotificationPrefab, notificationPanel);
        notification.GetComponent<ImplantNotification>().Setup(implant.Icon, implant.Name);
        StartCoroutine(RemoveNotificationAfterDelay(notification));
    }

    private IEnumerator RemoveNotificationAfterDelay(GameObject notification)
    {
        yield return new WaitForSeconds(3f);
        Destroy(notification);
    }
}
