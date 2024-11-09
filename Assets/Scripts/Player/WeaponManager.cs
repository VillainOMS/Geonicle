using UnityEngine;

public class WeaponManager : MonoBehaviour
{
    public enum WeaponType { Katana, Rifle }

    [SerializeField] private Transform weaponHolder; // Ссылка на точку для оружия
    [SerializeField] private GameObject katanaPrefab; // Префаб катаны
    [SerializeField] private GameObject riflePrefab; // Префаб ружья

    private GameObject currentWeapon; // Текущее активное оружие
    private WeaponType currentWeaponType;

    private void Start()
    {
        EquipWeapon(WeaponType.Katana); // Изначально игрок с катаной
    }

    public void EquipWeapon(WeaponType weaponType)
    {
        // Убираем текущее оружие
        if (currentWeapon != null)
        {
            currentWeapon.SetActive(false);
        }

        currentWeaponType = weaponType;
        

        // Спавним новое оружие
        switch (weaponType)
        {
            case WeaponType.Katana:
                currentWeapon = katanaPrefab;
                break;
            case WeaponType.Rifle:
                currentWeapon = riflePrefab;
                break;
        }
        currentWeapon.SetActive(true);
    }

    public WeaponType GetCurrentWeaponType()
    {
        return currentWeaponType;
    }
}
