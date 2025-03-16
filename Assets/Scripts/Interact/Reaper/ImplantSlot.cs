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
            PlayerStats.Instance.ApplyImplantEffect(currentImplant);

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

        PlayerStats.Instance.ApplyImplantEffect(currentImplant);
        PlayerInventory.Instance.EquipImplant(currentImplant); // Важно: добавляем сюда!
    }

    public void RemoveImplant()
    {
        if (currentImplant != null)
        {
            PlayerStats.Instance.RemoveImplantEffect(currentImplant);
            PlayerInventory.Instance.UnequipImplant(currentImplant); // Убираем из экипированных
            PlayerInventory.Instance.ReturnImplantToInventory(currentImplant);
            currentImplant = null;
        }
    }

    public Implant GetCurrentImplant()
    {
        return currentImplant;
    }

    

}
