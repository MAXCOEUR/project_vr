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
                ARIdentity identity = hit.transform.GetComponentInParent<ARIdentity>();

                if (identity != null && !identity.isShadow)
                {
                    if (hit.transform.IsChildOf(this.transform) || hit.transform == this.transform)
                    {
                        if (identity.category == "Arbre" || identity.category == "Rocher")
                        {
                            DataHolding.Instance.AddResource(identity.category, 1);
                        }
                        else if (identity.category == "Maison")
                        {
                            DataHolding.Instance.TrySpendResources(DataHolding.Instance.houseCurrentLevel);
                        }

                        if (anim != null) anim.SetTrigger("Bounceclicked");
                    }
                }
            }
        }
    }
}