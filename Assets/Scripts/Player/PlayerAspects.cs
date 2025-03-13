using UnityEngine;
using UnityEngine.UI;

public class PlayerAspects : MonoBehaviour
{
    public static PlayerAspects Instance { get; private set; }

    [SerializeField] private int aspectPoints = 0; // ���������� ����� ��������

    [SerializeField] private Text aspectPointsText; // ����� ����� ��������
    [SerializeField] private GameObject aspectPointsUI; // UI-������� � ������ ��������

    // ��������� ������� �������� (������������� � Inspector)
    [SerializeField] private float fireImpact = 0.25f;
    [SerializeField] private float metalImpact = 0.25f;
    [SerializeField] private float waterImpact = 0.25f;
    [SerializeField] private float shockImpact = 0.25f;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        UpdateUI();
    }

    public void AddAspectPoint()
    {
        aspectPoints++;
        Debug.Log($"��������� ���� ��������. �����: {aspectPoints}");
        UpdateUI();
    }

    private void UpdateUI()
    {
        Debug.Log($"���������� UI. ���� ��������: {aspectPoints}");

        if (aspectPoints > 0)
        {
            aspectPointsUI.SetActive(true);
            aspectPointsText.text = aspectPoints.ToString();
        }
        else
        {
            aspectPointsUI.SetActive(false);
        }
    }

    public int GetAspectPoints()
    {
        return aspectPoints;
    }

    public bool SpendAspectPoint()
    {
        if (aspectPoints > 0)
        {
            aspectPoints--;
            Debug.Log($"��������� ���� ��������. ��������: {aspectPoints}");
            UpdateUI();
            return true;
        }
        Debug.Log("��� ��������� ����� ��������!");
        return false;
    }

    // ������ ��� ��������� ������� ��������
    public float GetFireImpact() => fireImpact;
    public float GetMetalImpact() => metalImpact;
    public float GetWaterImpact() => waterImpact;
    public float GetShockImpact() => shockImpact;
}
