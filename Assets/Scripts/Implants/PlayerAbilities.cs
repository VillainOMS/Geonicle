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

    private void PerformDoubleJump()
    {
        Debug.Log("����� ������ ������� ������!");
        // ����� ����� �������� ��������� ������ ������
    }
}
