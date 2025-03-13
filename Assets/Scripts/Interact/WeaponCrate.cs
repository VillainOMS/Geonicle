using UnityEngine;

public class WeaponCrate : MonoBehaviour
{
    [SerializeField] private GameObject weaponUI; // UI выбора оружи€
    [SerializeField] private WeaponManager weaponManager; // ”правление оружием

    private void Start()
    {
        weaponUI.SetActive(false); // —крываем UI при старте
    }

    public void OpenWeaponMenu()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        weaponUI.SetActive(true);
        Time.timeScale = 0f;
    }

    public void ChooseKatana()
    {
        weaponManager.EquipWeapon(WeaponManager.WeaponType.Katana);
        CloseMenu();
    }

    public void ChooseRifle()
    {
        weaponManager.EquipWeapon(WeaponManager.WeaponType.Rifle);
        CloseMenu();
    }

    private void CloseMenu()
    {
        weaponUI.SetActive(false);
        Time.timeScale = 1f;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
}
