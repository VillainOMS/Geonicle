using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;

public class KatanaAttack : WeaponBase
{
    [SerializeField] private float attackRange = 8f;
    [SerializeField] private float attackAngle = 90f;
    [SerializeField] private Transform attackOrigin;

    [SerializeField] private float katanaDamage = 15f;
    [SerializeField] private float katanaAttackSpeed = 1.2f;

    protected override void Start()
    {
        base.Start();
        baseDamage = katanaDamage;
        baseAttackSpeed = katanaAttackSpeed;

        if (attackOrigin == null)
        {
            attackOrigin = transform;
        }
    }

    public override void Attack()
    {
        if (Time.time < nextAttackTime) return;

        float attackCooldown = GetAttackCooldown();
        nextAttackTime = Time.time + attackCooldown;

        AudioManager.Instance.PlayKatanaSound();

        if (attackTween != null && attackTween.IsActive())
        {
            attackTween.Kill();
        }

        float attackDuration = GetAttackAnimationDuration();
        attackTween = transform.DOLocalRotate(new Vector3(0, -120, -90), attackDuration)
            .SetLoops(2, LoopType.Yoyo)
            .SetEase(Ease.OutSine);

        Vector3 attackCenter = attackOrigin.position + attackOrigin.forward * (attackRange * 0.2f);

        Collider[] hitColliders = Physics.OverlapSphere(
            attackCenter,
            attackRange,
            ~0,
            QueryTriggerInteraction.Collide
        );

        HashSet<Enemy> damagedEnemies = new HashSet<Enemy>();

        foreach (var hitCollider in hitColliders)
        {
            // используем bounds.center вместо transform.position
            Vector3 directionToTarget = (hitCollider.bounds.center - attackOrigin.position).normalized;
            float angleToTarget = Vector3.Angle(attackOrigin.forward, directionToTarget);

            if (angleToTarget <= attackAngle / 2)
            {
                Enemy enemy = hitCollider.GetComponentInParent<Enemy>();
                if (enemy != null && !damagedEnemies.Contains(enemy))
                {
                    enemy.TakeDamage((int)GetFinalDamage());
                    damagedEnemies.Add(enemy);
                }
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        if (attackOrigin == null) return;

        Gizmos.color = new Color(1f, 0f, 0f, 0.5f);
        Vector3 forward = attackOrigin.forward * attackRange;
        Vector3 attackCenter = attackOrigin.position + attackOrigin.forward * (attackRange * 0.2f);

        Gizmos.DrawWireSphere(attackCenter, attackRange);

        for (float angle = -attackAngle / 2; angle <= attackAngle / 2; angle += 5f)
        {
            Vector3 direction = Quaternion.Euler(0, angle, 0) * forward;
            Gizmos.DrawRay(attackCenter, direction);
        }
    }
}
