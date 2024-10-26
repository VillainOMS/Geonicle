using UnityEngine;
using UnityEngine.AI;

public class MeleeEnemy : Enemy
{
    [SerializeField] private float attackRange = 2f;
    [SerializeField] private float attackDamage = 10f;
    [SerializeField] private float attackCooldown = 1f;
    private float lastAttackTime;

    private Transform player;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform; // ����� ������ �� ����
    }

    private void Update()
    {
        // ���� ����� ����������, ���� ���
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
        // �������� � ������
        NavMeshAgent agent = GetComponent<NavMeshAgent>();
        agent.SetDestination(player.position);
    }

    private void Attack()
    {
        lastAttackTime = Time.time;
        Debug.Log("Melee Enemy Attacked Player for " + attackDamage + " damage!");
        // ����� ����� ������� �����, ����� ������� ���� ������
    }
}
