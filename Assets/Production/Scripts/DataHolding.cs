using UnityEngine;
using System.Collections.Generic;
using System;

public class DataHolding : MonoBehaviour
{
    public static DataHolding Instance;

    [System.Serializable]
    public struct UpgradeCost
    {
        public int woodRequired;
        public int rockRequired;
    }

    [Header("Configuration des Prix")]
    public List<UpgradeCost> upgradeCosts = new List<UpgradeCost>();

    [Header("Stock actuel")]
    public int woodCount = 0;
    public int rockCount = 0;
    
    [Header("Progression")]
    public int houseCurrentLevel = 0;

    public static event Action OnResourcesChanged;
    public static event Action OnUpdateHouse;

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
        OnResourcesChanged?.Invoke();
    }

    public bool TrySpendResources(int currentLevel)
    {
        if (currentLevel != houseCurrentLevel)
        {
            Debug.LogWarning($"Tentative d'amélioration du niveau {currentLevel} alors que le niveau actuel est {houseCurrentLevel}. Annulation.");
            return false;
        }

        if (currentLevel >= upgradeCosts.Count)
        {
            Debug.LogError("Niveau max atteint ou pas de prix défini pour le niveau " + currentLevel);
            return false;
        }

        UpgradeCost cost = upgradeCosts[currentLevel];

        if (woodCount >= cost.woodRequired && rockCount >= cost.rockRequired)
        {
            woodCount -= cost.woodRequired;
            rockCount -= cost.rockRequired;
            
            houseCurrentLevel++;

            Debug.Log($"Amélioration Niveau {currentLevel} -> {houseCurrentLevel} réussie !");
            OnResourcesChanged?.Invoke();
            OnUpdateHouse?.Invoke();
            return true;
        }

        Debug.Log($"Manque de ressources. Besoin de {cost.woodRequired} bois et {cost.rockRequired} roches.");
        return false;
    }
}