using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UpgradeStatus : MonoBehaviour
{
    [Header("Icônes d'état")]
    public Image statusImage;      // L'image à gauche
    public Sprite crossRed;        // Sprite "X" rouge
    public Sprite arrowGreen;      // Sprite "Flèche" haut verte

    [Header("Textes des prix")]
    public TMP_Text woodText;
    public TMP_Text rockText;

    [Header("Couleurs")]
    public Color colorReady = Color.green;
    public Color colorMissing = Color.red;

    private int woodNeeded;
    private int rockNeeded;

    void OnEnable()
    {
        DataHolding.OnResourcesChanged += Refresh;
    }

    void OnDisable()
    {
        DataHolding.OnResourcesChanged -= Refresh;
    }
    public void Start()
    {
        ARIdentity identity = transform.GetComponentInParent<ARIdentity>();

        var data = DataHolding.Instance.upgradeCosts[identity.level];

        woodNeeded = data.woodRequired;
        rockNeeded = data.rockRequired;
        woodText.text = woodNeeded.ToString();
        rockText.text = rockNeeded.ToString();
        Refresh();
    }

    public void Refresh()
    {
        int currentWood = DataHolding.Instance.woodCount;
        int currentRock = DataHolding.Instance.rockCount;

        bool canAffordWood = currentWood >= woodNeeded;
        bool canAffordRock = currentRock >= rockNeeded;
        bool canUpgrade = canAffordWood && canAffordRock;

        statusImage.sprite = canUpgrade ? arrowGreen : crossRed;

        woodText.color = canAffordWood ? colorReady : colorMissing;
        rockText.color = canAffordRock ? colorReady : colorMissing;
    }

    void Update()
    {
        if (Camera.main != null)
            transform.LookAt(transform.position + Camera.main.transform.rotation * Vector3.forward,
                             Camera.main.transform.rotation * Vector3.up);
    }
}