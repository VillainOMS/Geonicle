using UnityEngine;

public class PlayerAbilities : MonoBehaviour
{
    public bool canDoubleJump = false;
    public bool hasDoubleJumped = false;

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
