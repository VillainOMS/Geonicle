using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    [SerializeField] private WeaponManager weaponManager;

    private void Update()
    {
        if (Input.GetMouseButtonDown(0)) // ЛКМ для атаки
        {
            weaponManager.GetCurrentWeapon()?.Attack();
        }
    }
}
