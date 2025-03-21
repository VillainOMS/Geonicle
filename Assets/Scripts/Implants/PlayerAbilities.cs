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
        Debug.Log("������� ������ �����������!");
    }

    public void DisableDoubleJump()
    {
        canDoubleJump = false;
        Debug.Log("������� ������ ��������.");
    }

    public void EnableDashDamage(float damage)
    {
        hasDashDamage = true;
        dashDamageAmount = damage;
        Debug.Log($"���� �� ����� �����������! ����: {damage}");
    }

    public void DisableDashDamage()
    {
        hasDashDamage = false;
        Debug.Log("���� �� ����� ��������.");
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
            Debug.Log("����� ������ ������� ������!");
            return true;
        }
        return false;
    }
}
