using UnityEngine;

public class Bounceclicked : MonoBehaviour
{
    [SerializeField] Camera cam;
    Animator anim;

    void Start()
    {
        cam = Camera.main;
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        if (cam != null && Input.GetMouseButtonDown(0))
        {
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                // On cherche l'identité sur l'objet touché ou ses parents
                ARIdentity identity = hit.transform.GetComponentInParent<ARIdentity>();

                if (identity != null && !identity.isShadow)
                {
                    // Vérifie si on a cliqué sur CET objet (le parent) ou un de ses enfants visuels
                    if (hit.transform.IsChildOf(this.transform) || hit.transform == this.transform)
                    {
                        if (identity.category == "Arbre" || identity.category == "Rocher")
                        {
                            DataHolding.Instance.AddResource(identity.category, 1);
                        }
                        else if (identity.category == "Maison")
                        {
                            // On tente de payer l'amélioration
                            if (DataHolding.Instance.TrySpendResources(identity.level))
                            {
                                UpgradeHouse(identity);
                            }
                            else
                            {
                                Debug.Log("Ressources insuffisantes !");
                            }
                        }

                        // Déclenche l'animation de rebond si elle existe
                        if (anim != null) anim.SetTrigger("Bounceclicked");
                    }
                }
            }
        }
    }

    void UpgradeHouse(ARIdentity identity)
    {
        var category = CarouselManager.Instance.houseCategory;

        // Vérifie s'il existe un niveau suivant
        if (identity.level + 1 < category.levels.Count)
        {
            identity.level++;
            GameObject nextPrefab = category.levels[identity.level];

            // Remplace le modèle 3D enfant
            ReplaceVisualModel(nextPrefab);

            // Rafraîchit les textes de prix sur l'UI UpgradeStatus
            UpgradeStatus statusUI = GetComponentInChildren<UpgradeStatus>();
            if (statusUI != null) statusUI.Start();

            Debug.Log("Maison améliorée au niveau " + (identity.level + 1));
        }
        else
        {
            Debug.Log("Niveau maximum atteint !");
        }
    }

    void ReplaceVisualModel(GameObject newPrefab)
    {
        foreach (Transform child in transform)
        {
            if (child.GetComponent<Canvas>() == null)
            {
                Destroy(child.gameObject);
            }
        }

        GameObject newVisual = Instantiate(newPrefab, transform.position, transform.rotation);
        newVisual.transform.SetParent(this.transform);

        newVisual.transform.localScale = Vector3.one;
    }
}