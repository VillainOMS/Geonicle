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

    private void UpdateHealthBar()
    {
        if (healthBar != null)
        {
            healthBar.value = (float)currentHealth / actualMaxHealth;
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

    public void RestartGame()
    {
        gameOverPanel.SetActive(false);
        Time.timeScale = 1f;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        SceneManager.LoadScene("GameScene");
    }

    public void LoadMainMenu()
    {
        gameOverPanel.SetActive(false);
        Time.timeScale = 1f;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        SceneManager.LoadScene("MainMenu");
    }

    // Аспекты изменяют ТОЛЬКО базовые характеристики
    public void ApplyAspectBonuses(int fireLevel, int metalLevel, int waterLevel, int shockLevel)
    {
        int oldMaxHealth = baseMaxHealth; // Сохраняем старое максимальное здоровье

        baseDamageMultiplier = 1.0f + fireLevel * PlayerAspects.Instance.GetFireImpact();
        baseMaxHealth = 100 + Mathf.RoundToInt(metalLevel * PlayerAspects.Instance.GetMetalImpact() * 100);
        baseMoveSpeed = 3.0f + waterLevel * PlayerAspects.Instance.GetWaterImpact();
        baseAttackSpeedMultiplier = 1.0f + shockLevel * PlayerAspects.Instance.GetShockImpact();

        // Разница в макс. здоровье
        int healthIncrease = baseMaxHealth - oldMaxHealth;

        if (healthIncrease > 0)
        {
            currentHealth += healthIncrease; // Увеличиваем текущее здоровье на разницу
            currentHealth = Mathf.Clamp(currentHealth, 0, baseMaxHealth); // Ограничиваем в пределах нового макс. здоровья
        }

        RecalculateActualStats(); // После изменений пересчитываем фактические характеристики
        UpdateHealthBar(); // Обновляем UI хелсбара
    }

    public void ApplyImplantEffect(Implant implant)
    {
        if (implant == null) return;

        implant.ApplyEffect(this, GetComponent<PlayerAbilities>());
        Debug.Log($"Применён имплант: {implant.Name}");
        RecalculateActualStats();
    }

    public void RemoveImplantEffect(Implant implant)
    {
        if (implant == null) return;

        implant.RemoveEffect(this, GetComponent<PlayerAbilities>());
        Debug.Log($"Удалён имплант: {implant.Name}");
        RecalculateActualStats();
    }

    // Фактические характеристики пересчитываются с учётом имплантов
    public void RecalculateActualStats(float damageBonus = 0f, float healthBonus = 0f, float speedBonus = 0f, float attackSpeedBonus = 0f)
    {
        actualDamageMultiplier = baseDamageMultiplier + damageBonus;
        actualMaxHealth = baseMaxHealth + Mathf.RoundToInt(healthBonus);
        actualMoveSpeed = baseMoveSpeed + speedBonus;
        actualAttackSpeedMultiplier = baseAttackSpeedMultiplier + attackSpeedBonus;

        Debug.Log($"Фактические характеристики пересчитаны: Урон {actualDamageMultiplier}, Здоровье {actualMaxHealth}, Скорость {actualMoveSpeed}, Скорость атаки {actualAttackSpeedMultiplier}");
    }
}
