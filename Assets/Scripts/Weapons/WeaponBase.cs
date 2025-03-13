using DG.Tweening;
using UnityEngine;

public abstract class WeaponBase : MonoBehaviour
{
    protected float baseDamage = 10f; 
    protected float baseAttackSpeed = 1f; 

    protected PlayerStats playerStats;
    protected float nextAttackTime = 0f;
    protected Tween attackTween;

    protected virtual void Start()
    {
        playerStats = FindObjectOfType<PlayerStats>();
        if (playerStats == null)
        {
            Debug.LogError($"{gameObject.name}: PlayerStats не найден!");
        }
    }

    public float GetFinalDamage()
    {
        return baseDamage * (playerStats != null ? playerStats.actualDamageMultiplier : 1f);
    }

    public float GetFinalAttackSpeed()
    {
        return baseAttackSpeed * (playerStats != null ? playerStats.actualAttackSpeedMultiplier : 1f);
    }

    public float GetAttackCooldown()
    {
        return 1f / GetFinalAttackSpeed();
    }

    public float GetAttackAnimationDuration()
    {
        return GetAttackCooldown() * 0.5f;
    }

    public abstract void Attack();
}
