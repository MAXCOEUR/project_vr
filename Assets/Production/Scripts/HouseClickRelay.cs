using UnityEngine;

public class HouseClickRelay : MonoBehaviour
{
    private HouseInteraction house;

    void Start()
    {
        house = GetComponentInParent<HouseInteraction>();
    }

    void OnMouseDown()
    {
        if (house != null)
        {
            house.HandleClick();
        }
    }
}