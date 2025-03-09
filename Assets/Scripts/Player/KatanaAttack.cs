using DG.Tweening;
using UnityEngine;

public class KatanaAttack : WeaponBase
{
    [SerializeField] private float attackRange = 8f; // Дальность атаки (высота конуса)
    [SerializeField] private float attackAngle = 90f; // Угол атаки (ширина конуса)
    [SerializeField] private Transform attackOrigin; // Точка исхода удара (чуть впереди игрока)

    private void Start()
    {
        if (attackOrigin == null)
        {
            attackOrigin = transform; // Если не задано, используем позицию игрока
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

        // Центр атаки – немного впереди игрока
        Vector3 attackCenter = attackOrigin.position + attackOrigin.forward * (attackRange * 0.2f);

        // Поиск всех врагов в радиусе атаки
        Collider[] hitColliders = Physics.OverlapSphere(attackCenter, attackRange);
        foreach (var hitCollider in hitColliders)
        {
            // Вычисляем направление к цели
            Vector3 directionToTarget = (hitCollider.transform.position - attackOrigin.position).normalized;
            float angleToTarget = Vector3.Angle(transform.forward, directionToTarget);

            // Проверяем, находится ли враг в зоне удара
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

    // Визуализация зоны удара
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
