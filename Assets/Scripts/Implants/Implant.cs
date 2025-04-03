using UnityEngine;
using System;
using System.Collections.Generic;

public class Implant
{
    public int ID { get; private set; }
    public string Name { get; private set; }
    public string Description { get; private set; }
    public string EnhancedDescription { get; private set; }
    public string Slot { get; private set; }
    public bool IsEnhanced { get; private set; }
    public Sprite Icon { get; private set; }

    public int requiredFire { get; private set; }
    public int requiredWater { get; private set; }
    public int requiredMetal { get; private set; }
    public int requiredShock { get; private set; }

    private float damageBonus;
    private float healthBonus;
    private float speedBonus;
    private float attackSpeedBonus;

    private float enhancedDamageBonus;
    private float enhancedHealthBonus;
    private float enhancedSpeedBonus;
    private float enhancedAttackSpeedBonus;

    private Action<PlayerAbilities> applyAbilitiesEffect;
    private Action<PlayerAbilities> removeAbilitiesEffect;

    public Implant(int id, string name, string description, string enhancedDescription, string slot, Sprite icon,
                   int requiredFire, int requiredWater, int requiredMetal, int requiredShock,
                   float damageBonus, float healthBonus, float speedBonus, float attackSpeedBonus,
                   float enhancedDamageBonus, float enhancedHealthBonus, float enhancedSpeedBonus, float enhancedAttackSpeedBonus,
                   Action<PlayerAbilities> applyAbilitiesEffect = null,
                   Action<PlayerAbilities> removeAbilitiesEffect = null)
    {
        ID = id;
        Name = name;
        Description = description;
        EnhancedDescription = enhancedDescription;
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

        this.enhancedDamageBonus = enhancedDamageBonus;
        this.enhancedHealthBonus = enhancedHealthBonus;
        this.enhancedSpeedBonus = enhancedSpeedBonus;
        this.enhancedAttackSpeedBonus = enhancedAttackSpeedBonus;

        this.applyAbilitiesEffect = applyAbilitiesEffect;
        this.removeAbilitiesEffect = removeAbilitiesEffect;
    }

    public void ApplyEnhancedEffect(PlayerStats playerStats)
    {
        PlayerAbilities abilities = playerStats.GetComponent<PlayerAbilities>();
        applyAbilitiesEffect?.Invoke(abilities);
    }

    public void RemoveEnhancedEffect(PlayerStats playerStats)
    {
        PlayerAbilities abilities = playerStats.GetComponent<PlayerAbilities>();
        removeAbilitiesEffect?.Invoke(abilities);
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

    public float GetEnhancedDamageBonus() => enhancedDamageBonus;
    public float GetEnhancedHealthBonus() => enhancedHealthBonus;
    public float GetEnhancedSpeedBonus() => enhancedSpeedBonus;
    public float GetEnhancedAttackSpeedBonus() => enhancedAttackSpeedBonus;

    public List<string> GetAspectSequence()
    {
        var result = new List<string>();
        for (int i = 0; i < requiredFire; i++) result.Add("Fire");
        for (int i = 0; i < requiredWater; i++) result.Add("Water");
        for (int i = 0; i < requiredMetal; i++) result.Add("Metal");
        for (int i = 0; i < requiredShock; i++) result.Add("Shock");
        return result;
    }
}
