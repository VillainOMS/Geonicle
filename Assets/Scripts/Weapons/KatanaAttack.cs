using DG.Tweening;
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

        Collider[] hitColliders = Physics.OverlapSphere(attackCenter, attackRange);
        foreach (var hitCollider in hitColliders)
        {
            Vector3 directionToTarget = (hitCollider.transform.position - attackOrigin.position).normalized;
            float angleToTarget = Vector3.Angle(transform.forward, directionToTarget);

            if (angleToTarget <= attackAngle / 2 && hitCollider.CompareTag("Enemy"))
            {
                Enemy enemy = hitCollider.GetComponent<Enemy>();
                if (enemy != null)
                {
                    enemy.TakeDamage((int)GetFinalDamage());
                }
            }
        }
    }
}
