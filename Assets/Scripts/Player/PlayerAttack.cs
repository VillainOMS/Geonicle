using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    [SerializeField] private WeaponManager weaponManager;

    private void Update()
    {
        if (GameState.IsUIOpen || GameState.IsPaused)
            return;

        if (Input.GetMouseButton(0))
        {
            weaponManager.GetCurrentWeapon()?.Attack();
        }
    }
}
