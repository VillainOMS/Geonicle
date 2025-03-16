using UnityEngine;
using UnityEngine.UI;

public class PlayerAspects : MonoBehaviour
{
    public static PlayerAspects Instance { get; private set; }

    [SerializeField] private int aspectPoints = 0; // Количество очков аспектов
    [SerializeField] private Text aspectPointsText;
    [SerializeField] private GameObject aspectPointsUI;

    // Уровни аспектов
    private int fireLevel = 0;
    private int metalLevel = 0;
    private int waterLevel = 0;
    private int shockLevel = 0;

    // Настройки влияния аспектов
    [SerializeField] private float fireImpact = 0.25f;
    [SerializeField] private float metalImpact = 0.25f;
    [SerializeField] private float waterImpact = 0.25f;
    [SerializeField] private float shockImpact = 0.25f;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        UpdateUI();
    }

    public void AddAspectPoint()
    {
        aspectPoints++;
        UpdateUI();
    }

    public bool SpendAspectPoint()
    {
        if (aspectPoints > 0)
        {
            aspectPoints--;
            UpdateUI();
            return true;
        }
        return false;
    }

    public int GetAspectPoints()
    {
        return aspectPoints;
    }

    public void UpgradeAspect(string aspect)
    {
        if (aspectPoints <= 0) return;

        bool upgraded = false;

        switch (aspect)
        {
            case "Fire":
                if (fireLevel < 5) { fireLevel++; upgraded = true; }
                break;
            case "Metal":
                if (metalLevel < 5) { metalLevel++; upgraded = true; }
                break;
            case "Water":
                if (waterLevel < 5) { waterLevel++; upgraded = true; }
                break;
            case "Shock":
                if (shockLevel < 5) { shockLevel++; upgraded = true; }
                break;
        }

        if (upgraded)
        {
            SpendAspectPoint();
            PlayerStats.Instance.ApplyAspectBonuses(fireLevel, metalLevel, waterLevel, shockLevel);
        }
    }

    private void UpdateUI()
    {
        aspectPointsText.text = aspectPoints > 0 ? aspectPoints.ToString() : "";
        aspectPointsUI.SetActive(aspectPoints > 0);
    }

    // Методы для получения уровней аспектов
    public int GetFireLevel() => fireLevel;
    public int GetMetalLevel() => metalLevel;
    public int GetWaterLevel() => waterLevel;
    public int GetShockLevel() => shockLevel;

    // Методы для получения степени влияния аспектов на характеристики
    public float GetFireImpact() => fireImpact;
    public float GetMetalImpact() => metalImpact;
    public float GetWaterImpact() => waterImpact;
    public float GetShockImpact() => shockImpact;
}
