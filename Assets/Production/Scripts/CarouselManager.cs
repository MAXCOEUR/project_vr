using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
// Ajoute ceci si tu utilises AR Foundation
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class CarouselManager : MonoBehaviour
{
    [Header("UI References")]
    public Image displayImage;
    public Text displayText;

    [Header("AR Settings")]
    public ARRaycastManager raycastManager; // À glisser depuis l'AR Session Origin

    [System.Serializable]
    public struct ItemData
    {
        public string name;
        public Sprite icon;
        public GameObject modelPrefab;
    }

    public List<ItemData> items;
    private int currentIndex = 0;
    private GameObject currentModel;
    static List<ARRaycastHit> hits = new List<ARRaycastHit>();

    void Start()
    {
        UpdateUI();
    }

    public void Next()
    {
        if (items.Count == 0) return;
        currentIndex = (currentIndex + 1) % items.Count;
        UpdateUI();
    }

    public void Previous()
    {
        if (items.Count == 0) return;
        currentIndex--;
        if (currentIndex < 0) currentIndex = items.Count - 1;
        UpdateUI();
    }

    // FONCTION POUR SPAWN EN AR
    public void SpawnInAR()
    {
        // 1. On définit le centre de l'écran
        Vector2 screenCenter = new Vector2(Screen.width / 2, Screen.height / 2);

        // 2. On tire un Raycast AR pour trouver un plan (le sol/table)
        if (raycastManager.Raycast(screenCenter, hits, TrackableType.PlaneWithinPolygon))
        {
            Pose hitPose = hits[0].pose;

            currentModel = Instantiate(items[currentIndex].modelPrefab, hitPose.position, hitPose.rotation);
        }
    }

    void UpdateUI()
    {
        if (items.Count > 0 && currentIndex < items.Count)
        {
            if (displayImage != null) displayImage.sprite = items[currentIndex].icon;
            if (displayText != null) displayText.text = items[currentIndex].name;
        }
    }
}