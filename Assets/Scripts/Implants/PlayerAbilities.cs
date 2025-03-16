using UnityEngine;

public class PlayerAbilities : MonoBehaviour
{
    private bool canDoubleJump = false;
    private bool hasDashDamage = false;
    private float dashDamageAmount = 10f;

    private void Update()
    {
        if (canDoubleJump && Input.GetButtonDown("Jump"))
        {
            PerformDoubleJump();
        }
    }

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

    private void PerformDoubleJump()
    {
        Debug.Log("Игрок сделал двойной прыжок!");
        // Здесь можно добавить изменение физики прыжка
    }
}
