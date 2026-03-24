using UnityEngine;

public class BearChase : MonoBehaviour
{
    public Transform target;
    public float speed = 1.5f;
    public float attackDistance = 2.5f;

    private HumanFlee human;

    void Start()
    {
        if (target != null)
            human = target.GetComponent<HumanFlee>();
    }

    void Update()
    {
        if (target == null) return;

        float dist = Vector3.Distance(transform.position, target.position);

        Vector3 direction = (target.position - transform.position).normalized;

        transform.position += direction * speed * Time.deltaTime;

        if (direction != Vector3.zero)
            transform.forward = direction;

        // déclenche fuite
        if (dist < attackDistance && human != null)
        {
            human.StartFlee();
        }
    }
}