using System.Collections.Generic;
using UnityEngine;

public class RootModelPrefab : MonoBehaviour
{
    public List<GameObject> levels;

    void OnEnable()
    {
        DataHolding.OnUpdateHouse += RefreshVisual;
    }

    void OnDisable()
    {
        DataHolding.OnUpdateHouse -= RefreshVisual;
    }

    public void RefreshVisual()
    {
        if (levels == null || levels.Count == 0) return;

        int currentLv = DataHolding.Instance.houseCurrentLevel;

        for (int i = 0; i < levels.Count; i++)
        {
            bool isActive = (i == currentLv);

            levels[i].SetActive(isActive);

            if (isActive)
            {
                SetupLevel(levels[i]);
            }
        }
    }

    void SetupLevel(GameObject level)
    {
        Collider col = level.GetComponent<Collider>();
        if (col == null)
        {
            col = level.AddComponent<BoxCollider>();
        }

        col.isTrigger = false;

        HouseClickRelay relay = level.GetComponent<HouseClickRelay>();
        if (relay == null)
        {
            relay = level.AddComponent<HouseClickRelay>();
        }
    }
}