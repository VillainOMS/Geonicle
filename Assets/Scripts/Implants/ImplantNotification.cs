using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ImplantNotification : MonoBehaviour
{
    [SerializeField] private Image icon;
    [SerializeField] private Text implantName;
    [SerializeField] private CanvasGroup canvasGroup; // Для плавного исчезновения
    [SerializeField] private float fadeOutTime = 1.5f;

    public void Setup(Sprite implantIcon, string name)
    {
        icon.sprite = implantIcon;
        implantName.text = name;
        StartCoroutine(FadeOut());
    }

    private IEnumerator FadeOut()
    {
        yield return new WaitForSeconds(2f); // Задержка перед исчезновением
        float elapsedTime = 0f;
        while (elapsedTime < fadeOutTime)
        {
            canvasGroup.alpha = Mathf.Lerp(1, 0, elapsedTime / fadeOutTime);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        Destroy(gameObject);
    }
}
