using UnityEngine;

public class Projectile : MonoBehaviour
{
    private int damage; // Урон, который будет наноситься
    private Vector3 target; // Цель, на которую будет направлен снаряд
    [SerializeField] private float speed = 20f; // Скорость снаряда

    // Метод для установки урона и цели снаряда
    public void Initialize(int damageAmount, Vector3 targetPosition)
    {
        damage = damageAmount;
        target = targetPosition;
        Destroy(gameObject, 8f); // Уничтожаем снаряд через 3 секунды, если он не попал в игрока
    }

    private void Update()
    {
        // Перемещаем снаряд в сторону цели
        Vector3 direction = (target - transform.position).normalized;
        transform.position += direction * speed * Time.deltaTime;

        // Если снаряд близок к цели (игроку), уничтожаем его
        if (Vector3.Distance(transform.position, target) < 0.5f)
        {
            Destroy(gameObject); // Уничтожаем снаряд, если он достиг цели
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        // Проверка на столкновение с игроком
        if (other.CompareTag("Player"))
        {
            // Здесь добавьте код для нанесения урона игроку
            // Например:
            // PlayerHealth playerHealth = other.GetComponent<PlayerHealth>();
            // if (playerHealth != null) 
            // {
            //     playerHealth.TakeDamage(damage);
            // }

            Debug.Log($"Player hit! Damage: {damage}"); // Псевдо-код для тестирования
        }

        // Уничтожаем снаряд при столкновении с любым объектом
        Destroy(gameObject);
    }
}
