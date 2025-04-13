using UnityEngine;
using System.Collections;

public class BossAttackController : MonoBehaviour
{
    [Header("Общие параметры")]
    [SerializeField] private Transform projectileSpawnPoint;
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private float projectileAttackInterval = 3f;
    [SerializeField] private int projectileCount = 5;
    [SerializeField] private float spreadAngle = 30f;

    [Header("Колонны энергии")]
    [SerializeField] private GameObject energyStrikePrefab;
    [SerializeField] private float energyAttackInterval = 8f;
    [SerializeField] private int strikeCount = 3;
    [SerializeField] private float strikeRadius = 6f;

    [Header("Цель")]
    [SerializeField] private Transform player;

    private bool useProjectileNext = true;

    private void Start()
    {
        if (player == null)
        {
            player = GameObject.FindGameObjectWithTag("Player")?.transform;
        }

        StartCoroutine(AttackLoop());
    }

    private IEnumerator AttackLoop()
    {
        while (true)
        {
            if (useProjectileNext)
            {
                ShootProjectiles();
                yield return new WaitForSeconds(projectileAttackInterval);
            }
            else
            {
                SummonEnergyStrikes();
                yield return new WaitForSeconds(energyAttackInterval);
            }

            useProjectileNext = !useProjectileNext;
        }
    }

    private void ShootProjectiles()
    {
        if (projectilePrefab == null || projectileSpawnPoint == null || player == null)
            return;

        Vector3 directionToPlayer = (player.position - projectileSpawnPoint.position).normalized;

        for (int i = 0; i < projectileCount; i++)
        {
            float angleOffset = Random.Range(-spreadAngle / 2f, spreadAngle / 2f);
            Quaternion rotation = Quaternion.AngleAxis(angleOffset, Vector3.up) * Quaternion.LookRotation(directionToPlayer);

            GameObject proj = Instantiate(projectilePrefab, projectileSpawnPoint.position, rotation);
            Projectile p = proj.GetComponent<Projectile>();
            if (p != null)
            {
                Vector3 spreadDirection = rotation * Vector3.forward;
                p.Initialize(15, spreadDirection); // используем новый метод и направление
            }
        }

        AudioManager.Instance?.PlayEnemyShootSound();
    }

    private void SummonEnergyStrikes()
    {
        if (energyStrikePrefab == null || player == null)
            return;

        float minRadius = strikeRadius * 1f;
        float maxRadius = strikeRadius * 3f;

        for (int i = 0; i < strikeCount; i++)
        {
            float angle = Random.Range(0f, 360f);
            float radius = Random.Range(minRadius, maxRadius);

            Vector3 offset = new Vector3(Mathf.Cos(angle), 0, Mathf.Sin(angle)) * radius;
            Vector3 strikePos = player.position + offset;

            Instantiate(energyStrikePrefab, strikePos, Quaternion.identity);
        }
    }
}
