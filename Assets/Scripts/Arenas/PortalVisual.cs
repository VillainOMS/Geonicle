using UnityEngine;

public class PortalVisual : MonoBehaviour
{
    [Header("Визуальные элементы")]
    [SerializeField] private Renderer statusSignRenderer;
    [SerializeField] private Renderer portalFillRenderer;

    [Header("Материалы статуса")]
    [SerializeField] private Material openMaterial;
    [SerializeField] private Material closedMaterial;

    [Header("Анимация шума")]
    [SerializeField] private float scrollSpeedX = 0.1f;
    [SerializeField] private float scrollSpeedY = 0.1f;

    private Material portalFillMatInstance;

    private void Start()
    {
        if (portalFillRenderer != null)
        {
            portalFillMatInstance = Instantiate(portalFillRenderer.material);
            portalFillRenderer.material = portalFillMatInstance;
        }
    }

    private void Update()
    {
        if (portalFillMatInstance != null)
        {
            float offsetX = Time.time * scrollSpeedX;
            float offsetY = Time.time * scrollSpeedY;
            portalFillMatInstance.mainTextureOffset = new Vector2(offsetX, offsetY);
        }
    }

    public void SetPortalOpen(bool isOpen)
    {
        Debug.Log($"[PortalVisual] SetPortalOpen: {(isOpen ? "ОТКРЫТ" : "ЗАКРЫТ")}");

        if (statusSignRenderer != null)
        {
            var block = new MaterialPropertyBlock();
            statusSignRenderer.GetPropertyBlock(block);
            block.SetColor("_Color", (isOpen ? openMaterial.color : closedMaterial.color));
            statusSignRenderer.SetPropertyBlock(block);
        }

        if (portalFillRenderer != null)
        {
            portalFillRenderer.gameObject.SetActive(isOpen);
            Debug.Log($"[PortalVisual] portalFillRenderer gameObject setActive({isOpen})");
        }
    }

}
