using UnityEngine;
using UnityEngine.AI;

public class MeleeEnemy : Enemy
{
    [SerializeField] private float attackRange = 2f;
    [SerializeField] private float attackCooldown = 1f;
    private float lastAttackTime;

    private Transform player;
    private PlayerStats playerStats;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        playerStats = player.GetComponent<PlayerStats>();
    }

    private void Update()
    {
        if (player != null)
        {
            float distance = Vector3.Distance(transform.position, player.position);

            if (distance <= attackRange && Time.time >= lastAttackTime + attackCooldown)
            {
                Attack();
            }
            else if (distance > attackRange)
            {
                MoveToPlayer();
            }
        }
    }

    private void MoveToPlayer()
    {
        NavMeshAgent agent = GetComponent<NavMeshAgent>();
        agent.SetDestination(player.position);
    }

    private void Attack()
    {
        lastAttackTime = Time.time;

        int damage = GetDamage();
        Debug.Log("Melee Enemy Attacked Player for " + damage + " damage!");

        if (playerStats != null)
        {
            playerStats.TakeDamage(damage);
        }
    }
}
