using UnityEngine;
using System.Collections.Generic;

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
            1, "Стеклянный скелет", "Урон +25%, Здоровье -50%", "Скелет", glassSkeletonIcon,
            2, 0, 2, 0,
            0.25f, -0.5f, 0f, 0f,
            0f, 0.25f, 0f, 0f
        ));

        implants.Add(new Implant(
            2, "Реактивные ноги", "Скорость +20%", "Ноги", jetLegsIcon,
            1, 2, 0, 0,
            0f, 0f, 0.2f, 0f,
            0f, 0f, 0.1f, 0f,
            (abilities) => abilities.EnableDoubleJump(),
            (abilities) => abilities.DisableDoubleJump()
        ));

        implants.Add(new Implant(
            3, "Паровое сердце", "Здоровье +20%", "Сердце", steamHeartIcon,
            3, 2, 0, 0,
            0f, 0.2f, 0f, 0f,
            0f, 0f, 0.2f, 0f
        ));

        implants.Add(new Implant(
            4, "Глаза с наведением", "Урон +20%", "Глаза", trackingEyesIcon,
            0, 0, 0, 5,
            0.2f, 0f, 0f, 0f,
            0.1f, 0f, 0f, 0f
        ));
    }

    public List<Implant> GetAllImplants() => implants;
}
