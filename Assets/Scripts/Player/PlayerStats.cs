using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerStats : MonoBehaviour
{
    public static PlayerStats Instance { get; private set; }

    // Базовые показатели (изменяются при прокачке аспектов)
    [Header("Базовые характеристики")]
    [SerializeField] public float baseDamageMultiplier = 1.0f;
    [SerializeField] public int baseMaxHealth = 100;
    [SerializeField] public float baseMoveSpeed = 2.7f;
    [SerializeField] public float baseAttackSpeedMultiplier = 1.0f;

    // Фактические показатели (учитывают импланты)
    [Header("Фактические характеристики (автоматически пересчитываются)")]
    [SerializeField] public float actualDamageMultiplier;
    [SerializeField] public int actualMaxHealth;
    [SerializeField] public float actualMoveSpeed;
    [SerializeField] public float actualAttackSpeedMultiplier;

    [SerializeField] private Slider healthBar;
    [SerializeField] private GameObject gameOverPanel;

    private int currentHealth;

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

    private void Start()
    {
        currentHealth = baseMaxHealth;
        UpdateHealthBar();
        if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(false);
        }
        RecalculateActualStats(); // Пересчёт фактических характеристик при старте
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        currentHealth = Mathf.Clamp(currentHealth, 0, actualMaxHealth);
        UpdateHealthBar();
        AudioManager.Instance.PlayPlayerHurtSound();

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        Debug.Log("Игрок погиб!");

        if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(true);
        }
        else
        {
            Debug.LogError("PlayerStats: Не удалось показать GameOver — объект не найден!");
        }

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        Time.timeScale = 0f;
    }

    public void ApplyPercentageBonus(float damageBonus, float healthBonus, float speedBonus, float attackSpeedBonus)
    {
        actualDamageMultiplier *= (1 + damageBonus);

        int oldMaxHealth = actualMaxHealth;
        actualMaxHealth = Mathf.RoundToInt(actualMaxHealth * (1 + healthBonus));

        int healthDiff = actualMaxHealth - oldMaxHealth;
        if (healthDiff > 0)
        {
            currentHealth += healthDiff;
        }
        else if (healthDiff < 0)
        {
            currentHealth = Mathf.Max(1, currentHealth + healthDiff);
        }

        actualMoveSpeed *= (1 + speedBonus);
        actualAttackSpeedMultiplier *= (1 + attackSpeedBonus);
    }

    public void RemovePercentageBonus(float damageBonus, float healthBonus, float speedBonus, float attackSpeedBonus)
    {
        actualDamageMultiplier /= (1 + damageBonus);

        int oldMaxHealth = actualMaxHealth;
        actualMaxHealth = Mathf.RoundToInt(actualMaxHealth / (1 + healthBonus));

        int healthDiff = actualMaxHealth - oldMaxHealth;
        if (healthDiff < 0)
        {
            currentHealth = Mathf.Max(1, currentHealth + healthDiff);
        }

        actualMoveSpeed /= (1 + speedBonus);
        actualAttackSpeedMultiplier /= (1 + attackSpeedBonus);
    }

    public void UpdateHealthBar()
    {
        if (healthBar != null)
        {
            healthBar.value = (float)currentHealth / actualMaxHealth;
        }
    }

    public void ApplyAspectBonuses(int fireLevel, int metalLevel, int waterLevel, int shockLevel)
    {
        baseDamageMultiplier = 1.0f + fireLevel * PlayerAspects.Instance.GetFireImpact();
        baseMaxHealth = 100 + Mathf.RoundToInt(metalLevel * PlayerAspects.Instance.GetMetalImpact() * 100);
        baseMoveSpeed = 3.0f + waterLevel * PlayerAspects.Instance.GetWaterImpact();
        baseAttackSpeedMultiplier = 1.0f + shockLevel * PlayerAspects.Instance.GetShockImpact();

        foreach (Implant implant in PlayerInventory.Instance.GetEquippedImplants())
        {
            bool wasEnhanced = implant.IsEnhanced;
            bool isNowEnhanced = implant.CheckIfEnhanced();
            implant.SetEnhanced(isNowEnhanced);

            if (!wasEnhanced && isNowEnhanced)
            {
                implant.ApplyEnhancedEffect(this);
            }
            else if (wasEnhanced && !isNowEnhanced)
            {
                implant.RemoveEnhancedEffect(this);
            }
        }

        RecalculateActualStats();
    }

    public void RecalculateActualStats()
    {
        Debug.Log("=== Пересчёт характеристик ===");

        baseDamageMultiplier = 1.0f + PlayerAspects.Instance.GetFireLevel() * PlayerAspects.Instance.GetFireImpact();
        baseMaxHealth = 100 + Mathf.RoundToInt(PlayerAspects.Instance.GetMetalLevel() * PlayerAspects.Instance.GetMetalImpact() * 100);
        baseMoveSpeed = 3.0f + PlayerAspects.Instance.GetWaterLevel() * PlayerAspects.Instance.GetWaterImpact();
        baseAttackSpeedMultiplier = 1.0f + PlayerAspects.Instance.GetShockLevel() * PlayerAspects.Instance.GetShockImpact();

        int oldMaxHealth = actualMaxHealth;

        actualDamageMultiplier = baseDamageMultiplier;
        actualMaxHealth = baseMaxHealth;
        actualMoveSpeed = baseMoveSpeed;
        actualAttackSpeedMultiplier = baseAttackSpeedMultiplier;

        foreach (Implant implant in PlayerInventory.Instance.GetEquippedImplants())
        {
            Debug.Log($"Применяем имплант {implant.Name} -> Усиленный: {implant.IsEnhanced}");

            actualDamageMultiplier *= (1 + implant.GetDamageBonus());
            actualMaxHealth = Mathf.RoundToInt(actualMaxHealth * (1 + implant.GetHealthBonus()));
            actualMoveSpeed *= (1 + implant.GetSpeedBonus());
            actualAttackSpeedMultiplier *= (1 + implant.GetAttackSpeedBonus());

            if (implant.IsEnhanced)
            {
                implant.ApplyEnhancedEffect(this);
            }
        }

        int newMaxHealth = actualMaxHealth;
        int diff = newMaxHealth - oldMaxHealth;
        if (diff > 0)
        {
            currentHealth += diff;
        }
        else if (diff < 0)
        {
            currentHealth = Mathf.Max(1, currentHealth + diff);
        }

        UpdateHealthBar();
        Debug.Log($"Финальные характеристики: Урон {actualDamageMultiplier}, Здоровье {actualMaxHealth}, Скорость {actualMoveSpeed}, Скорость атаки {actualAttackSpeedMultiplier}");
    }
}
