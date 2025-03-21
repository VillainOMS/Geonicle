using System.Collections.Generic;
using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    public static PlayerInventory Instance { get; private set; }

    private List<Implant> implants = new List<Implant>();
    private List<Implant> equippedImplants = new List<Implant>();

    public InventoryManager inventoryManager;
    private PlayerStats playerStats;
    private PlayerAbilities playerAbilities;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);

        playerStats = PlayerStats.Instance;
        playerAbilities = FindObjectOfType<PlayerAbilities>();
    }

    public void SetInventoryManager(InventoryManager manager)
    {
        inventoryManager = manager;
    }

    public void AddNewImplant(Implant implant)
    {
        if (HasImplantAnywhere(implant)) 
        {
            Debug.Log($"[AddNewImplant] Попытка повторной выдачи импланта: {implant.Name}. Дубликат не добавлен.");
            return; 
        }

        implants.Add(implant);
        Debug.Log($"Игрок получил новый имплант: {implant.Name}");
        inventoryManager?.UpdateInventoryUI();
    }



    // Возврат импланта обратно в инвентарь (без проверки на дубликаты)
    public void ReturnImplantToInventory(Implant implant)
    {
        if (!implants.Contains(implant))
        {
            implants.Add(implant);
            Debug.Log($"Имплант {implant.Name} возвращён в инвентарь.");
            inventoryManager?.UpdateInventoryUI();
        }
    }

    public void RemoveImplant(Implant implant)
    {
        if (implants.Contains(implant))
        {
            implants.Remove(implant);
            Debug.Log($"Имплант {implant.Name} удалён из инвентаря.");
            inventoryManager?.UpdateInventoryUI();
        }
    }

    public bool HasImplantAnywhere(Implant implant)
    {
        if (implants.Exists(i => i.ID == implant.ID))
            return true;

        if (equippedImplants.Exists(i => i.ID == implant.ID))
            return true;

        return false;
    }



    public bool IsImplantEquipped(Implant implant)
    {
        foreach (var slot in FindObjectsOfType<ImplantSlot>())
        {
            if (slot.GetCurrentImplant() == implant)
            {
                return true;
            }
        }
        return false;
    }


    public List<Implant> GetImplants()
    {
        return new List<Implant>(implants);
    }

    public void EquipImplant(Implant implant)
    {
        if (!equippedImplants.Contains(implant))
        {
            equippedImplants.Add(implant);

            // Проверяем, соответствует ли имплант требованиям аспектов
            bool isEnhanced = implant.CheckIfEnhanced();
            implant.SetEnhanced(isEnhanced);

            Debug.Log($"[EquipImplant] Экипирован имплант: {implant.Name}. Усиленная версия: {isEnhanced}");

            PlayerStats.Instance.RecalculateActualStats();
        }
    }


    public void UnequipImplant(Implant implant)
    {
        if (equippedImplants.Contains(implant))
        {
            Debug.Log($"[UnequipImplant] Снят имплант: {implant.Name}. Пересчитываем характеристики.");

            implant.RemoveEffect(playerStats, playerAbilities);
            equippedImplants.Remove(implant);

            implant.SetEnhanced(false);
            PlayerStats.Instance.RecalculateActualStats();
        }
    }


    public List<Implant> GetEquippedImplants()
    {
        return new List<Implant>(equippedImplants);
    }

    public PlayerAbilities GetPlayerAbilities()
    {
        if (playerAbilities == null)
        {
            playerAbilities = FindObjectOfType<PlayerAbilities>();
        }
        return playerAbilities;
    }

}
