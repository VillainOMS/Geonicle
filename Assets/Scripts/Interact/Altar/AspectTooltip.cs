using UnityEngine;
using UnityEngine.EventSystems;

public class AspectTooltip : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private string tooltipText;

    public void OnPointerEnter(PointerEventData eventData)
    {
        AltarUI.Instance.SetTooltip(tooltipText);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        AltarUI.Instance.ClearTooltip();
    }
}
