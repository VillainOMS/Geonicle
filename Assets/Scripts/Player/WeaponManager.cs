using UnityEngine;

public class WeaponManager : MonoBehaviour
{
    public enum WeaponType { Katana, Rifle }

    [SerializeField] private Transform weaponHolder; // ������ �� ����� ��� ������
    [SerializeField] private GameObject katanaPrefab; // ������ ������
    [SerializeField] private GameObject riflePrefab; // ������ �����

    private GameObject currentWeapon; // ������� �������� ������
    private WeaponType currentWeaponType;

    private void Start()
    {
        EquipWeapon(WeaponType.Katana); // ���������� ����� � �������
    }

    public void EquipWeapon(WeaponType weaponType)
    {
        // ������� ������� ������
        if (currentWeapon != null)
        {
            currentWeapon.SetActive(false);
        }

        currentWeaponType = weaponType;
        

        // ������� ����� ������
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
