using UnityEngine;

public class Bounceclicked : MonoBehaviour
{
    [SerializeField] Camera cam;

    Animator anim;
    public int clickCount = 0; 

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
                    if (hit.transform == transform)
                    {
                        clickCount++; 
                        Debug.Log(gameObject.name + " clics : " + clickCount);

                        anim.SetTrigger("Bounceclicked");

                        CheckActions();
                    }
                }
            }
        }
    }

    void CheckActions()
    {
        if (clickCount == 3)
        {
            Debug.Log("3 clics !");
            transform.localScale *= 1.2f;
        }

        if (clickCount == 5)
        {
            Debug.Log("5 clics !");
            GetComponent<Renderer>().material.color = Color.red;
        }
    }
}