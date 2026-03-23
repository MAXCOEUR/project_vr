using UnityEngine;
using System.Collections.Generic;

public class DataHolding : MonoBehaviour
{
    public static DataHolding Instance;

    // On crée une petite structure pour définir un "Prix"
    [System.Serializable]
    public struct UpgradeCost
    {
        public int woodRequired;
        public int rockRequired;
    }

    [Header("Configuration des Prix")]
    // Cette liste apparaîtra dans l'inspecteur : Element 0 = Niveau 0 vers 1, etc.
    public List<UpgradeCost> upgradeCosts = new List<UpgradeCost>();

    [Header("Stock actuel")]
    public int woodCount = 0;
    public int rockCount = 0;

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    public void AddResource(string type, int amount)
    {
        if (type == "Arbre")
        {
            woodCount += amount;
            Debug.Log("Bois ajouté ! Total : " + woodCount);
        }
        else if (type == "Rocher")
        {
            rockCount += amount;
            Debug.Log("Roche ajoutée ! Total : " + rockCount);
        }
    }

    public bool TrySpendResources(int currentLevel)
    {
        // Sécurité : on vérifie si le niveau existe dans notre liste de prix
        if (currentLevel >= upgradeCosts.Count)
        {
            Debug.LogError("Pas de prix défini pour le niveau " + currentLevel);
            return false;
        }

        UpgradeCost cost = upgradeCosts[currentLevel];

        if (woodCount >= cost.woodRequired && rockCount >= cost.rockRequired)
        {
            woodCount -= cost.woodRequired;
            rockCount -= cost.rockRequired;
            Debug.Log($"Amélioration Niveau {currentLevel} -> {currentLevel + 1} réussie !");
            return true;
        }

        Debug.Log($"Manque de ressources. Besoin de {cost.woodRequired} bois et {cost.rockRequired} roches.");
        return false;
    }
}