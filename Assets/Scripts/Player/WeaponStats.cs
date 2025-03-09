using UnityEngine;

public class WeaponStats : MonoBehaviour
{
    public float baseDamage = 10.0f;  // ������� ����
    public float baseAttackSpeed = 1.0f;  // �������� ����� (���������/������ � �������)

    private PlayerStats playerStats;

    private void Start()
    {
        playerStats = FindObjectOfType<PlayerStats>();  // �������� �������������� ������
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
