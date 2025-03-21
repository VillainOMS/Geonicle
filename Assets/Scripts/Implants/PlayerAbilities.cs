using UnityEngine;

public class PlayerAbilities : MonoBehaviour
{
    public bool canDoubleJump = false;
    public bool hasDoubleJumped = false;

    private bool hasDashDamage = false;
    private float dashDamageAmount = 10f;

    public void EnableDoubleJump()
    {
        canDoubleJump = true;
        Debug.Log("Двойной прыжок активирован!");
    }

    public void DisableDoubleJump()
    {
        canDoubleJump = false;
        Debug.Log("Двойной прыжок отключён.");
    }

    public void EnableDashDamage(float damage)
    {
        hasDashDamage = true;
        dashDamageAmount = damage;
        Debug.Log($"Урон от рывка активирован! Урон: {damage}");
    }

    public void DisableDashDamage()
    {
        hasDashDamage = false;
        Debug.Log("Урон от рывка отключён.");
    }

    public void ResetDoubleJump()
    {
        hasDoubleJumped = false;
    }

    public bool TryUseDoubleJump()
    {
        if (canDoubleJump && !hasDoubleJumped)
        {
            hasDoubleJumped = true;
            Debug.Log("Игрок сделал двойной прыжок!");
            return true;
        }
        return false;
    }
}
