using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerStats : MonoBehaviour
{
    public float damageMultiplier = 1.0f;  // Бонус к урону
    public int maxHealth = 100;  // Максимальное здоровье
    public float moveSpeed = 3.0f;  // Скорость передвижения
    public float attackSpeedMultiplier = 1.0f;  // Бонус к скорости атаки

    [SerializeField] private Slider healthBar; // Полоса здоровья
    [SerializeField] private GameObject gameOverPanel; // Экран смерти

    private int currentHealth;

    private void Start()
    {
        currentHealth = maxHealth;
        UpdateHealthBar();
        if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(false);
        }
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
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
            healthBar.value = (float)currentHealth / maxHealth;
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
}
