using UnityEngine;

public class WeaponManager : MonoBehaviour
{
    public enum WeaponType { Katana, Rifle }
    public WeaponType currentWeapon; // ������� ��������� ������

    // ������ �� ������ � ��������
    [SerializeField] private GameObject katana;
    [SerializeField] private GameObject rifle;

    void Start()
    {
        // ������� ��� ������ ��� ������, ��� ����� ������������ ��� ������
        katana.SetActive(false);
        rifle.SetActive(false);
    }

    public void EquipWeapon(WeaponType weapon)
    {
        // ��������� ��� ������ ����� ���������� ������
        katana.SetActive(false);
        rifle.SetActive(false);

        // �������� ��������� ������
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
