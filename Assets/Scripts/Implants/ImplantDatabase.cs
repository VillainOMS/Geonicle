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
        // ID 1–4: старые импланты, сохраняем порядок
        implants.Add(new Implant(
            1, "Стеклянный скелет", "Урон +25%, Здоровье -50%",
            "Штраф здоровья значительно снижен",
            "Скелет", glassSkeletonIcon,
            2, 0, 2, 0,
            0.25f, -0.5f, 0f, 0f,
            0.25f, -0.25f, 0f, 0f
        ));

        implants.Add(new Implant(
            2, "Реактивные ноги", "Скорость +10%",
            "Двойной прыжок",
            "Ноги", jetLegsIcon,
            1, 2, 0, 0,
            0f, 0f, 0.1f, 0f,
            0f, 0f, 0.1f, 0f,
            (abilities) => abilities.EnableDoubleJump(),
            (abilities) => abilities.DisableDoubleJump()
        ));

        implants.Add(new Implant(
            3, "Паровое сердце", "Здоровье +20%",
            "Дополнительно +15% к скорости атаки и передвижения",
            "Сердце", steamHeartIcon,
            3, 2, 0, 0,
            0f, 0.2f, 0f, 0f,
            0f, 0.2f, 0.15f, 0.15f
        ));

        implants.Add(new Implant(
            4, "Глаза с наведением", "Урон +10%",
            "При убийстве врага наносит 20 урона случайному другому",
            "Глаза", trackingEyesIcon,
            0, 0, 0, 5,
            0.1f, 0f, 0f, 0f,
            0.1f, 0f, 0f, 0f
        ));

        implants.Add(new Implant(
            5, "Стальные кости", "Здоровье +25%",
            "Восстанавливает 2 HP за убийство",
            "Скелет", steelBonesIcon,
            0, 1, 3, 0,
            0f, 0.25f, 0f, 0f,
            0f, 0.25f, 0f, 0f
        ));

        implants.Add(new Implant(
            6, "Пламенное ядро", "Урон +10%",
            "+15% к скорости атаки",
            "Сердце", flameCoreIcon,
            3, 0, 0, 0,
            0.1f, 0f, 0f, 0f,
            0.1f, 0f, 0f, 0.15f
        ));

        implants.Add(new Implant(
            7, "Катализатор", "Нет эффектов в обычной версии",
            "+15% ко всем характеристикам",
            "Сердце", catalystIcon,
            2, 2, 2, 2,
            0f, 0f, 0f, 0f,
            0.15f, 0.15f, 0.15f, 0.15f
        ));

        implants.Add(new Implant(
            8, "Гидродвигатели", "Скорость +15%",
            "+15% к высоте прыжка",
            "Ноги", hydroDriveIcon,
            0, 2, 0, 0,
            0f, 0f, 0.15f, 0f,
            0f, 0f, 0.15f, 0f
        ));

        implants.Add(new Implant(
            9, "Защитные тормоза", "Здоровье +15%",
            "Неуязвимость 0.5 сек после рывка",
            "Ноги", dashArmorIcon,
            0, 0, 3, 1,
            0f, 0.15f, 0f, 0f,
            0f, 0.15f, 0f, 0f
        ));

        implants.Add(new Implant(
            10, "Тактический интерфейс", "Скорость атаки +10%",
            "+10% урона",
            "Глаза", tacticInterfaceIcon,
            2, 0, 0, 1,
            0f, 0f, 0f, 0.1f,
            0.1f, 0f, 0f, 0.1f
        ));

        implants.Add(new Implant(
            11, "Боевой каркас", "Урон +10%, Здоровье +10%",
            "+10% к скорости атаки",
            "Скелет", combatFrameIcon,
            1, 0, 2, 0,
            0.1f, 0.1f, 0f, 0f,
            0.1f, 0.1f, 0f, 0.1f
        ));

        implants.Add(new Implant(
            12, "Усиленная диафрагма", "Урон +5%, Скорость +5%",
            "+10% здоровья, +10% скорости атаки",
            "Особое", diaphragmIcon,
            2, 0, 2, 1,
            0.05f, 0f, 0.05f, 0f,
            0.05f, 0.1f, 0.05f, 0.1f
        ));

        implants.Add(new Implant(
            13, "Мозговая защита", "Здоровье +10%",
            "Игнорирует первый полученный урон в комнате",
            "Особое", brainShieldIcon,
            0, 0, 2, 3,
            0f, 0.1f, 0f, 0f,
            0f, 0.1f, 0f, 0f
        ));

        implants.Add(new Implant(
            14, "Электроступы", "Скорость +10%",
            "Рывок наносит 10 урона врагам рядом",
            "Ноги", electroStepIcon,
            1, 0, 0, 2,
            0f, 0f, 0.1f, 0f,
            0f, 0f, 0.1f, 0f
        ));
    }

    public List<Implant> GetAllImplants() => implants;
}
