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
            "Стеклянный скелет",
            "Увеличивает урон на 25%, но снижает здоровье на 50%.",
            "Скелет",
            glassSkeletonIcon,
            2, 0, 2, 0, // Требования аспектов
            0.25f, -0.5f, 0f, 0f,
            (playerStats) => { },
            (playerStats) => { },
            (playerStats) => { playerStats.ApplyPercentageBonus(0f, 0.5f, 0f, 0f); }, // Улучшенный эффект
            (playerStats) => { playerStats.RemovePercentageBonus(0f, 0.5f, 0f, 0f); }
        ));

        implants.Add(new Implant(
            2,
            "Реактивные ноги",
            "Увеличивает скорость передвижения на 20%.",
            "Ноги",
            jetLegsIcon,
            1, 2, 0, 0, // Требования аспектов
            0f, 0f, 0.2f, 0f,
            (playerStats) => { },
            (playerStats) => { },
            (playerStats) => { playerStats.ApplyPercentageBonus(0f, 0f, 0.15f, 0f); }, // Улучшенные эффекты
            (playerStats) => { playerStats.RemovePercentageBonus(0f, 0f, 0.15f, 0f); },
            (abilities) => abilities.EnableDoubleJump(),
            (abilities) => abilities.DisableDoubleJump()
        ));

        implants.Add(new Implant(
            3,
            "Паровое сердце",
            "Увеличивает здоровье на 20%.",
            "Сердце",
            steamHeartIcon,
            3, 2, 0, 0,
            0f, 0.2f, 0f, 0f,
            (playerStats) => { },
            (playerStats) => { },
            (playerStats) => { playerStats.ApplyPercentageBonus(0f, 0f, 0.2f, 0f); }, // Улучшенная версия
            (playerStats) => { playerStats.RemovePercentageBonus(0f, 0f, 0.2f, 0f); }
        ));

        implants.Add(new Implant(
            4,
            "Глаза с наведением",
            "Увеличивает урон на 20%.",
            "Глаза",
            trackingEyesIcon,
            0, 0, 0, 5,
            0.2f, 0f, 0f, 0f,
            (playerStats) => { },
            (playerStats) => { },
            (playerStats) => { playerStats.ApplyPercentageBonus(0.1f, 0f, 0f, 0f); }, // Улучшенный эффект (+10% урона)
            (playerStats) => { playerStats.RemovePercentageBonus(0.1f, 0f, 0f, 0f); }
        ));
    }

    public List<Implant> GetAllImplants()
    {
        return implants;
    }
}
