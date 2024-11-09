using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    [SerializeField] private WeaponManager weaponManager;
    [SerializeField] private KatanaAttack katanaAttack;
    [SerializeField] private RifleAttack rifleAttack;

    private void Update()
    {
        if (Input.GetMouseButtonDown(0)) // ЛКМ для атаки
        {
            if (weaponManager.GetCurrentWeaponType() == WeaponManager.WeaponType.Katana)
            {
                katanaAttack.Attack();
            }
            else if (weaponManager.GetCurrentWeaponType() == WeaponManager.WeaponType.Rifle)
            {
                rifleAttack.Shoot();
            }
        }
    }
}
