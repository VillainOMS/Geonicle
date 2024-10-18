using UnityEngine;

public class WeaponCrate : MonoBehaviour
{
    [SerializeField] private GameObject weaponUI; // ������ �� UI ������ ������
    [SerializeField] private WeaponManager weaponManager; // ������ �� WeaponManager

    private bool isPlayerNear = false;

    private void Update()
    {
        // ��������� ���� ��� ������� "E", ���� ����� �����
        if (isPlayerNear && Input.GetKeyDown(KeyCode.E))
        {
            Cursor.lockState = CursorLockMode.None;  // ������������ ������
            Cursor.visible = true;  // ���������� ������

            weaponUI.SetActive(true); // ���������� UI ������ ������
            Time.timeScale = 0f; // ������������� ����� �� ����� ������
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
        weaponUI.SetActive(false); // �������� UI
        Time.timeScale = 1f; // ���������� ���������� �����
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        Debug.Log("���� �������.");
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
