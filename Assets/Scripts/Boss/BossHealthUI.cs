using UnityEngine;
using UnityEngine.UI;

public class BossHealthUI : MonoBehaviour
{
    [SerializeField] private Slider bossHealthSlider;
    [SerializeField] private Image fillImage;

    [Header("÷вета")]
    [SerializeField] private Color vulnerableColor = new Color(1f, 0.6f, 0f); // оранжевый
    [SerializeField] private Color shieldedColor = new Color(0.5f, 0.5f, 0.5f); // серый

    private void Awake()
    {
        gameObject.SetActive(false); // скрыт до вызова Show()
    }

    public void SetHealthPercent(float percent)
    {
        bossHealthSlider.value = percent;
    }

    public void SetShielded(bool isShielded)
    {
        if (fillImage != null)
        {
            fillImage.color = isShielded ? shieldedColor : vulnerableColor;
        }
    }

    public void Show()
    {
        gameObject.SetActive(true);
        Debug.Log("BossHealthUI: ѕанель активирована.");
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }
}
