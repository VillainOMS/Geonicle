using UnityEngine;
using UnityEngine.UI;

public class AltarUI : MonoBehaviour
{
    [SerializeField] private GameObject altarUI;
    [SerializeField] private Button fireButton, metalButton, waterButton, shockButton;
    [SerializeField] private Image[] fireLevels, metalLevels, waterLevels, shockLevels;

    private int fireAspect = 0, metalAspect = 0, waterAspect = 0, shockAspect = 0;

    // Цвета аспектов
    private Color fireActive = new Color(1f, 0f, 0f, 1f);  // Ярко-красный
    private Color fireInactive = new Color(1f, 0.5f, 0.5f, 0.5f); // Тускло-красный

    private Color metalActive = new Color(0.75f, 0.75f, 0.75f, 1f);  // Яркий серый (металл)
    private Color metalInactive = new Color(0.5f, 0.5f, 0.5f, 0.5f); // Тускло-серый

    private Color waterActive = new Color(0f, 0f, 1f, 1f);  // Ярко-синий
    private Color waterInactive = new Color(0.5f, 0.5f, 1f, 0.5f); // Тускло-синий

    private Color shockActive = new Color(1f, 0f, 1f, 1f);  // Ярко-пурпурный
    private Color shockInactive = new Color(1f, 0.5f, 1f, 0.5f); // Тускло-пурпурный



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
    }

    public void UpgradeAspect(string aspect)
    {
        if (PlayerAspects.Instance.GetAspectPoints() > 0)
        {
            bool upgraded = false;

            switch (aspect)
            {
                case "Fire":
                    if (fireAspect < 5) { fireAspect++; upgraded = true; }
                    break;
                case "Metal":
                    if (metalAspect < 5) { metalAspect++; upgraded = true; }
                    break;
                case "Water":
                    if (waterAspect < 5) { waterAspect++; upgraded = true; }
                    break;
                case "Shock":
                    if (shockAspect < 5) { shockAspect++; upgraded = true; }
                    break;
            }

            if (upgraded)
            {
                PlayerAspects.Instance.SpendAspectPoint();
                UpdateUI();
            }
        }
    }

    private void SetInitialColors()
    {
        UpdateLevels(fireLevels, fireAspect, fireInactive, fireActive);
        UpdateLevels(metalLevels, metalAspect, metalInactive, metalActive);
        UpdateLevels(waterLevels, waterAspect, waterInactive, waterActive);
        UpdateLevels(shockLevels, shockAspect, shockInactive, shockActive);
    }

    private void UpdateUI()
    {
        PlayerStats.Instance.ApplyAspectBonuses(fireAspect, metalAspect, waterAspect, shockAspect);
        Debug.Log($"Применены бонусы аспектов: Fire({fireAspect}), Metal({metalAspect}), Water({waterAspect}), Shock({shockAspect})");

        UpdateLevels(fireLevels, fireAspect, fireInactive, fireActive);
        UpdateLevels(metalLevels, metalAspect, metalInactive, metalActive);
        UpdateLevels(waterLevels, waterAspect, waterInactive, waterActive);
        UpdateLevels(shockLevels, shockAspect, shockInactive, shockActive);
    }


    private void UpdateLevels(Image[] levelImages, int level, Color inactiveColor, Color activeColor)
    {
        for (int i = 0; i < levelImages.Length; i++)
        {
            levelImages[i].color = (i < level) ? activeColor : inactiveColor;
        }
    }
}
