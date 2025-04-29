using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryManager : MonoBehaviour
{
    [Header("Инвентарь")]
    [SerializeField] public Transform inventoryPanel;
    [SerializeField] private GameObject implantPrefab;

    [Header("Панель информации")]
    [SerializeField] private Text implantNameText;
    [SerializeField] private Text implantDescriptionText;
    [SerializeField] private Transform requiredAspectsPanel;
    [SerializeField] private Text enhancedEffectText;
    [SerializeField] private GameObject aspectIconPrefab;

    [Header("Иконки аспектов")]
    [SerializeField] private Sprite fireSprite;
    [SerializeField] private Sprite waterSprite;
    [SerializeField] private Sprite metalSprite;
    [SerializeField] private Sprite shockSprite;

    private void Start()
    {
        PlayerInventory.Instance.SetInventoryManager(this);
        UpdateInventoryUI();
    }

    public void UpdateInventoryUI()
    {
        Debug.Log($"Обновляем UI. Количество имплантов в инвентаре: {PlayerInventory.Instance.GetImplants().Count}");

        // Удаляем старые элементы
        foreach (Transform child in inventoryPanel)
        {
            Destroy(child.gameObject);
        }

        // Добавляем только те импланты, которые НЕ экипированы
        foreach (Implant implant in PlayerInventory.Instance.GetImplants())
        {
            if (!PlayerInventory.Instance.IsImplantEquipped(implant))
            {
                GameObject implantUI = Instantiate(implantPrefab, inventoryPanel);
                implantUI.GetComponent<ImplantUI>().Setup(implant, this);
            }
        }
    }

    public void ShowImplantInfo(Implant implant)
    {
        implantNameText.text = implant.Name;
        implantDescriptionText.text = implant.Description;
        enhancedEffectText.text = implant.EnhancedDescription;

        bool isEnhanced = PlayerAspects.Instance.HasEnoughAspects(implant); // !!! Поясню ниже что это
        UpdateEnhancedEffectVisual(isEnhanced);

        // Удаляем старые иконки
        foreach (Transform child in requiredAspectsPanel)
        {
            Destroy(child.gameObject);
        }

        // Получаем последовательность аспектов
        List<string> requiredAspects = implant.GetAspectSequence();

        // Получаем уровень аспектов игрока
        Dictionary<string, int> playerAspects = new()
        {
            { "Fire", PlayerAspects.Instance.GetFireLevel() },
            { "Water", PlayerAspects.Instance.GetWaterLevel() },
            { "Metal", PlayerAspects.Instance.GetMetalLevel() },
            { "Shock", PlayerAspects.Instance.GetShockLevel() }
        };

        // Сопоставление иконок
        Dictionary<string, Sprite> aspectSprites = new()
        {
            { "Fire", fireSprite },
            { "Water", waterSprite },
            { "Metal", metalSprite },
            { "Shock", shockSprite }
        };

        // Счётчик уже использованных аспектов
        Dictionary<string, int> used = new()
        {
            { "Fire", 0 },
            { "Water", 0 },
            { "Metal", 0 },
            { "Shock", 0 }
        };

        foreach (string aspect in requiredAspects)
        {
            GameObject iconGO = Instantiate(aspectIconPrefab, requiredAspectsPanel);
            Image iconImage = iconGO.GetComponent<Image>();
            iconImage.sprite = aspectSprites[aspect];

            used[aspect]++;
            bool hasEnough = used[aspect] <= playerAspects[aspect];
            iconImage.color = hasEnough ? Color.white : new Color(1f, 1f, 1f, 0.3f);
        }
    }

    public void HideImplantInfo()
    {
        implantNameText.text = "";
        implantDescriptionText.text = "";
        enhancedEffectText.text = "";

        foreach (Transform child in requiredAspectsPanel)
        {
            Destroy(child.gameObject);
        }
    }

    private void UpdateEnhancedEffectVisual(bool isEnhanced)
    {
        if (isEnhanced)
        {
            // Насыщенный чёрный
            enhancedEffectText.color = new Color(0.1f, 0.1f, 0.1f);
        }
        else
        {
            // Светлый серо-чёрный
            enhancedEffectText.color = new Color(0.5f, 0.5f, 0.5f);
        }
    }
}
