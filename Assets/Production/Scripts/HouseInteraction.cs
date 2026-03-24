using Unity.Android.Gradle.Manifest;
using UnityEngine;

public class HouseInteraction : MonoBehaviour
{
    RootModelPrefab root;

    void Start()
    {
        root = GetComponent<RootModelPrefab>();
    }
    public void HandleClick()
    {
        Debug.Log("GameManager: " + GameManager.Instance);
        Debug.Log("DataHolding: " + DataHolding.Instance);
        
        var data = DataHolding.Instance;

        int level = data.houseCurrentLevel;

        Debug.Log($"HouseInteraction: Click détecté sur la maison. Niveau actuel : {level} < {data.upgradeCosts.Count} ?");
        if (level < data.upgradeCosts.Count)
        {
            var cost = data.upgradeCosts[level];

            bool canUpgrade = data.woodCount >= cost.woodRequired &&
                            data.rockCount >= cost.rockRequired;

            if (canUpgrade)
            {
                Debug.Log($"Tentative d'amélioration du niveau {level} -> {level + 1}...");
                data.TrySpendResources();

                GetComponent<RootModelPrefab>().RefreshVisual();
                return;
            }
        }
        if(data.TrySpendResourcesHuman())
        {
            Vector3 pos = transform.position + Random.insideUnitSphere;
            pos.y = transform.position.y;

            GameManager.Instance.SpawnHuman(pos, transform);
        }
    }
}
