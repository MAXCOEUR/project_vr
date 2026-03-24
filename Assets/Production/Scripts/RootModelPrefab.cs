using System.Collections.Generic;
using UnityEngine;

public class RootModelPrefab : MonoBehaviour
{
    // Cette liste sera remplie par le CarouselManager au moment du Spawn
    public List<GameObject> levels; 

    void OnEnable() { DataHolding.OnUpdateHouse += RefreshVisual; }
    void OnDisable() { DataHolding.OnUpdateHouse -= RefreshVisual; }

    public void RefreshVisual()
    {
        if (levels == null || levels.Count == 0) return;

        int currentLv = DataHolding.Instance.houseCurrentLevel;

        for(int i = 0; i < levels.Count; i++)
        {
            levels[i].SetActive(i == currentLv);
        }
    }
}
