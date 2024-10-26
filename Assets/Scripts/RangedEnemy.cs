using UnityEngine;
using UnityEngine.AI;

public class RangedEnemy : Enemy
{
    [SerializeField] private Transform shootPoint; // Точка для выстрела
    [SerializeField] private GameObject projectilePrefab; // Префаб снаряда
    [SerializeField] private int damage = 10; // Урон, который будет наноситься снарядом
    [SerializeField] private float detectionRange = 15f; // Дальность обнаружения игрока
    [SerializeField] private float fireRate = 2f; // Скорострельность

    private Transform player;
    private NavMeshAgent agent;
    private float nextFireTime = 0f;

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        player = GameObject.FindGameObjectWithTag("Player").transform; // Найти игрока по тегу
    }

    private void Update()
    {
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        // Проверяем, находится ли игрок в пределах обнаружения
        if (distanceToPlayer <= detectionRange)
        {
            // Проверяем прямой обзор на игрока
            if (HasLineOfSight())
            {
                // Если враг видит игрока и время для стрельбы
                if (Time.time >= nextFireTime)
                {
                    Attack();
                    nextFireTime = Time.time + fireRate; // Устанавливаем время следующей стрельбы
                }
            }
            else
            {
                // Двигаемся к игроку, если обзор заблокирован
                MoveToPlayer();
            }
        }
    }

    private void MoveToPlayer()
    {
        agent.SetDestination(player.position);
    }

    private void Attack()
    {
        // Создаем снаряд и направляем его на игрока
        GameObject projectile = Instantiate(projectilePrefab, shootPoint.position, Quaternion.identity);
        Projectile projectileComponent = projectile.GetComponent<Projectile>();
        if (projectileComponent != null)
        {
            projectileComponent.Initialize(damage, player.position); // Устанавливаем урон и цель для снаряда
        }
    }

    private bool HasLineOfSight()
    {
        // Проверяем, есть ли преграда между врагом и игроком
        RaycastHit hit;
        Vector3 directionToPlayer = player.position - transform.position;
        if (Physics.Raycast(transform.position, directionToPlayer.normalized, out hit, detectionRange))
        {
            return hit.collider.CompareTag("Player");
        }
        return false;
    }
}
