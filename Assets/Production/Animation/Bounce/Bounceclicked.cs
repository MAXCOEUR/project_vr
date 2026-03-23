using UnityEngine;

public class Bounceclicked : MonoBehaviour
{
 [SerializeField]
    Camera cam;

    Animator anim;

    void Start()
    {
        cam = Camera.main;
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        if(cam != null)
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

                    anim.SetTrigger("Bounceclicked");                   
                }
            }
        }    
    }
    }
}
