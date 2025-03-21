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

            // ���������, ������������� �� ������� ����������� ��������
            bool isEnhanced = implant.CheckIfEnhanced();
            implant.SetEnhanced(isEnhanced);

            Debug.Log($"[EquipImplant] ���������� �������: {implant.Name}. ��������� ������: {isEnhanced}");

            PlayerStats.Instance.RecalculateActualStats();
        }
    }


    public void UnequipImplant(Implant implant)
    {
        if (equippedImplants.Contains(implant))
        {
            Debug.Log($"[UnequipImplant] ���� �������: {implant.Name}. ������������� ��������������.");

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
