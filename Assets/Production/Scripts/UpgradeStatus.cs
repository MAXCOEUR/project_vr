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
        // On s'abonne à l'événement
        DataHolding.OnResourcesChanged += Refresh;
    }

    void OnDisable()
    {
        // On se désabonne (CRITIQUE pour éviter les erreurs)
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

    // À appeler chaque fois que le joueur gagne une ressource
    public void Refresh()
    {
        int currentWood = DataHolding.Instance.woodCount;
        int currentRock = DataHolding.Instance.rockCount;

        bool canAffordWood = currentWood >= woodNeeded;
        bool canAffordRock = currentRock >= rockNeeded;
        bool canUpgrade = canAffordWood && canAffordRock;

        // Mise à jour de l'icône de gauche
        statusImage.sprite = canUpgrade ? arrowGreen : crossRed;

        // Mise à jour des couleurs des textes
        woodText.color = canAffordWood ? colorReady : colorMissing;
        rockText.color = canAffordRock ? colorReady : colorMissing;
    }

    void Update()
    {
        // Optionnel : Le panneau regarde toujours le joueur
        if (Camera.main != null)
            transform.LookAt(transform.position + Camera.main.transform.rotation * Vector3.forward,
                             Camera.main.transform.rotation * Vector3.up);
    }
}