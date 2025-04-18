using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AltarUI : MonoBehaviour
{
    [SerializeField] private GameObject altarUI;
    [SerializeField] private Button fireButton, metalButton, waterButton, shockButton;

    [SerializeField] private Image[] fireLevels, metalLevels, waterLevels, shockLevels;

    // Спрайты аспектов
    [SerializeField] private Sprite fireIcon;
    [SerializeField] private Sprite metalIcon;
    [SerializeField] private Sprite waterIcon;
    [SerializeField] private Sprite shockIcon;

    public static AltarUI Instance { get; private set; }
    [SerializeField] private Text tooltipText;


    // Цвета затемнения
    private Color activeColor = Color.white;
    private Color inactiveColor = new Color(1f, 1f, 1f, 0.3f); // просто прозрачный белый

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            CloseMenu();
        }
    }

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
        altarUI.SetActive(false);
    }

    private void Start()
    {
        SetInitialIcons();
    }

    public void SetTooltip(string text)
    {
        tooltipText.text = text;
    }

    public void ClearTooltip()
    {
        tooltipText.text = "";
    }

    public void OpenAltarMenu()
    {
        altarUI.SetActive(true);
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        Time.timeScale = 0f;
        UpdateUI();
    }

    public void CloseMenu()
    {
        altarUI.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        Time.timeScale = 1f;

        GameState.IsUIOpen = false;
    }

    public void UpgradeAspect(string aspect)
    {
        if (PlayerAspects.Instance.GetAspectPoints() > 0)
        {
            PlayerAspects.Instance.UpgradeAspect(aspect);
            UpdateUI();
        }
    }

    private void SetInitialIcons()
    {
        UpdateLevels(fireLevels, PlayerAspects.Instance.GetFireLevel(), fireIcon);
        UpdateLevels(metalLevels, PlayerAspects.Instance.GetMetalLevel(), metalIcon);
        UpdateLevels(waterLevels, PlayerAspects.Instance.GetWaterLevel(), waterIcon);
        UpdateLevels(shockLevels, PlayerAspects.Instance.GetShockLevel(), shockIcon);
    }

    private void UpdateUI()
    {
        PlayerStats.Instance.ApplyAspectBonuses(
            PlayerAspects.Instance.GetFireLevel(),
            PlayerAspects.Instance.GetMetalLevel(),
            PlayerAspects.Instance.GetWaterLevel(),
            PlayerAspects.Instance.GetShockLevel()
        );

        Debug.Log($"Применены бонусы аспектов: Fire({PlayerAspects.Instance.GetFireLevel()}), " +
                  $"Metal({PlayerAspects.Instance.GetMetalLevel()}), " +
                  $"Water({PlayerAspects.Instance.GetWaterLevel()}), " +
                  $"Shock({PlayerAspects.Instance.GetShockLevel()})");

        UpdateLevels(fireLevels, PlayerAspects.Instance.GetFireLevel(), fireIcon);
        UpdateLevels(metalLevels, PlayerAspects.Instance.GetMetalLevel(), metalIcon);
        UpdateLevels(waterLevels, PlayerAspects.Instance.GetWaterLevel(), waterIcon);
        UpdateLevels(shockLevels, PlayerAspects.Instance.GetShockLevel(), shockIcon);
    }

    private void UpdateLevels(Image[] levelImages, int level, Sprite icon)
    {
        for (int i = 0; i < levelImages.Length; i++)
        {
            levelImages[i].sprite = icon;
            levelImages[i].color = (i < level) ? activeColor : inactiveColor;
            levelImages[i].preserveAspect = true;
        }
    }
}
