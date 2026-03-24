using System.Collections.Generic;
using UnityEngine;

public class RootModelPrefab : MonoBehaviour
{
    // Liste remplie par CarouselManager
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
        // 🔧 Ajouter collider si absent
        Collider col = level.GetComponent<Collider>();
        if (col == null)
        {
            col = level.AddComponent<BoxCollider>();
        }

        // 🔧 Ajuster collider (optionnel mais utile)
        col.isTrigger = false;

        // 🔧 Ajouter relay de clic
        HouseClickRelay relay = level.GetComponent<HouseClickRelay>();
        if (relay == null)
        {
            relay = level.AddComponent<HouseClickRelay>();
        }
    }
}