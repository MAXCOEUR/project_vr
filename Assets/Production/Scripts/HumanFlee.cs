using UnityEngine;

public class HumanFlee : MonoBehaviour
{
    public Transform house;
    public float speed = 2f;

    private bool isFleeing = false;

    public void StartFlee()
    {
        isFleeing = true;
    }

    void Update()
    {
        if (!isFleeing || house == null) return;

        Vector3 direction = (house.position - transform.position).normalized;

        transform.position += direction * speed * Time.deltaTime;

        if (direction != Vector3.zero)
            transform.forward = direction;
    }
}