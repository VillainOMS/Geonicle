using UnityEngine;
using UnityEngine.UI;

public class PlayerAspects : MonoBehaviour
{
    public static PlayerAspects Instance { get; private set; }

    [SerializeField] private int aspectPoints = 0; // Количество очков аспектов

    [SerializeField] private Text aspectPointsText; // Текст очков аспектов
    [SerializeField] private GameObject aspectPointsUI; // UI-элемент с очками аспектов

    // Настройки влияния аспектов (редактируемые в Inspector)
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
        Debug.Log($"Добавлено очко аспектов. Всего: {aspectPoints}");
        UpdateUI();
    }

    private void UpdateUI()
    {
        Debug.Log($"Обновление UI. Очки аспектов: {aspectPoints}");

        if (aspectPoints > 0)
        {
            aspectPointsUI.SetActive(true);
            aspectPointsText.text = aspectPoints.ToString();
        }
        else
        {
            aspectPointsUI.SetActive(false);
        }
    }

    public int GetAspectPoints()
    {
        return aspectPoints;
    }

    public bool SpendAspectPoint()
    {
        if (aspectPoints > 0)
        {
            aspectPoints--;
            Debug.Log($"Потрачено очко аспектов. Осталось: {aspectPoints}");
            UpdateUI();
            return true;
        }
        Debug.Log("Нет доступных очков аспектов!");
        return false;
    }

    // Методы для получения влияния аспектов
    public float GetFireImpact() => fireImpact;
    public float GetMetalImpact() => metalImpact;
    public float GetWaterImpact() => waterImpact;
    public float GetShockImpact() => shockImpact;
}
