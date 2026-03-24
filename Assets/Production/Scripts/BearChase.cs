using UnityEngine;

public class BearChase : MonoBehaviour
{
    public Transform target;
    public float speed = 1.5f;

    void Update()
    {
        if (target == null) return;

        Vector3 direction = (target.position - transform.position).normalized;

        transform.position += direction * speed * Time.deltaTime;

        if (direction != Vector3.zero)
            transform.forward = direction;
    }
}