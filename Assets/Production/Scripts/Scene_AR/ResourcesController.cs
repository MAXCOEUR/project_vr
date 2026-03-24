using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections.Generic;

public class ResourcesController : MonoBehaviour
{
    [Header("Textes UI")]
    public TextMeshProUGUI woodText;
    public TextMeshProUGUI rockText;
    public TextMeshProUGUI houseLevelText;
    [Header("New UI Reference")]
    public Image imageHouse; // L'emplacement de l'image dans ton UI
    public List<Sprite> levelIconsHouse;

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
        imageHouse.sprite = levelIconsHouse[DataHolding.Instance.houseCurrentLevel];
        houseLevelText.text = "Niv. " + DataHolding.Instance.houseCurrentLevel.ToString();
    }
}