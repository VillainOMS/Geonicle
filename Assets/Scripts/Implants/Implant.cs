using UnityEngine;
using System.Collections.Generic;

public class Implant
{
    public int ID { get; private set; }
    public string Name { get; private set; }
    public string Description { get; private set; }
    public string Slot { get; private set; }
    public bool IsEnhanced { get; private set; }
    public Sprite Icon { get; private set; }

    // Требуемые уровни аспектов для усиленного эффекта
    public int requiredFire { get; private set; }
    public int requiredWater { get; private set; }
    public int requiredMetal { get; private set; }
    public int requiredShock { get; private set; }

    // Процентные бонусы
    private float damageBonus;
    private float healthBonus;
    private float speedBonus;
    private float attackSpeedBonus;

    private System.Action<PlayerStats> applySpecialEffect;
    private System.Action<PlayerStats> removeSpecialEffect;
    private System.Action<PlayerAbilities> applyAbilitiesEffect;
    private System.Action<PlayerAbilities> removeAbilitiesEffect;

    // Добавляем улучшенные эффекты
    private System.Action<PlayerStats> applyEnhancedEffect;
    private System.Action<PlayerStats> removeEnhancedEffect;

    public Implant(int id, string name, string description, string slot, Sprite icon,
                   int requiredFire, int requiredWater, int requiredMetal, int requiredShock,
                   float damageBonus, float healthBonus, float speedBonus, float attackSpeedBonus,
                   System.Action<PlayerStats> applySpecialEffect,
                   System.Action<PlayerStats> removeSpecialEffect,
                   System.Action<PlayerStats> applyEnhancedEffect, // Добавлено
                   System.Action<PlayerStats> removeEnhancedEffect, // Добавлено
                   System.Action<PlayerAbilities> applyAbilitiesEffect = null,
                   System.Action<PlayerAbilities> removeAbilitiesEffect = null)
    {
        ID = id;
        Name = name;
        Description = description;
        Slot = slot;
        Icon = icon;

        this.requiredFire = requiredFire;
        this.requiredWater = requiredWater;
        this.requiredMetal = requiredMetal;
        this.requiredShock = requiredShock;

        this.damageBonus = damageBonus;
        this.healthBonus = healthBonus;
        this.speedBonus = speedBonus;
        this.attackSpeedBonus = attackSpeedBonus;

        this.applySpecialEffect = applySpecialEffect;
        this.removeSpecialEffect = removeSpecialEffect;
        this.applyAbilitiesEffect = applyAbilitiesEffect;
        this.removeAbilitiesEffect = removeAbilitiesEffect;

        this.applyEnhancedEffect = applyEnhancedEffect;
        this.removeEnhancedEffect = removeEnhancedEffect;
    }

    public void ApplyEffect(PlayerStats playerStats, PlayerAbilities abilities)
    {
        Debug.Log($"[ApplyEffect] {Name}: Применяем бонусы. Усиленный: {IsEnhanced}");

        playerStats.ApplyPercentageBonus(damageBonus, healthBonus, speedBonus, attackSpeedBonus);
        applySpecialEffect?.Invoke(playerStats);

        if (IsEnhanced)
        {
            applyEnhancedEffect?.Invoke(playerStats);
        }

        if (abilities != null)
        {
            applyAbilitiesEffect?.Invoke(abilities);
        }
    }

    public void RemoveEffect(PlayerStats playerStats, PlayerAbilities abilities)
    {
        Debug.Log($"[RemoveEffect] {Name}: IsEnhanced = {IsEnhanced}");

        playerStats.RemovePercentageBonus(damageBonus, healthBonus, speedBonus, attackSpeedBonus);
        removeSpecialEffect?.Invoke(playerStats);

        if (IsEnhanced)
        {
            Debug.Log($"[RemoveEffect] {Name}: Вызов RemoveEnhancedEffect");
            removeEnhancedEffect?.Invoke(playerStats);
        }

        if (abilities != null)
        {
            removeAbilitiesEffect?.Invoke(abilities);
        }
    }

    public void ApplyEnhancedEffect(PlayerStats playerStats)
    {
        applyEnhancedEffect?.Invoke(playerStats);

        PlayerAbilities abilities = playerStats.GetComponent<PlayerAbilities>();
        applyAbilitiesEffect?.Invoke(abilities);

        Debug.Log($"[ApplyEnhancedEffect] {Name}: Улучшенный эффект применён.");
    }

    public void RemoveEnhancedEffect(PlayerStats playerStats)
    {
        removeEnhancedEffect?.Invoke(playerStats);

        PlayerAbilities abilities = playerStats.GetComponent<PlayerAbilities>();

        if (removeAbilitiesEffect != null)
        {
            Debug.Log("[RemoveEnhancedEffect] Нашли removeAbilitiesEffect");
            removeAbilitiesEffect.Invoke(abilities);
        }
        else
        {
            Debug.Log("[RemoveEnhancedEffect] removeAbilitiesEffect = NULL");
        }

        Debug.Log($"[RemoveEnhancedEffect] {Name}: Улучшенный эффект отключён.");
    }

    public void SetEnhanced(bool enhanced)
    {
        IsEnhanced = enhanced;
    }

    public bool CheckIfEnhanced()
    {
        return PlayerAspects.Instance.GetFireLevel() >= requiredFire &&
               PlayerAspects.Instance.GetWaterLevel() >= requiredWater &&
               PlayerAspects.Instance.GetMetalLevel() >= requiredMetal &&
               PlayerAspects.Instance.GetShockLevel() >= requiredShock;
    }

    public float GetDamageBonus() => damageBonus;
    public float GetHealthBonus() => healthBonus;
    public float GetSpeedBonus() => speedBonus;
    public float GetAttackSpeedBonus() => attackSpeedBonus;
}
