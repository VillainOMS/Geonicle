using UnityEngine;
using UnityEngine.UI;

public class TutorialUI : MonoBehaviour
{
    public GameObject[] pages;
    public Button buttonNext;
    public Button buttonPrev;
    public Text nextButtonText;
    public Text prevButtonText;
    public GameObject panelTutorial;
    public GameObject panelMainMenu;

    private int currentPage = 0;

    private void OnEnable()
    {
        ShowPage(0);
    }

    public void ShowPage(int index)
    {
        currentPage = index;

        for (int i = 0; i < pages.Length; i++)
            pages[i].SetActive(i == currentPage);

        UpdateButtonLabels();
    }

    public void NextPage()
    {
        if (currentPage >= pages.Length - 1)
        {
            CloseTutorial();
        }
        else
        {
            ShowPage(currentPage + 1);
        }
    }

    public void PrevPage()
    {
        if (currentPage == 0)
        {
            CloseTutorial();
        }
        else
        {
            ShowPage(currentPage - 1);
        }
    }

    private void CloseTutorial()
    {
        panelTutorial.SetActive(false);
        panelMainMenu.SetActive(true);
    }

    private void UpdateButtonLabels()
    {
        if (currentPage == 0)
        {
            prevButtonText.text = "«¿ –€“‹";
            nextButtonText.text = "ƒ¿À≈≈";
        }
        else if (currentPage == pages.Length - 1)
        {
            prevButtonText.text = "Õ¿«¿ƒ";
            nextButtonText.text = "«¿ –€“‹";
        }
        else
        {
            prevButtonText.text = "Õ¿«¿ƒ";
            nextButtonText.text = "ƒ¿À≈≈";
        }
    }
}
