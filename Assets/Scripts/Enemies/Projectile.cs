using UnityEngine;

public class Projectile : MonoBehaviour
{
    private int damage; // Урон, который будет наноситься
    private PlayerStats playerStats; // Система здоровья игрока
    private Vector3 direction; // Направление полета
    [SerializeField] private float speed = 20f; // Скорость снаряда
    [SerializeField] private float lifetime = 8f; // Время жизни снаряда

    // Метод для установки урона и цели снаряда
    public void Initialize(int damageAmount, Vector3 targetPosition, PlayerStats targetStats)
    {
        damage = damageAmount;
        playerStats = targetStats;
        direction = (targetPosition - transform.position).normalized;

        Destroy(gameObject, lifetime); // Уничтожаем снаряд через lifetime секунд, если он не попал в игрока
    }

    private void Update()
    {
        // Перемещаем снаряд в сторону цели
        transform.position += direction * speed * Time.deltaTime;
    }

    private void OnTriggerEnter(Collider other)
    {
        // Проверка на столкновение с игроком
        if (other.CompareTag("Player") && playerStats != null)
        {
            playerStats.TakeDamage(damage);
            Destroy(gameObject);
        }

        // Проверяем, не является ли объект союзным (например, враг)
        if (!other.CompareTag("Enemy") && !other.CompareTag("Projectile"))
        {
            Destroy(gameObject);
        }
    }
}
