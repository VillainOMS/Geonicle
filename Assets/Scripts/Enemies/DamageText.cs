using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using TMPro;

public class DamageText : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI damageText;
    [SerializeField] private float fadeOutTime = 1f;

    public void SetText(int damage)
    {
        damageText.text = damage.ToString();
        StartCoroutine(FadeOut());
    }

    private IEnumerator FadeOut()
    {
        float elapsedTime = 0f;
        Color originalColor = damageText.color;

        while (elapsedTime < fadeOutTime)
        {
            damageText.color = new Color(originalColor.r, originalColor.g, originalColor.b, 1 - (elapsedTime / fadeOutTime));
            transform.position += Vector3.up * Time.deltaTime; // Текст поднимается вверх
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        Destroy(gameObject);
    }
}
