using UnityEngine;
using UnityEngine.EventSystems;

public class ImplantSlot : MonoBehaviour, IDropHandler
{
    [SerializeField] public string slotType;
    private Implant currentImplant;

    public void OnDrop(PointerEventData eventData)
    {
        ImplantUI implantUI = eventData.pointerDrag.GetComponent<ImplantUI>();

        if (implantUI != null && implantUI.ImplantData.Slot == slotType)
        {
            if (currentImplant != null)
            {
                RemoveImplant();
            }

            currentImplant = implantUI.ImplantData;
            PlayerInventory.Instance.EquipImplant(currentImplant);

            PlayerInventory.Instance.RemoveImplant(currentImplant); // Удаляем из инвентаря

            implantUI.transform.SetParent(transform);
            implantUI.transform.localPosition = Vector3.zero;

            PlayerInventory.Instance.inventoryManager?.UpdateInventoryUI();
        }
    }

    public void SetImplant(ImplantUI implantUI)
    {
        currentImplant = implantUI.ImplantData;
        implantUI.transform.SetParent(transform);
        implantUI.transform.localPosition = Vector3.zero;

        PlayerInventory.Instance.EquipImplant(currentImplant); 
    }

    public void RemoveImplant()
    {
        if (currentImplant != null)
        {
            PlayerInventory.Instance.UnequipImplant(currentImplant);
            PlayerInventory.Instance.ReturnImplantToInventory(currentImplant);
            currentImplant = null;
        }
    }

    public Implant GetCurrentImplant()
    {
        return currentImplant;
    }

    

}
