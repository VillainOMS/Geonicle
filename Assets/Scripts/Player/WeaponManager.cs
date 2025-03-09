using UnityEngine;

public class WeaponManager : MonoBehaviour
{
    public enum WeaponType { Katana, Rifle }

    [SerializeField] private WeaponBase katana;
    [SerializeField] private WeaponBase rifle;
    private WeaponBase currentWeapon;
    private PlayerStats playerStats;

    private void Start()
    {
        playerStats = FindObjectOfType<PlayerStats>();
        if (playerStats == null)
        {
            Debug.LogError("PlayerStats не найден!");
        }

        EquipWeapon(WeaponType.Katana);
    }

    public void EquipWeapon(WeaponType weaponType)
    {
        if (currentWeapon != null)
        {
            currentWeapon.gameObject.SetActive(false);
        }

        switch (weaponType)
        {
            case WeaponType.Katana:
                currentWeapon = katana;
                break;
            case WeaponType.Rifle:
                currentWeapon = rifle;
                break;
        }

        if (currentWeapon != null)
        {
            currentWeapon.gameObject.SetActive(true);
            currentWeapon.Initialize(playerStats); // Передаём PlayerStats в оружие
        }
        else
        {
            Debug.LogError($"WeaponManager: Не удалось переключиться на {weaponType}");
        }
    }

    public WeaponBase GetCurrentWeapon()
    {
        return currentWeapon;
    }
}
