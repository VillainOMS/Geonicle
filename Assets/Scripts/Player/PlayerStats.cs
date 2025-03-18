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
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(true);
        }
        AudioManager.Instance.PlayDeathScreenMusic();
        Time.timeScale = 0f;
    }

    public void ApplyPercentageBonus(float damageBonus, float healthBonus, float speedBonus, float attackSpeedBonus)
    {
        actualDamageMultiplier *= (1 + damageBonus);
        actualMaxHealth = Mathf.RoundToInt(actualMaxHealth * (1 + healthBonus));
        actualMoveSpeed *= (1 + speedBonus);
        actualAttackSpeedMultiplier *= (1 + attackSpeedBonus);
    }

    public void RemovePercentageBonus(float damageBonus, float healthBonus, float speedBonus, float attackSpeedBonus)
    {
        actualDamageMultiplier /= (1 + damageBonus);
        actualMaxHealth = Mathf.RoundToInt(actualMaxHealth / (1 + healthBonus));
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
        int oldMaxHealth = baseMaxHealth;

        baseDamageMultiplier = 1.0f + fireLevel * PlayerAspects.Instance.GetFireImpact();
        baseMaxHealth = 100 + Mathf.RoundToInt(metalLevel * PlayerAspects.Instance.GetMetalImpact() * 100);
        baseMoveSpeed = 3.0f + waterLevel * PlayerAspects.Instance.GetWaterImpact();
        baseAttackSpeedMultiplier = 1.0f + shockLevel * PlayerAspects.Instance.GetShockImpact();

        int healthIncrease = baseMaxHealth - oldMaxHealth;

        if (healthIncrease > 0)
        {
            currentHealth += healthIncrease;
            currentHealth = Mathf.Clamp(currentHealth, 0, baseMaxHealth);
        }

        RecalculateActualStats();
        UpdateHealthBar();
    }

    public void RecalculateActualStats()
    {
        Debug.Log("=== RecalculateActualStats вызван ===");
        Debug.Log("=== Пересчёт характеристик ===");
        Debug.Log($"Базовые характеристики ДО аспектов: Урон {baseDamageMultiplier}, Здоровье {baseMaxHealth}, Скорость {baseMoveSpeed}");

        // 1. Применяем бонусы аспектов
        baseDamageMultiplier = 1.0f + PlayerAspects.Instance.GetFireLevel() * PlayerAspects.Instance.GetFireImpact();
        baseMaxHealth = 100 + Mathf.RoundToInt(PlayerAspects.Instance.GetMetalLevel() * PlayerAspects.Instance.GetMetalImpact() * 100);
        baseMoveSpeed = 3.0f + PlayerAspects.Instance.GetWaterLevel() * PlayerAspects.Instance.GetWaterImpact();
        baseAttackSpeedMultiplier = 1.0f + PlayerAspects.Instance.GetShockLevel() * PlayerAspects.Instance.GetShockImpact();

        Debug.Log($"После аспектов: Урон {baseDamageMultiplier}, Здоровье {baseMaxHealth}, Скорость {baseMoveSpeed}");

        // 2. Начинаем фактические характеристики с изменённых аспектами значений
        actualDamageMultiplier = baseDamageMultiplier;
        actualMaxHealth = baseMaxHealth;
        actualMoveSpeed = baseMoveSpeed;
        actualAttackSpeedMultiplier = baseAttackSpeedMultiplier;

        // 3. Добавляем процентные бонусы от имплантов
        foreach (Implant implant in PlayerInventory.Instance.GetEquippedImplants())
        {
            Debug.Log($"Применяем имплант {implant.Name} -> Урон: +{implant.GetDamageBonus() * 100}%, Здоровье: +{implant.GetHealthBonus() * 100}%, Скорость: +{implant.GetSpeedBonus() * 100}%");

            actualDamageMultiplier *= (1 + implant.GetDamageBonus());
            actualMaxHealth = Mathf.RoundToInt(actualMaxHealth * (1 + implant.GetHealthBonus()));
            actualMoveSpeed *= (1 + implant.GetSpeedBonus());
            actualAttackSpeedMultiplier *= (1 + implant.GetAttackSpeedBonus());
        }

        Debug.Log($"Финальные характеристики: Урон {actualDamageMultiplier}, Здоровье {actualMaxHealth}, Скорость {actualMoveSpeed}, Скорость атаки {actualAttackSpeedMultiplier}");
    }

}
