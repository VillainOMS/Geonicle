using DG.Tweening;
using UnityEngine;

public abstract class WeaponBase : MonoBehaviour
{
    public float baseDamage = 10f;
    public float baseAttackSpeed = 1f;
    protected PlayerStats playerStats;
    protected float nextAttackTime = 0f;
    protected Tween attackTween;

    public void Initialize(PlayerStats stats)
    {
        playerStats = stats;
        if (playerStats == null)
        {
            Debug.LogError($"{gameObject.name}: PlayerStats �� ������!");
        }
    }

    public float GetFinalDamage()
    {
        return baseDamage * (playerStats != null ? playerStats.damageMultiplier : 1f);
    }

    public float GetFinalAttackSpeed()
    {
        return baseAttackSpeed * playerStats.attackSpeedMultiplier;
    }

    public float GetAttackCooldown()
    {
        return 1f / GetFinalAttackSpeed();
    }

    // ������ �������� ���� ������, ����� �� ����������� ��������� �����
    public float GetAttackAnimationDuration()
    {
        return GetAttackCooldown() * 0.5f; // �.�. Yoyo ��������� �������� ������, ����� �������
    }

    public abstract void Attack();
}
