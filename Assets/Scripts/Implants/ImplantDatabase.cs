using UnityEngine;
using System.Collections.Generic;

public class ImplantDatabase : MonoBehaviour
{
    public static ImplantDatabase Instance { get; private set; }

    [SerializeField] private Sprite glassSkeletonIcon;
    [SerializeField] private Sprite jetLegsIcon;
    [SerializeField] private Sprite steamHeartIcon;
    [SerializeField] private Sprite trackingEyesIcon;

    [SerializeField] private Sprite steelBonesIcon;
    [SerializeField] private Sprite flameCoreIcon;
    [SerializeField] private Sprite catalystIcon;
    [SerializeField] private Sprite hydroDriveIcon;
    [SerializeField] private Sprite dashArmorIcon;
    [SerializeField] private Sprite tacticInterfaceIcon;
    [SerializeField] private Sprite combatFrameIcon;
    [SerializeField] private Sprite diaphragmIcon;
    [SerializeField] private Sprite brainShieldIcon;
    [SerializeField] private Sprite electroStepIcon;

    private List<Implant> implants = new List<Implant>();

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            InitializeImplants();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void InitializeImplants()
    {
        // ID 1�4: ������ ��������, ��������� �������
        implants.Add(new Implant(
            1, "���������� ������", "���� +25%, �������� -50%",
            "����� �������� ����������� ������",
            "������", glassSkeletonIcon,
            2, 0, 2, 0,
            0.25f, -0.5f, 0f, 0f,
            0.25f, -0.25f, 0f, 0f
        ));

        implants.Add(new Implant(
            2, "���������� ����", "�������� +10%",
            "������� ������",
            "����", jetLegsIcon,
            1, 2, 0, 0,
            0f, 0f, 0.1f, 0f,
            0f, 0f, 0.1f, 0f,
            (abilities) => abilities.EnableDoubleJump(),
            (abilities) => abilities.DisableDoubleJump()
        ));

        implants.Add(new Implant(
            3, "������� ������", "�������� +20%",
            "������������� +15% � �������� ����� � ������������",
            "������", steamHeartIcon,
            3, 2, 0, 0,
            0f, 0.2f, 0f, 0f,
            0f, 0.2f, 0.15f, 0.15f
        ));

        implants.Add(new Implant(
            4, "����� � ����������", "���� +10%",
            "��� �������� ����� ������� 20 ����� ���������� �������",
            "�����", trackingEyesIcon,
            0, 0, 0, 5,
            0.1f, 0f, 0f, 0f,
            0.1f, 0f, 0f, 0f
        ));

        implants.Add(new Implant(
            5, "�������� �����", "�������� +25%",
            "��������������� 2 HP �� ��������",
            "������", steelBonesIcon,
            0, 1, 3, 0,
            0f, 0.25f, 0f, 0f,
            0f, 0.25f, 0f, 0f
        ));

        implants.Add(new Implant(
            6, "��������� ����", "���� +10%",
            "+15% � �������� �����",
            "������", flameCoreIcon,
            3, 0, 0, 0,
            0.1f, 0f, 0f, 0f,
            0.1f, 0f, 0f, 0.15f
        ));

        implants.Add(new Implant(
            7, "�����������", "��� �������� � ������� ������",
            "+15% �� ���� ���������������",
            "������", catalystIcon,
            2, 2, 2, 2,
            0f, 0f, 0f, 0f,
            0.15f, 0.15f, 0.15f, 0.15f
        ));

        implants.Add(new Implant(
            8, "��������������", "�������� +15%",
            "+15% � ������ ������",
            "����", hydroDriveIcon,
            0, 2, 0, 0,
            0f, 0f, 0.15f, 0f,
            0f, 0f, 0.15f, 0f
        ));

        implants.Add(new Implant(
            9, "�������� �������", "�������� +15%",
            "������������ 0.5 ��� ����� �����",
            "����", dashArmorIcon,
            0, 0, 3, 1,
            0f, 0.15f, 0f, 0f,
            0f, 0.15f, 0f, 0f
        ));

        implants.Add(new Implant(
            10, "����������� ���������", "�������� ����� +10%",
            "+10% �����",
            "�����", tacticInterfaceIcon,
            2, 0, 0, 1,
            0f, 0f, 0f, 0.1f,
            0.1f, 0f, 0f, 0.1f
        ));

        implants.Add(new Implant(
            11, "������ ������", "���� +10%, �������� +10%",
            "+10% � �������� �����",
            "������", combatFrameIcon,
            1, 0, 2, 0,
            0.1f, 0.1f, 0f, 0f,
            0.1f, 0.1f, 0f, 0.1f
        ));

        implants.Add(new Implant(
            12, "��������� ���������", "���� +5%, �������� +5%",
            "+10% ��������, +10% �������� �����",
            "������", diaphragmIcon,
            2, 0, 2, 1,
            0.05f, 0f, 0.05f, 0f,
            0.05f, 0.1f, 0.05f, 0.1f
        ));

        implants.Add(new Implant(
            13, "�������� ������", "�������� +10%",
            "���������� ������ ���������� ���� � �������",
            "������", brainShieldIcon,
            0, 0, 2, 3,
            0f, 0.1f, 0f, 0f,
            0f, 0.1f, 0f, 0f
        ));

        implants.Add(new Implant(
            14, "������������", "�������� +10%",
            "����� ������� 10 ����� ������ �����",
            "����", electroStepIcon,
            1, 0, 0, 2,
            0f, 0f, 0.1f, 0f,
            0f, 0f, 0.1f, 0f
        ));
    }

    public List<Implant> GetAllImplants() => implants;
}
