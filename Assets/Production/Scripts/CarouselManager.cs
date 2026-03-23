using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class CarouselManager : MonoBehaviour
{
    [Header("UI References")]
    public Image displayImage;
    public Text displayText;
    public Toggle previewToggle; // Optionnel : pour lier à un Toggle UI

    [Header("AR Settings")]
    public ARRaycastManager raycastManager;

    [System.Serializable]
    public struct ItemData
    {
        public string name;
        public Sprite icon;
        public GameObject modelPrefab;
    }

    public List<ItemData> items;
    private int currentIndex = 0;
    
    private GameObject previewModel; 
    private bool isPreviewActive = true; // Contrôle si on affiche l'ombre ou pas
    static List<ARRaycastHit> hits = new List<ARRaycastHit>();

    void Start() { UpdateUI(); }

    void Update()
    {
        // On ne gère l'aperçu que si l'option est activée
        if (isPreviewActive) { UpdatePreviewPosition(); }
        else if (previewModel != null) { previewModel.SetActive(false); }
    }

    // --- FONCTION POUR TON BOUTON ON/OFF ---
    public void TogglePreview()
    {
        isPreviewActive = !isPreviewActive;
        if (!isPreviewActive && previewModel != null) 
            previewModel.SetActive(false);
    }

    void UpdatePreviewPosition()
    {
        if (raycastManager == null || items.Count == 0) return;

        Vector2 screenCenter = new Vector2(Screen.width / 2, Screen.height / 2);

        if (raycastManager.Raycast(screenCenter, hits, TrackableType.PlaneWithinPolygon))
        {
            Pose hitPose = hits[0].pose;

            if (previewModel == null)
            {
                previewModel = Instantiate(items[currentIndex].modelPrefab, hitPose.position, hitPose.rotation);
                ApplyShadowEffect(previewModel, 0.2f); // 40% d'opacité noire
            }

            previewModel.transform.position = hitPose.position;
            previewModel.transform.rotation = hitPose.rotation;
            previewModel.SetActive(true);
        }
        else
        {
            if (previewModel != null) previewModel.SetActive(false);
        }
    }

    public void SpawnInAR()
    {
        // On ne spawn que si le preview est visible (donc qu'on a trouvé un sol)
        if (previewModel != null && previewModel.activeSelf)
        {
            // On instancie le vrai objet (il sera créé avec ses couleurs d'origine)
            Instantiate(items[currentIndex].modelPrefab, previewModel.transform.position, previewModel.transform.rotation);
        }
    }

    public void Next() { ChangeItem(1); }
    public void Previous() { ChangeItem(-1); }

    void ChangeItem(int step)
    {
        if (items.Count == 0) return;
        currentIndex = (currentIndex + step + items.Count) % items.Count;
        
        // Si on change d'item, on détruit l'ancien fantôme pour que le nouveau se crée
        if (previewModel != null) Destroy(previewModel);
        
        UpdateUI();
    }

    // --- LA LOGIQUE "OMBRE NOIRE" ---
    void ApplyShadowEffect(GameObject obj, float alpha)
    {
        Renderer[] renderers = obj.GetComponentsInChildren<Renderer>();
        foreach (Renderer r in renderers)
        {
            foreach (Material m in r.materials)
            {
                // Forcer le mode Transparent
                m.SetFloat("_Surface", 1); 
                m.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
                m.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
                m.SetInt("_ZWrite", 0); 
                m.renderQueue = (int)UnityEngine.Rendering.RenderQueue.Transparent;
                m.EnableKeyword("_ALPHABLEND_ON");

                // Trouver la propriété de couleur selon le shader (glTF ou Standard)
                string prop = m.HasProperty("baseColorFactor") ? "baseColorFactor" : 
                             (m.HasProperty("_BaseColor") ? "_BaseColor" : "_Color");

                // Appliquer Noir + Alpha
                m.SetColor(prop, new Color(0, 0, 0, alpha));
            }
        }
    }

    void UpdateUI()
    {
        if (items.Count > 0)
        {
            if(displayImage != null) displayImage.sprite = items[currentIndex].icon;
            if(displayText != null) displayText.text = items[currentIndex].name;
        }
    }
}