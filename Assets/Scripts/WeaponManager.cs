using UnityEngine;

public class WeaponManager : MonoBehaviour
{
    public enum WeaponType { Katana, Rifle }
    public WeaponType currentWeapon; // Текущее выбранное оружие

    // Ссылки на катану и винтовку
    [SerializeField] private GameObject katana;
    [SerializeField] private GameObject rifle;

    void Start()
    {
        // Убираем оба оружия при старте, они будут активированы при выборе
        katana.SetActive(false);
        rifle.SetActive(false);
    }

    public void EquipWeapon(WeaponType weapon)
    {
        // Отключаем оба оружия перед активацией нового
        katana.SetActive(false);
        rifle.SetActive(false);

        // Включаем выбранное оружие
        switch (weapon)
        {
            case WeaponType.Katana:
                katana.SetActive(true);
                break;
            case WeaponType.Rifle:
                rifle.SetActive(true);
                break;
        }

        currentWeapon = weapon;
        Debug.Log("Weapon equipped: " + currentWeapon);
    }
}
