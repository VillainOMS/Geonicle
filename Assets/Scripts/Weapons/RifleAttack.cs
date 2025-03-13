using UnityEngine;
using DG.Tweening;

public class RifleAttack : WeaponBase
{
    [SerializeField] private float range = 100f;
    [SerializeField] private Camera playerCamera;

    [SerializeField] private float rifleDamage = 8f;  // Можно менять урон винтовки
    [SerializeField] private float rifleAttackSpeed = 0.8f;  // Можно менять скорость винтовки

    protected override void Start()
    {
        base.Start();
        baseDamage = rifleDamage;
        baseAttackSpeed = rifleAttackSpeed;
    }

    public override void Attack()
    {
        if (Time.time < nextAttackTime) return;

        float attackCooldown = GetAttackCooldown();
        nextAttackTime = Time.time + attackCooldown;

        AudioManager.Instance.PlayShootSound();

        if (attackTween != null && attackTween.IsActive())
        {
            attackTween.Kill();
        }

        float attackDuration = GetAttackAnimationDuration();
        attackTween = transform.DOLocalRotate(new Vector3(-45, 0, 0), attackDuration)
            .SetLoops(2, LoopType.Yoyo)
            .SetEase(Ease.OutSine);

        if (Physics.Raycast(playerCamera.transform.position, playerCamera.transform.forward, out RaycastHit hit, range))
        {
            if (hit.collider.CompareTag("Enemy"))
            {
                Enemy enemy = hit.collider.GetComponent<Enemy>();
                if (enemy != null)
                {
                    enemy.TakeDamage((int)GetFinalDamage());
                }
            }
        }
    }
}
