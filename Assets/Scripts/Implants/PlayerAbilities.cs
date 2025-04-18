using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerAbilities : MonoBehaviour
{
    public bool canDoubleJump = false;
    public bool hasDoubleJumped = false;
    private bool isDashInvulnerabilityActive = false;
    private bool dashDamageEnabled = false;
    public bool IsInvulnerable => isDashInvulnerabilityActive;
    private bool jumpBoostEnabled = false;
    private bool trackingShotOnKill = false;
    private bool healOnKill = false;
    private bool ignoreNextDamage = false;

    public void TriggerIgnoreNextDamage()
    {
        ignoreNextDamage = true;
    }

    public bool TryBlockDamage()
    {
        if (ignoreNextDamage)
        {
            Debug.Log("Мозговая защита: первый урон в комнате заблокирован.");
            ignoreNextDamage = false;
            return true;
        }
        return false;
    }

    public void EnableTrackingShotOnKill()
    {
        trackingShotOnKill = true;
    }

    public void DisableTrackingShotOnKill()
    {
        trackingShotOnKill = false;
    }

    public void EnableHealOnKill()
    {
        healOnKill = true;
    }

    public void DisableHealOnKill()
    {
        healOnKill = false;
    }

    public void OnEnemyKilled()
    {
        if (trackingShotOnKill)
        {
            TryShootTrackingDamage();
        }

        if (healOnKill)
        {
            PlayerStats.Instance.Heal(4);
        }
    }

    private void TryShootTrackingDamage()
    {
        Enemy[] enemies = GameObject.FindObjectsOfType<Enemy>();
        if (enemies.Length <= 1) return; // Только что умерший враг тоже в списке

        List<Enemy> alive = new List<Enemy>();
        foreach (var e in enemies)
        {
            if (!e.IsDead)
                alive.Add(e);
        }

        if (alive.Count > 0)
        {
            int index = UnityEngine.Random.Range(0, alive.Count);
            alive[index].TakeDamage(10);
            Debug.Log("Глаза с наведением: нанесён урон другому врагу");
        }
    }

    public void EnableJumpBoost()
    {
        jumpBoostEnabled = true;
    }

    public void DisableJumpBoost()
    {
        jumpBoostEnabled = false;
    }

    public bool IsJumpBoostEnabled()
    {
        return jumpBoostEnabled;
    }


    public void EnableDashInvulnerability(float duration)
    {
        StartCoroutine(TemporaryInvulnerability(duration));
    }

    private IEnumerator TemporaryInvulnerability(float duration)
    {
        isDashInvulnerabilityActive = true;
        yield return new WaitForSeconds(duration);
        isDashInvulnerabilityActive = false;
    }

    public void EnableDashDamage()
    {
        dashDamageEnabled = true;
    }

    public void DisableDashDamage()
    {
        dashDamageEnabled = false;
    }

    public bool IsDashDamageEnabled()
    {
        return dashDamageEnabled;
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
