using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerStats : MonoBehaviour
{
    public static PlayerStats Instance { get; private set; }

    // ������� ���������� (���������� ��� �������� ��������)
    [Header("������� ��������������")]
    [SerializeField] public float baseDamageMultiplier = 1.0f;
    [SerializeField] public int baseMaxHealth = 100;
    [SerializeField] public float baseMoveSpeed = 2.7f;
    [SerializeField] public float baseAttackSpeedMultiplier = 1.0f;

    // ����������� ���������� (��������� ��������)
    [Header("����������� �������������� (������������� ���������������)")]
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
        RecalculateActualStats(); // �������� ����������� ������������� ��� ������
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
        Debug.Log("����� �����!");
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

    // ������� �������� ������ ������� ��������������
    public void ApplyAspectBonuses(int fireLevel, int metalLevel, int waterLevel, int shockLevel)
    {
        int oldMaxHealth = baseMaxHealth; // ��������� ������ ������������ ��������

        baseDamageMultiplier = 1.0f + fireLevel * PlayerAspects.Instance.GetFireImpact();
        baseMaxHealth = 100 + Mathf.RoundToInt(metalLevel * PlayerAspects.Instance.GetMetalImpact() * 100);
        baseMoveSpeed = 3.0f + waterLevel * PlayerAspects.Instance.GetWaterImpact();
        baseAttackSpeedMultiplier = 1.0f + shockLevel * PlayerAspects.Instance.GetShockImpact();

        // ������� � ����. ��������
        int healthIncrease = baseMaxHealth - oldMaxHealth;

        if (healthIncrease > 0)
        {
            currentHealth += healthIncrease; // ����������� ������� �������� �� �������
            currentHealth = Mathf.Clamp(currentHealth, 0, baseMaxHealth); // ������������ � �������� ������ ����. ��������
        }

        RecalculateActualStats(); // ����� ��������� ������������� ����������� ��������������
        UpdateHealthBar(); // ��������� UI ��������
    }

    public void ApplyImplantEffect(Implant implant)
    {
        if (implant == null) return;

        implant.ApplyEffect(this, GetComponent<PlayerAbilities>());
        Debug.Log($"������� �������: {implant.Name}");
        RecalculateActualStats();
    }

    public void RemoveImplantEffect(Implant implant)
    {
        if (implant == null) return;

        implant.RemoveEffect(this, GetComponent<PlayerAbilities>());
        Debug.Log($"����� �������: {implant.Name}");
        RecalculateActualStats();
    }

    // ����������� �������������� ��������������� � ������ ���������
    public void RecalculateActualStats(float damageBonus = 0f, float healthBonus = 0f, float speedBonus = 0f, float attackSpeedBonus = 0f)
    {
        actualDamageMultiplier = baseDamageMultiplier + damageBonus;
        actualMaxHealth = baseMaxHealth + Mathf.RoundToInt(healthBonus);
        actualMoveSpeed = baseMoveSpeed + speedBonus;
        actualAttackSpeedMultiplier = baseAttackSpeedMultiplier + attackSpeedBonus;

        Debug.Log($"����������� �������������� �����������: ���� {actualDamageMultiplier}, �������� {actualMaxHealth}, �������� {actualMoveSpeed}, �������� ����� {actualAttackSpeedMultiplier}");
    }
}
