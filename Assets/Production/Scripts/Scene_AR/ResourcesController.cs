using UnityEngine;
using TMPro;

public class ResourcesController : MonoBehaviour
{
    [Header("Textes UI")]
    public TextMeshProUGUI woodText;
    public TextMeshProUGUI rockText;
    public TextMeshProUGUI houseLevelText;

    void OnEnable()
    {
        DataHolding.OnResourcesChanged += UpdateUI;
        DataHolding.OnUpdateHouse += UpdateUI;
    }

    void OnDisable()
    {
        DataHolding.OnResourcesChanged -= UpdateUI;
        DataHolding.OnUpdateHouse -= UpdateUI;
    }

    void Start()
    {
        UpdateUI();
    }

    void UpdateUI()
    {
        if (DataHolding.Instance == null) return;

        woodText.text = DataHolding.Instance.woodCount.ToString();
        rockText.text = DataHolding.Instance.rockCount.ToString();
        houseLevelText.text = "Niv. " + DataHolding.Instance.houseCurrentLevel.ToString();
    }
}