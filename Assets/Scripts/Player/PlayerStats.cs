using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class PlayerStats : MonoBehaviour
{
    public static PlayerStats Instance { get; private set; }

    [Header("Базовые характеристики")]
    public float baseDamageMultiplier = 1.0f;
    public int baseMaxHealth = 100;
    public float baseMoveSpeed = 3f;
    public float baseAttackSpeedMultiplier = 1.0f;

    [Header("Фактические характеристики (учитывают импланты и аспекты)")]
    public float actualDamageMultiplier;
    public int actualMaxHealth;
    public float actualMoveSpeed;
    public float actualAttackSpeedMultiplier;

    [Header("Отладочные показатели здоровья")]
    [SerializeField] private int currentHealth;
    [SerializeField] private float currentHealthPercentage;

    [SerializeField] private Text healthText;
    [SerializeField] private Slider healthBar;
    [SerializeField] private GameObject gameOverPanel;

    [Header("Визуальный оверлей (урон / лечение)")]
    [SerializeField] private Image damageOverlayImage;
    [SerializeField] private float overlayDuration = 0.2f;
    [SerializeField] private float overlayFadeSpeed = 4f;
    private Coroutine damageOverlayCoroutine;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    private void Start()
    {
        RecalculateActualStats();
        currentHealth = actualMaxHealth;
        UpdateHealthBar();

        if (gameOverPanel != null)
            gameOverPanel.SetActive(false);

        if (damageOverlayImage != null)
        {
            damageOverlayImage.color = new Color(1, 0, 0, 0);
            damageOverlayImage.gameObject.SetActive(false);
        }
    }

    public void RestartGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void LoadMainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenu");
    }

    public void TakeDamage(int damage)
    {
        if (PlayerInventory.Instance.GetPlayerAbilities().IsInvulnerable)
        {
            Debug.Log("Урон заблокирован: игрок неуязвим.");
            return;
        }

        if (PlayerInventory.Instance.GetPlayerAbilities().TryBlockDamage())
        {
            return;
        }

        currentHealth -= damage;
        currentHealth = Mathf.Clamp(currentHealth, 0, actualMaxHealth);
        UpdateHealthBar();
        AudioManager.Instance.PlayPlayerHurtSound();

        TriggerOverlay(Color.red, 4f);

        if (currentHealth <= 0)
            Die();
    }

    public void Heal(int amount)
    {
        currentHealth += amount;
        currentHealth = Mathf.Clamp(currentHealth, 0, actualMaxHealth);
        UpdateHealthBar();
        Debug.Log($"Игрок исцелен на {amount}. Текущее HP: {currentHealth}");

        TriggerOverlay(Color.green, 0.5f);
    }

    private void TriggerOverlay(Color flashColor, float fadeSpeed)
    {
        if (damageOverlayImage == null)
            return;

        if (damageOverlayCoroutine != null)
            StopCoroutine(damageOverlayCoroutine);

        damageOverlayCoroutine = StartCoroutine(FlashOverlay(flashColor, fadeSpeed));
    }


    private IEnumerator FlashOverlay(Color flashColor, float fadeSpeed)
    {
        damageOverlayImage.gameObject.SetActive(true);
        damageOverlayImage.color = new Color(flashColor.r, flashColor.g, flashColor.b, 0.1f); 

        yield return new WaitForSeconds(overlayDuration);

        while (damageOverlayImage.color.a > 0)
        {
            Color c = damageOverlayImage.color;
            c.a -= Time.deltaTime * fadeSpeed;
            damageOverlayImage.color = c;
            yield return null;
        }

        damageOverlayImage.gameObject.SetActive(false);
    }


    private void Die()
    {
        if (gameOverPanel != null)
            gameOverPanel.SetActive(true);

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        Time.timeScale = 0f;

        if (damageOverlayCoroutine != null)
            StopCoroutine(damageOverlayCoroutine);

        if (damageOverlayImage != null)
        {
            damageOverlayImage.color = new Color(1, 0, 0, 0);
            damageOverlayImage.gameObject.SetActive(false);
        }
    }

    public int GetCurrentHealth()
    {
        return currentHealth;
    }

    public void UpdateHealthBar()
    {
        if (healthBar != null)
            healthBar.value = (float)currentHealth / actualMaxHealth;

        if (healthText != null)
            healthText.text = $"{currentHealth}/{actualMaxHealth}";

        RectTransform rt = healthBar.GetComponent<RectTransform>();
        if (rt != null)
        {
            float baseWidth = 350f;
            float newWidth = baseWidth * ((float)actualMaxHealth / 100f);
            rt.sizeDelta = new Vector2(newWidth, rt.sizeDelta.y);
        }
    }

    public void ApplyAspectBonuses(int fireLevel, int metalLevel, int waterLevel, int shockLevel)
    {
        baseDamageMultiplier = 1.0f + fireLevel * PlayerAspects.Instance.GetFireImpact();
        baseMaxHealth = 100 + Mathf.RoundToInt(metalLevel * PlayerAspects.Instance.GetMetalImpact() * 100);
        baseMoveSpeed = 3.0f + waterLevel * PlayerAspects.Instance.GetWaterImpact();
        baseAttackSpeedMultiplier = 1.0f + shockLevel * PlayerAspects.Instance.GetShockImpact();

        RecalculateActualStats();
    }

    public void RecalculateActualStats()
    {
        currentHealthPercentage = (actualMaxHealth > 0) ? (float)currentHealth / actualMaxHealth : 1f;

        actualDamageMultiplier = baseDamageMultiplier;
        actualMaxHealth = baseMaxHealth;
        actualMoveSpeed = baseMoveSpeed;
        actualAttackSpeedMultiplier = baseAttackSpeedMultiplier;

        float totalDamageBonus = 0f;
        float totalHealthBonus = 0f;
        float totalSpeedBonus = 0f;
        float totalAttackSpeedBonus = 0f;

        foreach (Implant implant in PlayerInventory.Instance.GetEquippedImplants())
        {
            implant.SetEnhanced(implant.CheckIfEnhanced());

            totalDamageBonus += implant.GetDamageBonus();
            totalHealthBonus += implant.GetHealthBonus();
            totalSpeedBonus += implant.GetSpeedBonus();
            totalAttackSpeedBonus += implant.GetAttackSpeedBonus();

            if (implant.IsEnhanced)
            {
                totalDamageBonus += implant.GetEnhancedDamageBonus();
                totalHealthBonus += implant.GetEnhancedHealthBonus();
                totalSpeedBonus += implant.GetEnhancedSpeedBonus();
                totalAttackSpeedBonus += implant.GetEnhancedAttackSpeedBonus();

                implant.ApplyEnhancedEffect(this);
            }
        }

        actualDamageMultiplier *= (1 + totalDamageBonus);
        actualMaxHealth = Mathf.RoundToInt(baseMaxHealth * (1 + totalHealthBonus));
        actualMoveSpeed *= (1 + totalSpeedBonus);
        actualAttackSpeedMultiplier *= (1 + totalAttackSpeedBonus);

        currentHealth = Mathf.Clamp(Mathf.RoundToInt(actualMaxHealth * currentHealthPercentage), 1, actualMaxHealth);

        UpdateHealthBar();
    }
}
