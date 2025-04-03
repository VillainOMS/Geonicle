using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Collections.Generic;

public class ImplantUI : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerEnterHandler, IPointerExitHandler
{
    public Implant ImplantData { get; private set; }
    [SerializeField] private Image icon;
    private Transform parentAfterDrag;
    private InventoryManager inventoryManager;

    public void Setup(Implant implant, InventoryManager manager)
    {
        ImplantData = implant;
        icon.sprite = implant.Icon;
        inventoryManager = manager;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        parentAfterDrag = transform.parent;
        transform.SetParent(transform.root);
        transform.SetAsLastSibling();
    }

    public void OnDrag(PointerEventData eventData)
    {
        transform.position = Input.mousePosition;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        ImplantSlot slot = GetSlotUnderPointer(eventData);
        ImplantSlot currentSlot = parentAfterDrag.GetComponent<ImplantSlot>();

        if (slot != null && slot.slotType == ImplantData.Slot)
        {
            if (slot.GetCurrentImplant() != null)
            {
                slot.RemoveImplant(); // вернёт текущий имплант слота в инвентарь
            }

            slot.SetImplant(this);
            PlayerInventory.Instance.RemoveImplant(ImplantData);
            transform.SetParent(slot.transform);
            transform.localPosition = Vector3.zero;
            inventoryManager.UpdateInventoryUI();
        }
        else if (GetInventoryUnderPointer(eventData))
        {
            if (currentSlot != null)
            {
                currentSlot.RemoveImplant(); // Корректно вернёт имплант обратно
                currentSlot = null;
            }

            PlayerInventory.Instance.AddNewImplant(ImplantData);
            transform.SetParent(inventoryManager.inventoryPanel);
            transform.localPosition = Vector3.zero;
            inventoryManager.UpdateInventoryUI();
        }
        else
        {
            // Если отпущен в пустом месте, вернуть обратно
            transform.SetParent(parentAfterDrag);
            transform.localPosition = Vector3.zero;
        }
    }


    private ImplantSlot GetSlotUnderPointer(PointerEventData eventData)
    {
        var results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventData, results);

        foreach (var raycast in results)
        {
            ImplantSlot slot = raycast.gameObject.GetComponent<ImplantSlot>();
            if (slot != null) return slot;
        }
        return null;
    }

    private bool GetInventoryUnderPointer(PointerEventData eventData)
    {
        var results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventData, results);

        foreach (var raycast in results)
        {
            if (raycast.gameObject == inventoryManager.inventoryPanel.gameObject ||
                raycast.gameObject.transform.IsChildOf(inventoryManager.inventoryPanel))
                return true;
        }
        return false;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        inventoryManager.ShowImplantInfo(ImplantData);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        inventoryManager.HideImplantInfo();
    }

}
