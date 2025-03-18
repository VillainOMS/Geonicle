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
            Debug.Log($"[AddNewImplant] ������� ��������� ������ ��������: {implant.Name}. �������� �� ��������.");
            return; 
        }

        implants.Add(implant);
        Debug.Log($"����� ������� ����� �������: {implant.Name}");
        inventoryManager?.UpdateInventoryUI();
    }



    // ������� �������� ������� � ��������� (��� �������� �� ���������)
    public void ReturnImplantToInventory(Implant implant)
    {
        if (!implants.Contains(implant))
        {
            implants.Add(implant);
            Debug.Log($"������� {implant.Name} ��������� � ���������.");
            inventoryManager?.UpdateInventoryUI();
        }
    }

    public void RemoveImplant(Implant implant)
    {
        if (implants.Contains(implant))
        {
            implants.Remove(implant);
            Debug.Log($"������� {implant.Name} ����� �� ���������.");
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
            Debug.Log($"[EquipImplant] ���������� �������: {implant.Name}. ������������� ��������������.");
            PlayerStats.Instance.RecalculateActualStats();
        }
    }

    public void UnequipImplant(Implant implant)
    {
        if (equippedImplants.Contains(implant))
        {
            equippedImplants.Remove(implant);
            Debug.Log($"[UnequipImplant] ���� �������: {implant.Name}. ������������� ��������������.");
            PlayerStats.Instance.RecalculateActualStats();
        }
    }

    private void ApplyImplantEffects(Implant implant)
    {
        bool isEnhanced = implant.CheckIfEnhanced();
        implant.SetEnhanced(isEnhanced);

        Debug.Log($"���������� ��������: {implant.Name}, ���������: {isEnhanced}");

        implant.ApplyEffect(playerStats, playerAbilities);
        playerStats.RecalculateActualStats();
        Debug.Log($"����� ����������: ����={playerStats.actualDamageMultiplier}, ��������={playerStats.actualMaxHealth}, ��������={playerStats.actualMoveSpeed}");
    }

    private void RemoveImplantEffects(Implant implant)
    {
        implant.RemoveEffect(playerStats, playerAbilities);
        Debug.Log($"���� �������: {implant.Name}");
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
