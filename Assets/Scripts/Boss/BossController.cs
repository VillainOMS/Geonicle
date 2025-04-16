using UnityEngine;
using System;
using DG.Tweening;

public class BossController : Enemy
{
    [Header("Босс-логика")]
    public bool isInvulnerable = false;

    [Header("Щит")]
    [SerializeField] private GameObject shieldVisual;

    private BossHealthUI bossHealthUI;

    public event Action<float> OnBossDamaged;
    public event Action OnBossDeath;

    protected override void Awake()
    {
        base.Awake();
        Debug.Log("BossController: Awake вызван.");

        SetShieldActive(false);

        bossHealthUI = FindObjectOfType<BossHealthUI>();
        if (bossHealthUI != null)
        {
            Debug.Log("BossHealthUI найден и будет активирован.");
            bossHealthUI.SetHealthPercent(1f);
            bossHealthUI.SetShielded(false);
            bossHealthUI.Show(); // Важно: явно вызываем отображение!
        }
        else
        {
            Debug.LogWarning("BossController: BossHealthUI не найден в сцене!");
        }
    }

    public override void TakeDamage(int damage)
    {
        if (isInvulnerable || IsDead) return;

        base.TakeDamage(damage);

        float percent = (float)currentHealth / maxHealth;
        OnBossDamaged?.Invoke(percent);
        bossHealthUI?.SetHealthPercent(percent);

        if (IsDead)
        {
            OnBossDeath?.Invoke();
        }
    }

    public void SetInvulnerable(bool value)
    {
        isInvulnerable = value;
        SetShieldActive(value);
        bossHealthUI?.SetShielded(value);
    }

    private void SetShieldActive(bool isActive)
    {
        if (shieldVisual != null)
            shieldVisual.SetActive(isActive);
    }

    protected override void Die()
    {
        OnBossDeath?.Invoke();
        bossHealthUI?.Hide();

        float shakeDuration = 2.5f;
        float fallDuration = 0.80f;

        Vector3 originalPos = transform.position;
        Vector3 fallOffset = transform.forward * 2.0f + Vector3.down * 12.0f;
        Vector3 finalPos = originalPos + fallOffset;

        Sequence deathSequence = DOTween.Sequence();
        deathSequence.Append(transform.DOShakePosition(shakeDuration, new Vector3(0.5f, 0.2f, 0.5f), 10, 90));
        deathSequence.Append(transform.DOMove(finalPos, fallDuration).SetEase(Ease.InOutCubic));
        deathSequence.Join(transform.DORotate(new Vector3(90f, transform.eulerAngles.y, 0f), fallDuration).SetEase(Ease.InOutCubic));
        deathSequence.AppendCallback(() => Destroy(gameObject));
    }
}
