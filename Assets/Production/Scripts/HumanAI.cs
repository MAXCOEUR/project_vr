using UnityEngine;
using System.Collections;

public class HumanAI : MonoBehaviour
{
    public Transform house;
    public float speed = 2f;

    private bool isFleeing = false;

    void Start()
    {
        StartCoroutine(FarmRoutine());
    }

    void Update()
    {
        if (isFleeing && house != null)
        {
            Vector3 dir = (house.position - transform.position).normalized;
            transform.position += dir * speed * Time.deltaTime;

            if (dir != Vector3.zero)
                transform.forward = dir;

            if (Vector3.Distance(transform.position, house.position) < 0.5f)
            {
                GameManager.Instance.RemoveHuman(gameObject);
                Destroy(gameObject);
            }
        }
        else
        {
            Wander();
        }
    }

    void Wander()
    {
        transform.position += transform.forward * speed * 0.3f * Time.deltaTime;

        if (Random.value < 0.01f)
        {
            transform.Rotate(0, Random.Range(-90, 90), 0);
        }
    }

    IEnumerator FarmRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(5f);

            if (!isFleeing)
            {
                if (Random.value > 0.5f)
                    DataHolding.Instance.AddResource("Arbre", 1);
                else
                    DataHolding.Instance.AddResource("Rocher", 1);
            }
        }
    }

    public void Flee()
    {
        isFleeing = true;
    }
}