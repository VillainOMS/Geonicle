using UnityEngine;

public class PlayerAbilities : MonoBehaviour
{
    public bool canDoubleJump = false;
    public bool hasDoubleJumped = false;

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
