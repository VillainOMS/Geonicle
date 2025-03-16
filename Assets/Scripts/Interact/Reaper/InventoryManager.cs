using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryManager : MonoBehaviour
{
    [SerializeField] public Transform inventoryPanel;
    [SerializeField] private GameObject implantPrefab;
    [SerializeField] private Text implantDescriptionText;

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

    public void ShowImplantDescription(string description)
    {
        implantDescriptionText.text = description;
    }

    public void HideImplantDescription()
    {
        implantDescriptionText.text = "";
    }
}
