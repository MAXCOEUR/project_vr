using UnityEngine;

public class HouseInteraction : MonoBehaviour
{
    RootModelPrefab root;

    void Start()
    {
        root = GetComponent<RootModelPrefab>();
    }

    void OnMouseDown()
    {
        var data = DataHolding.Instance;

        int level = data.houseCurrentLevel;

        bool upgraded = data.TrySpendResources(level);

        if (upgraded)
        {
            Debug.Log(" AHHHHHHHH");
            data.houseCurrentLevel++;

            if (root != null)
                root.RefreshVisual();
        }
        else
        {
             Debug.Log(" IHHHHHHH");
            if (data.woodCount > 0)
            {
                data.woodCount--;

                Vector3 pos = transform.position + Random.insideUnitSphere;
                pos.y = transform.position.y;

                GameManager.Instance.SpawnHuman(pos, transform);
            }
        }
    }
    public void HandleClick()
    {
    Debug.Log("GameManager: " + GameManager.Instance);
    Debug.Log("DataHolding: " + DataHolding.Instance);
    
    var data = DataHolding.Instance;

    int level = data.houseCurrentLevel;

    if (level < data.upgradeCosts.Count)
    {
        var cost = data.upgradeCosts[level];

        bool canUpgrade = data.woodCount >= cost.woodRequired &&
                          data.rockCount >= cost.rockRequired;

        if (canUpgrade)
        {
            data.TrySpendResources(level);
            data.houseCurrentLevel++;

            GetComponent<RootModelPrefab>().RefreshVisual();
        }
        else
        {
                data.TrySpendResourcesHuman();
                Vector3 pos = transform.position + Random.insideUnitSphere;
                pos.y = transform.position.y;

                GameManager.Instance.SpawnHuman(pos, transform);
            
        }
    }
}
}