using UnityEngine;

public class Implant
{
    public int ID { get; private set; }              // Добавлено сохранение ID
    public string Name { get; private set; }
    public string Description { get; private set; }
    public string Slot { get; private set; }
    public bool IsEnhanced { get; private set; }
    public Sprite Icon { get; private set; }

    private System.Action<PlayerStats> applyEffect;
    private System.Action<PlayerStats> removeEffect;
    private System.Action<PlayerStats> applyEnhancedEffect;
    private System.Action<PlayerStats> removeEnhancedEffect;
    private System.Action<PlayerAbilities> applySpecialEffect;
    private System.Action<PlayerAbilities> removeSpecialEffect;

    public Implant(int id, string name, string description, string slot, Sprite icon,
                   System.Action<PlayerStats> applyEffect,
                   System.Action<PlayerStats> removeEffect,
                   System.Action<PlayerStats> applyEnhancedEffect = null,
                   System.Action<PlayerStats> removeEnhancedEffect = null,
                   System.Action<PlayerAbilities> specialApply = null,
                   System.Action<PlayerAbilities> specialRemove = null)
    {
        ID = id; // теперь ID корректно назначается!
        Name = name;
        Description = description;
        Slot = slot;
        Icon = icon;
        this.applyEffect = applyEffect;
        this.removeEffect = removeEffect;
        this.applyEnhancedEffect = applyEnhancedEffect;
        this.removeEnhancedEffect = removeEnhancedEffect;
        applySpecialEffect = specialApply;
        removeSpecialEffect = specialRemove;
    }

    public void ApplyEffect(PlayerStats playerStats, PlayerAbilities abilities)
    {
        applyEffect?.Invoke(playerStats);
        if (IsEnhanced)
        {
            applyEnhancedEffect?.Invoke(playerStats);
        }
        applySpecialEffect?.Invoke(abilities);
    }

    public void RemoveEffect(PlayerStats playerStats, PlayerAbilities abilities)
    {
        removeEffect?.Invoke(playerStats);
        if (IsEnhanced)
        {
            removeEnhancedEffect?.Invoke(playerStats);
        }
        removeSpecialEffect?.Invoke(abilities);
    }

    public void SetEnhanced(bool enhanced)
    {
        IsEnhanced = enhanced;
    }
}
