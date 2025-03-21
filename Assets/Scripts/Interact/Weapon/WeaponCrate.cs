using System.Collections;
using UnityEngine;

public class WeaponCrate : MonoBehaviour
{
    [SerializeField] private GameObject weaponUI; // UI выбора оружия
    [SerializeField] private WeaponManager weaponManager; // Управление оружием
    private bool isMenuOpen = false; // Флаг состояния меню

    private void Start()
    {
        weaponUI.SetActive(false); // Скрываем UI при старте
    }

    private void Update()
    {
        // Закрытие UI при нажатии ESC
        if (isMenuOpen && Input.GetKeyDown(KeyCode.Escape))
        {
            CloseMenu();
        }
    }

    public void OpenWeaponMenu()
    {
        GameState.IsUIOpen = true;
        if (!isMenuOpen)
        {
            isMenuOpen = true;
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            weaponUI.SetActive(true);
            Time.timeScale = 0f;
        }
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
        GameState.IsUIOpen = false;
        isMenuOpen = false;
        weaponUI.SetActive(false);
        Time.timeScale = 1f;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        FindObjectOfType<PauseMenu>()?.BlockPauseForOneFrame();
    }
}
