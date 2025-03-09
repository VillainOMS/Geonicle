using UnityEngine;

public class WeaponStats : MonoBehaviour
{
    public float baseDamage = 10.0f;  // Базовый урон
    public float baseAttackSpeed = 1.0f;  // Скорость атаки (выстрелов/ударов в секунду)

    private PlayerStats playerStats;

    private void Start()
    {
        playerStats = FindObjectOfType<PlayerStats>();  // Получаем характеристики игрока
    }

    public float GetFinalDamage()
    {
        return baseDamage * playerStats.damageMultiplier;
    }

    public float GetFinalAttackSpeed()
    {
        return baseAttackSpeed * playerStats.attackSpeedMultiplier;
    }
}
