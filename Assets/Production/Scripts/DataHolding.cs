using UnityEngine;
using System.Collections.Generic;
using System;

public class DataHolding : MonoBehaviour
{
    public static DataHolding Instance;

    public bool housePlaced = false;

    [System.Serializable]
    public struct UpgradeCost
    {
        public int woodRequired;
        public int rockRequired;
    }

    [Header("Configuration des Prix")] public List<UpgradeCost> upgradeCosts = new List<UpgradeCost>();

    [Header("Stock actuel")] public int woodCount = 0;
    public int rockCount = 0;
    public int humanCount = 0;

    [Header("Progression")] public int houseCurrentLevel = 0;

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
        else if (type == "Human")
        {
            humanCount += amount;
            Debug.Log("Humain ajouté ! Total : " + humanCount);
        }

        OnResourcesChanged?.Invoke();
    }
    public bool deadHuman()
    {
        if (humanCount > 0)
        {
            humanCount--;
            OnResourcesChanged?.Invoke();
            return true;
        }
        return false;
    }

    public bool TrySpendResources()
    {
        if (houseCurrentLevel >= upgradeCosts.Count)
        {
            Debug.LogError("Niveau max atteint ou pas de prix défini pour le niveau " + houseCurrentLevel);
            return false;
        }

        UpgradeCost cost = upgradeCosts[houseCurrentLevel];

        Debug.Log($"houseCurrentLevel {houseCurrentLevel}");
        if (woodCount >= cost.woodRequired && rockCount >= cost.rockRequired)
        {
            woodCount -= cost.woodRequired;
            rockCount -= cost.rockRequired;

            houseCurrentLevel++;

            Debug.Log($"Amélioration Niveau {houseCurrentLevel - 1} -> {houseCurrentLevel} réussie !");
            OnResourcesChanged?.Invoke();
            OnUpdateHouse?.Invoke();
            return true;
        }

        Debug.Log($"Manque de ressources. Besoin de {cost.woodRequired} bois et {cost.rockRequired} roches.");
        return false;
    }

    public bool TrySpendResourcesHuman()
    {
 


        if (woodCount >= 1)
        {            
            woodCount -= 1;

            OnResourcesChanged?.Invoke();
            return true;
        }

        
        return false;
    }
}