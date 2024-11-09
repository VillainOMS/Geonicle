using UnityEngine;

public class Projectile : MonoBehaviour
{
    private int damage; // Урон, который будет наноситься
    private PlayerHealth playerHealth;
    private Vector3 target; // Цель, на которую будет направлен снаряд
    private Vector3 direction;
    [SerializeField] private float speed = 20f; // Скорость снаряда

    // Метод для установки урона и цели снаряда
    public void Initialize(int damageAmount, Vector3 targetPosition, PlayerHealth playerHealthTarget)
    {
        damage = damageAmount;
        target = targetPosition;
        playerHealth = playerHealthTarget;
        direction = (target - transform.position).normalized;
        Destroy(gameObject, 8f); // Уничтожаем снаряд через 8 секунды, если он не попал в игрока
    }

    private void Update()
    {
        // Перемещаем снаряд в сторону цели
        transform.position += direction * speed * Time.deltaTime;
    }

    private void OnTriggerEnter(Collider other)
    {
        // Проверка на столкновение с игроком
        if (other.CompareTag("Player") && playerHealth != null)
        {
            playerHealth.TakeDamage(damage);
        }
        Debug.Log(other.gameObject);
        // Уничтожаем снаряд при столкновении с любым объектом
        Destroy(gameObject);
    }
}
