using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using TMPro;

public class CarouselManager : MonoBehaviour
{
    // Instance statique pour le Singleton
    public static CarouselManager Instance;

    public GameObject modelRootPrefab; // Un prefab vide qui servira de parent (root) pour les modèles instanciés

    [System.Serializable]
    public class CategoryData
    {
        [HideInInspector] public string categoryName;
        public List<Sprite> levelIcons;
        public List<GameObject> levels;
        [HideInInspector] public int currentLevelIndex = 0;
        [HideInInspector] public GameObject spawnedInstance;
    }

    [Header("Pages")]
    public GameObject buildUi;
    public GameObject gameUi;

    [Header("Character Spawn")]
    public GameObject characterPrefab;
    private GameObject spawnedCharacter;

    [Header("Bear Spawn")]
    public GameObject bearPrefab;
    private GameObject spawnedBear;

    [Header("Progression Data")]
    public CategoryData houseCategory;
    public CategoryData treeCategory;
    public CategoryData rockCategory;

    private List<CategoryData> allCategories = new List<CategoryData>();
    private int currentCategoryIndex = 0;
    private CategoryData activeCategory;

    [Header("UI References")]
    public Image displayImage;
    public GameObject spawnButton;

    [Header("Toggle Preview Settings")]
    public Image previewButtonImage;
    public TMP_Text previewButtonText;
    public Color onColor = Color.green;
    public Color offColor = Color.red;

    [Header("AR Settings")]
    public ARRaycastManager raycastManager;

    private GameObject previewModel;
    private bool isPreviewEnabled = true;
    static List<ARRaycastHit> hits = new List<ARRaycastHit>();

    void Awake()
    {
        // Initialisation du Singleton
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    void OnValidate()
    {
        houseCategory.categoryName = "Maison";
        treeCategory.categoryName = "Arbre";
        rockCategory.categoryName = "Rocher";
    }

    void Start()
    {
        if (allCategories.Count == 0)
        {
            allCategories.Add(houseCategory);
            allCategories.Add(treeCategory);
            allCategories.Add(rockCategory);
        }

        LoadUserLevelsFromDB();
        UpdateActiveCategory();

        if (previewButtonImage != null) previewButtonImage.color = onColor;
        if (previewButtonText != null) previewButtonText.text = "Preview: ON";
        if (gameUi != null) gameUi.SetActive(false);
        if (previewButtonText != null) previewButtonText.text = "Preview: ON";
    }

    public void TogglePreview()
    {
        isPreviewEnabled = !isPreviewEnabled;
        if (isPreviewEnabled)
        {
            if (previewButtonImage != null) previewButtonImage.color = onColor;
            if (previewButtonText != null) previewButtonText.text = "Preview: ON";
        }
        else
        {
            if (previewButtonImage != null) previewButtonImage.color = offColor;
            if (previewButtonText != null) previewButtonText.text = "Preview: OFF";
            if (previewModel != null) previewModel.SetActive(false);
        }
    }

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

    void Update()
    {
        bool alreadySpawned = activeCategory.spawnedInstance != null;
        if (isPreviewEnabled && !alreadySpawned) UpdatePreviewPosition();
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
                ARIdentity id = previewModel.AddComponent<ARIdentity>();
                id.isShadow = true;
                id.category = activeCategory.categoryName;
                id.level = activeCategory.currentLevelIndex;
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
        if (previewModel != null && previewModel.activeSelf && activeCategory.spawnedInstance == null)
        {
            GameObject newRoot = Instantiate(modelRootPrefab, previewModel.transform.position, previewModel.transform.rotation);
            newRoot.name = "Root_" + activeCategory.categoryName;

            if (activeCategory.categoryName == "Maison")
            {
                RootModelPrefab rootScript = newRoot.GetComponent<RootModelPrefab>();
                if (rootScript != null)
                {
                    rootScript.levels = new List<GameObject>();
                    foreach (GameObject levelPrefab in activeCategory.levels)
                    {
                        GameObject levelInstance = Instantiate(levelPrefab, newRoot.transform);
                        
                        levelInstance.transform.localPosition = Vector3.zero;
                        levelInstance.transform.localRotation = Quaternion.identity;
                        
                        levelInstance.SetActive(false);
                        rootScript.levels.Add(levelInstance);
                    }
                    rootScript.RefreshVisual();
                }
            }
            else
            {
                GameObject prefabModel = activeCategory.levels[activeCategory.currentLevelIndex];
                GameObject modelInstance = Instantiate(prefabModel, newRoot.transform);
                modelInstance.transform.localPosition = Vector3.zero;
                modelInstance.transform.localRotation = Quaternion.identity;
            }

            activeCategory.spawnedInstance = newRoot;
           
            ARIdentity id = newRoot.AddComponent<ARIdentity>();
            id.isShadow = false;
            id.category = activeCategory.categoryName;
            
            id.level = (activeCategory.categoryName == "Maison") ? DataHolding.Instance.houseCurrentLevel : activeCategory.currentLevelIndex;

        previewModel.SetActive(false);
        NextCategory();
            
            Debug.Log(newRoot.name + " créé avec succès.");
    }
}

    bool AreAllItemsPlaced()
    {
        foreach (var cat in allCategories)
        {
            if (cat.spawnedInstance == null) return false;
        }
        return true;
    }

    public void ResetAll()
    {
        foreach (var cat in allCategories)
        {
            if (cat.spawnedInstance != null) Destroy(cat.spawnedInstance);
            cat.spawnedInstance = null;
        }
        currentCategoryIndex = 0;
        UpdateActiveCategory();
    }

    void RefreshPreview() { if (previewModel != null) Destroy(previewModel); }

    void UpdateUI()
    {
        if (activeCategory != null)
        {
            if (displayImage != null && activeCategory.levelIcons.Count > activeCategory.currentLevelIndex)
                displayImage.sprite = activeCategory.levelIcons[activeCategory.currentLevelIndex];
            bool alreadySpawned = activeCategory.spawnedInstance != null;
            bool everythingPlaced = AreAllItemsPlaced();
            if (spawnButton != null) spawnButton.SetActive(!alreadySpawned);
            if (gameUi != null) gameUi.SetActive(everythingPlaced);
            if (buildUi != null) buildUi.SetActive(!everythingPlaced);
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
                string prop = m.HasProperty("baseColorFactor") ? "baseColorFactor" : (m.HasProperty("_BaseColor") ? "_BaseColor" : "_Color");
                m.SetColor(prop, new Color(0, 0, 0, alpha));
            }
        }
    }

    void LoadUserLevelsFromDB()
    {
        DataHolding.Instance.houseCurrentLevel = 0;
        houseCategory.currentLevelIndex = 0;
        treeCategory.currentLevelIndex = 0;
        rockCategory.currentLevelIndex = 0;
    }
}