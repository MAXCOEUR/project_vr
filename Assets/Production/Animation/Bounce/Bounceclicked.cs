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
        if (cam != null)
        {
            if (Input.GetMouseButtonDown(0))
            {
                Ray ray = cam.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;

                if (Physics.Raycast(ray, out hit))
                {
                    ARIdentity identity = hit.transform.GetComponentInParent<ARIdentity>();
                    if (identity != null)
                    {
                        if (identity.isShadow) 
                        {
                            Debug.Log("On ne peut pas récolter sur un fantôme !");
                        }
                        else 
                        {
                            if (hit.transform == transform)
                            {
                                if(identity.category == "Arbre" || identity.category == "Rocher")
                                {
                                    DataHolding.Instance.AddResource(identity.category, 1);
                                }
                                else
                                {
                                    if (DataHolding.Instance.TrySpendResources(identity.level)) {
                                        // Le paiement est fait, ici tu déclenches l'évolution visuelle
                                        Debug.Log("L'amélioration de la maison commence !");
                                    }
                                    else
                                    {
                                        Debug.Log("Pas assez de ressources pour améliorer la maison !");
                                    }
                                }
                                

                                anim.SetTrigger("Bounceclicked");
                            }
                        }
                    }
                }
            }
        }
    }
}