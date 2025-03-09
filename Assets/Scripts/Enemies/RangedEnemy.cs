using UnityEngine;
using UnityEngine.AI;

public class RangedEnemy : Enemy
{
    [SerializeField] private Transform shootPoint;
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private int damage = 10;
    [SerializeField] private float detectionRange = 15f;
    [SerializeField] private float fireRate = 2f;

    private Transform player;
    private PlayerStats playerStats;
    private NavMeshAgent agent;
    private float nextFireTime = 0f;

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
        playerStats = player.GetComponent<PlayerStats>();
    }

    private void Update()
    {
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        if (distanceToPlayer <= detectionRange)
        {
            if (HasLineOfSight())
            {
                if (Time.time >= nextFireTime)
                {
                    Attack();
                    nextFireTime = Time.time + fireRate;
                }
            }
            else
            {
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
        AudioManager.Instance.PlayEnemyShootSound();
        GameObject projectile = Instantiate(projectilePrefab, shootPoint.position, Quaternion.identity);
        Projectile projectileComponent = projectile.GetComponent<Projectile>();
        if (projectileComponent != null)
        {
            projectileComponent.Initialize(damage, player.position, playerStats);
        }
    }

    private bool HasLineOfSight()
    {
        RaycastHit hit;
        Vector3 directionToPlayer = player.position - transform.position;
        if (Physics.Raycast(transform.position, directionToPlayer.normalized, out hit, detectionRange))
        {
            return hit.collider.CompareTag("Player");
        }
        return false;
    }
}
