using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerStats : MonoBehaviour
{
    public static PlayerStats Instance { get; private set; }

    [Header("������� ��������������")]
    public float baseDamageMultiplier = 1.0f;
    public int baseMaxHealth = 100;
    public float baseMoveSpeed = 3f;
    public float baseAttackSpeedMultiplier = 1.0f;

    [Header("����������� �������������� (��������� �������� � �������)")]
    public float actualDamageMultiplier;
    public int actualMaxHealth;
    public float actualMoveSpeed;
    public float actualAttackSpeedMultiplier;

    [Header("���������� ���������� ��������")]
    [SerializeField] private int currentHealth;
    [SerializeField] private float currentHealthPercentage;

    [SerializeField] private Slider healthBar;
    [SerializeField] private GameObject gameOverPanel;

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
        currentHealth -= damage;
        currentHealth = Mathf.Clamp(currentHealth, 0, actualMaxHealth);
        UpdateHealthBar();
        AudioManager.Instance.PlayPlayerHurtSound();

        if (currentHealth <= 0)
            Die();
    }

    private void Die()
    {
        if (gameOverPanel != null)
            gameOverPanel.SetActive(true);

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        Time.timeScale = 0f;
    }

    public void UpdateHealthBar()
    {
        if (healthBar != null)
            healthBar.value = (float)currentHealth / actualMaxHealth;
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
