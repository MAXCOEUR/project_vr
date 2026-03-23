using UnityEngine;

public class starter_houseclicked : MonoBehaviour
{
 [SerializeField]
    Camera cam;

    Animator anim;

    void Start()
    {
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
 
                if (hit.transform == transform)
                {
                    Debug.Log("AAAAAAHHHHH");

                anim.SetTrigger("starter_houseclicked");                   
                }
            }
        }    
    }
}
