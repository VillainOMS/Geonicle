using UnityEngine;

public class WeaponCrate : MonoBehaviour
{
    [SerializeField] private GameObject weaponUI; // Ссылка на UI выбора оружия
    [SerializeField] private WeaponManager weaponManager; // Ссылка на WeaponManager

    private bool isPlayerNear = false;

    private void Update()
    {
        // Открываем меню при нажатии "E", если игрок рядом
        if (isPlayerNear && Input.GetKeyDown(KeyCode.E))
        {
            Cursor.lockState = CursorLockMode.None;  // Разблокируем курсор
            Cursor.visible = true;  // Показываем курсор

            weaponUI.SetActive(true); // Показываем UI выбора оружия
            Time.timeScale = 0f; // Останавливаем время на время выбора
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
        weaponUI.SetActive(false); // Скрываем UI
        Time.timeScale = 1f; // Возвращаем нормальное время
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        Debug.Log("Меню закрыто.");
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerNear = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerNear = false;
        }
    }
}
