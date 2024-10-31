using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField] private int maxHealth = 100;
    [SerializeField] private Slider healthBar; // Полоса здоровья в интерфейсе
    [SerializeField] private GameObject gameOverPanel; // Панель экрана смерти

    private int currentHealth;

    private void Start()
    {
        currentHealth = maxHealth;
        UpdateHealthBar();
        gameOverPanel.SetActive(false); // Панель смерти скрыта при запуске
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
        UpdateHealthBar();

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
        Cursor.lockState = CursorLockMode.None;  // Разблокируем курсор
        Cursor.visible = true;  // Показываем курсор
        gameOverPanel.SetActive(true);
        Time.timeScale = 0f; // Останавливаем время
    }

    // Методы для кнопок будут назначены в инспекторе, но можно оставить их здесь как базу
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
