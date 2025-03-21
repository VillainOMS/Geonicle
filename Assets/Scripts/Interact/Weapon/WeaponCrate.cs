using System.Collections;
using UnityEngine;

public class WeaponCrate : MonoBehaviour
{
    [SerializeField] private GameObject weaponUI; // UI ������ ������
    [SerializeField] private WeaponManager weaponManager; // ���������� �������
    private bool isMenuOpen = false; // ���� ��������� ����

    private void Start()
    {
        weaponUI.SetActive(false); // �������� UI ��� ������
    }

    private void Update()
    {
        // �������� UI ��� ������� ESC
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
