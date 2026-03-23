using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using TMPro;

public class CarouselManager : MonoBehaviour
{
    [System.Serializable]
    public class CategoryData
    {
        [HideInInspector] public string categoryName;
        public List<Sprite> levelIcons; 
        public List<GameObject> levels;  
        [HideInInspector] public int currentLevelIndex = 0; 
    }

    [Header("Progression Data")]
    public CategoryData houseCategory;
    public CategoryData treeCategory;
    public CategoryData rockCategory;

    // --- NOUVEAU : GESTION DE LA NAVIGATION ---
    private List<CategoryData> allCategories = new List<CategoryData>();
    private int currentCategoryIndex = 0;
    private CategoryData activeCategory;

    [Header("UI References")]
    public Image displayImage;      
    public TMP_Text categoryTitleText;  
    public TMP_Text levelText;          

    [Header("AR Settings")]
    public ARRaycastManager raycastManager;
    
    private GameObject previewModel; 
    private bool isPreviewActive = true;
    static List<ARRaycastHit> hits = new List<ARRaycastHit>();

    void OnValidate()
    {
        houseCategory.categoryName = "Maison";
        treeCategory.categoryName = "Arbre";
        rockCategory.categoryName = "Rocher";
    }

    void Start() 
    { 
        // Initialisation de la liste des catégories pour la navigation
        allCategories.Add(houseCategory);
        allCategories.Add(treeCategory);
        allCategories.Add(rockCategory);

        LoadUserLevelsFromDB();
        
        // On définit la catégorie de départ (index 0 = Maison)
        UpdateActiveCategory();
    }

    // --- NOUVELLES FONCTIONS DE NAVIGATION ---

    public void NextCategory()
    {
        currentCategoryIndex = (currentCategoryIndex + 1) % allCategories.Count;
        UpdateActiveCategory();
    }

    public void PreviousCategory()
    {
        currentCategoryIndex--;
        if (currentCategoryIndex < 0) currentCategoryIndex = allCategories.Count - 1;
        UpdateActiveCategory();
    }

    private void UpdateActiveCategory()
    {
        activeCategory = allCategories[currentCategoryIndex];
        RefreshPreview();
        UpdateUI();
    }

    // Gardé au cas où tu as besoin de cliquer sur un bouton spécifique
    public void SelectCategory(string name)
    {
        if (name == "House") currentCategoryIndex = 0;
        else if (name == "Tree") currentCategoryIndex = 1;
        else if (name == "Rock") currentCategoryIndex = 2;

        UpdateActiveCategory();
    }

    // ------------------------------------------

    void LoadUserLevelsFromDB()
    {
        houseCategory.currentLevelIndex = 0; 
        treeCategory.currentLevelIndex = 0;
        rockCategory.currentLevelIndex = 0;
    }

    void Update()
    {
        if (isPreviewActive) UpdatePreviewPosition();
        else if (previewModel != null) previewModel.SetActive(false);
    }

    void UpdatePreviewPosition()
    {
        if (raycastManager == null || activeCategory == null || activeCategory.levels.Count == 0) return;

        Vector2 screenCenter = new Vector2(Screen.width / 2, Screen.height / 2);

        if (raycastManager.Raycast(screenCenter, hits, TrackableType.PlaneWithinPolygon))
        {
            Pose hitPose = hits[0].pose;

            if (previewModel == null)
            {
                GameObject prefab = activeCategory.levels[activeCategory.currentLevelIndex];
                previewModel = Instantiate(prefab, hitPose.position, hitPose.rotation);
                ApplyShadowEffect(previewModel, 0.2f);
            }

            previewModel.transform.position = hitPose.position;
            previewModel.transform.rotation = hitPose.rotation;
            previewModel.SetActive(true);
        }
        else if (previewModel != null) previewModel.SetActive(false);
    }

    public void SpawnInAR()
    {
        if (previewModel != null && previewModel.activeSelf)
        {
            GameObject prefab = activeCategory.levels[activeCategory.currentLevelIndex];
            Instantiate(prefab, previewModel.transform.position, previewModel.transform.rotation);
        }
    }

    void RefreshPreview() { if (previewModel != null) Destroy(previewModel); }

    void UpdateUI()
    {
        if (activeCategory != null)
        {
            if (displayImage != null && activeCategory.levelIcons.Count > activeCategory.currentLevelIndex)
                displayImage.sprite = activeCategory.levelIcons[activeCategory.currentLevelIndex];

            if (categoryTitleText != null)
                categoryTitleText.text = activeCategory.categoryName;

            if (levelText != null)
                levelText.text = "Niveau " + (activeCategory.currentLevelIndex + 1);
        }
    }

    void ApplyShadowEffect(GameObject obj, float alpha)
    {
        foreach (Renderer r in obj.GetComponentsInChildren<Renderer>())
        {
            foreach (Material m in r.materials)
            {
                m.SetFloat("_Surface", 1); 
                m.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
                m.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
                m.SetInt("_ZWrite", 0); 
                m.renderQueue = 3000;
                m.EnableKeyword("_ALPHABLEND_ON");

                string prop = m.HasProperty("baseColorFactor") ? "baseColorFactor" : 
                             (m.HasProperty("_BaseColor") ? "_BaseColor" : "_Color");

                m.SetColor(prop, new Color(0, 0, 0, alpha));
            }
        }
    }
}