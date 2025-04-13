using UnityEngine;

public class Projectile : MonoBehaviour
{
    private int damage; // Урон, который будет наноситься
    private PlayerStats playerStats; // Система здоровья игрока
    private Vector3 direction; // Направление полета
    [SerializeField] private float speed = 20f; // Скорость снаряда
    [SerializeField] private float lifetime = 15f; // Время жизни снаряда


    public void Initialize(int damageAmount, Vector3 direction)
    {
        damage = damageAmount;
        this.direction = direction.normalized;
        Destroy(gameObject, lifetime);
    }

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
        if (other.CompareTag("Player"))
        {
            PlayerStats stats = other.GetComponent<PlayerStats>();
            if (stats != null)
            {
                stats.TakeDamage(damage);
            }

            Destroy(gameObject);
            return;
        }

        // Уничтожаемся, если попали во что-то, что НЕ "Enemy" и НЕ "Projectile"
        if (!other.CompareTag("Enemy") && !other.CompareTag("Projectile") && !other.isTrigger)
        {
            Destroy(gameObject);
        }
    }

}
