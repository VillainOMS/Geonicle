using UnityEngine;
using System;

public class BossController : Enemy
{
    [Header("Босс-логика")]
    public bool isInvulnerable = false;

    [Header("Щит")]
    [SerializeField] private GameObject shieldVisual;

    public event Action<float> OnBossDamaged;
    public event Action OnBossDeath;

    protected override void Awake()
    {
        base.Awake();
        SetShieldActive(false); // по умолчанию щит выключен
    }

    public override void TakeDamage(int damage)
    {
        if (isInvulnerable || IsDead) return;

        base.TakeDamage(damage);
        OnBossDamaged?.Invoke((float)currentHealth / maxHealth);

        if (IsDead)
        {
            OnBossDeath?.Invoke();
        }
    }

    public void SetInvulnerable(bool value)
    {
        isInvulnerable = value;
        SetShieldActive(value);
    }

    private void SetShieldActive(bool isActive)
    {
        if (shieldVisual != null)
            shieldVisual.SetActive(isActive);
    }
}
