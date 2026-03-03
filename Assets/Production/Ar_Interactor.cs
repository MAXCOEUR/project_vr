using UnityEngine;

public class Ar_Interactor : MonoBehaviour
{
    [SerializeField]
    private GameObject ball;
    [SerializeField]
    private Transform user;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void InstantiateObject()
    {
        RaycastHit hit;
        if (Physics.Raycast(user.position, user.forward, out hit, 100f))
        {
            GameObject instantiatedObject = Instantiate(ball);
            instantiatedObject.transform.position = hit.point;

        }
    }
}