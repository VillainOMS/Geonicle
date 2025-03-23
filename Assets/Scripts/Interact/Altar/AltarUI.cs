using UnityEngine;
using UnityEngine.UI;

public class AltarUI : MonoBehaviour
{
    [SerializeField] private GameObject altarUI;
    [SerializeField] private Button fireButton, metalButton, waterButton, shockButton;
    [SerializeField] private Image[] fireLevels, metalLevels, waterLevels, shockLevels;

    // Цвета аспектов
    private Color fireActive = new Color(1f, 0f, 0f, 1f);
    private Color fireInactive = new Color(1f, 0.5f, 0.5f, 0.5f);
    private Color metalActive = new Color(0.75f, 0.75f, 0.75f, 1f);
    private Color metalInactive = new Color(0.5f, 0.5f, 0.5f, 0.5f);
    private Color waterActive = new Color(0f, 0f, 1f, 1f);
    private Color waterInactive = new Color(0.5f, 0.5f, 1f, 0.5f);
    private Color shockActive = new Color(1f, 0f, 1f, 1f);
    private Color shockInactive = new Color(1f, 0.5f, 1f, 0.5f);

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            CloseMenu();
        }
    }

    private void Awake()
    {
        altarUI.SetActive(false);
    }

    private void Start()
    {
        SetInitialColors();
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

    private void SetInitialColors()
    {
        UpdateLevels(fireLevels, PlayerAspects.Instance.GetFireLevel(), fireInactive, fireActive);
        UpdateLevels(metalLevels, PlayerAspects.Instance.GetMetalLevel(), metalInactive, metalActive);
        UpdateLevels(waterLevels, PlayerAspects.Instance.GetWaterLevel(), waterInactive, waterActive);
        UpdateLevels(shockLevels, PlayerAspects.Instance.GetShockLevel(), shockInactive, shockActive);
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

        UpdateLevels(fireLevels, PlayerAspects.Instance.GetFireLevel(), fireInactive, fireActive);
        UpdateLevels(metalLevels, PlayerAspects.Instance.GetMetalLevel(), metalInactive, metalActive);
        UpdateLevels(waterLevels, PlayerAspects.Instance.GetWaterLevel(), waterInactive, waterActive);
        UpdateLevels(shockLevels, PlayerAspects.Instance.GetShockLevel(), shockInactive, shockActive);
    }

    private void UpdateLevels(Image[] levelImages, int level, Color inactiveColor, Color activeColor)
    {
        for (int i = 0; i < levelImages.Length; i++)
        {
            levelImages[i].color = (i < level) ? activeColor : inactiveColor;
        }
    }
}
