using System.Collections.Generic;
using UnityEngine;

public class ImplantDatabase : MonoBehaviour
{
    public static ImplantDatabase Instance { get; private set; }

    [SerializeField] private Sprite glassSkeletonIcon;
    [SerializeField] private Sprite jetLegsIcon;
    [SerializeField] private Sprite steamHeartIcon;
    [SerializeField] private Sprite trackingEyesIcon;

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
        implants.Add(new Implant(
            1,
            "���������� ������",
            "����������� ���� �� 25%, �� ������� �������� �� 50%.",
            "������",
            glassSkeletonIcon,
            (playerStats) =>
            {
                playerStats.actualDamageMultiplier += 0.25f;
                playerStats.actualMaxHealth = Mathf.RoundToInt(playerStats.actualMaxHealth * 0.5f);
            },
            (playerStats) =>
            {
                playerStats.actualDamageMultiplier -= 0.25f;
                playerStats.actualMaxHealth = Mathf.RoundToInt(playerStats.actualMaxHealth * 2f);
            },
            (playerStats) =>
            {
                if (PlayerAspects.Instance.GetWaterLevel() >= 2 && PlayerAspects.Instance.GetMetalLevel() >= 2)
                {
                    playerStats.actualMaxHealth = Mathf.RoundToInt(playerStats.actualMaxHealth * 1.5f);
                }
            },
            (playerStats) =>
            {
                if (PlayerAspects.Instance.GetWaterLevel() >= 2 && PlayerAspects.Instance.GetMetalLevel() >= 2)
                {
                    playerStats.actualMaxHealth = Mathf.RoundToInt(playerStats.actualMaxHealth / 1.5f);
                }
            }
        ));

        implants.Add(new Implant(
            2,
            "���������� ����",
            "����������� �������� ������������ �� 20%.",
            "����",
            jetLegsIcon,
            (playerStats) =>
            {
                playerStats.actualMoveSpeed += 0.2f;
            },
            (playerStats) =>
            {
                playerStats.actualMoveSpeed -= 0.2f;
            },
            (playerStats) =>
            {
                if (PlayerAspects.Instance.GetWaterLevel() >= 1 && PlayerAspects.Instance.GetFireLevel() >= 2)
                {
                    playerStats.actualMoveSpeed += 0.15f;
                }
            },
            (playerStats) =>
            {
                if (PlayerAspects.Instance.GetWaterLevel() >= 1 && PlayerAspects.Instance.GetFireLevel() >= 2)
                {
                    playerStats.actualMoveSpeed -= 0.15f;
                }
            }
        ));

        implants.Add(new Implant(
            3,
            "������� ������",
            "����������� �������� �� 20%.",
            "������",
            steamHeartIcon,
            (playerStats) =>
            {
                playerStats.actualMaxHealth = Mathf.RoundToInt(playerStats.actualMaxHealth * 1.2f);
            },
            (playerStats) =>
            {
                playerStats.actualMaxHealth = Mathf.RoundToInt(playerStats.actualMaxHealth / 1.2f);
            },
            (playerStats) =>
            {
                if (PlayerAspects.Instance.GetFireLevel() >= 3 && PlayerAspects.Instance.GetWaterLevel() >= 2)
                {
                    playerStats.actualMoveSpeed += 0.15f;
                    playerStats.actualAttackSpeedMultiplier += 0.15f;
                }
            },
            (playerStats) =>
            {
                if (PlayerAspects.Instance.GetFireLevel() >= 3 && PlayerAspects.Instance.GetWaterLevel() >= 2)
                {
                    playerStats.actualMoveSpeed -= 0.15f;
                    playerStats.actualAttackSpeedMultiplier -= 0.15f;
                }
            }
        ));

        implants.Add(new Implant(
            4,
            "����� � ����������",
            "����������� ���� �� 20%.",
            "�����",
            trackingEyesIcon,
            (playerStats) =>
            {
                playerStats.actualDamageMultiplier += 0.2f;
            },
            (playerStats) =>
            {
                playerStats.actualDamageMultiplier -= 0.2f;
            },
            (playerStats) => 
            {
                if (PlayerAspects.Instance.GetShockLevel() >= 5)
                {
                    playerStats.actualDamageMultiplier += 0.1f;
                }
            },
            (playerStats) =>
            {
                if (PlayerAspects.Instance.GetShockLevel() >= 5)
                {
                    playerStats.actualDamageMultiplier -= 0.1f;
                }
            }
        ));
    }

    public List<Implant> GetAllImplants()
    {
        return implants;
    }
}
