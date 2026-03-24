using UnityEngine;
using System.Collections;

public class HumanAI : MonoBehaviour
{
    public Transform house;
    public float speed = 2f;

    private GameObject targetResource;
    private bool isFleeing = false;
    private bool isFarming = false;

    void Start()
    {
        FindResource();
    }

    void Update()
    {
        if (GameManager.Instance.HasBear())
        {
            Flee();
        }
        else
        {
            isFleeing = false;
        }

        if (isFleeing)
        {
            GoToHouse();
            return;
        }

        if (targetResource != null)
        {
            MoveToResource();
        }
        else
        {
            FindResource();
        }
    }

    void FindResource()
    {
        var gm = GameManager.Instance;

        if (Random.value > 0.5f && gm.trees.Count > 0)
            targetResource = gm.trees[Random.Range(0, gm.trees.Count)];
        else if (gm.rocks.Count > 0)
            targetResource = gm.rocks[Random.Range(0, gm.rocks.Count)];
    }

    void MoveToResource()
    {
        Vector3 dir = (targetResource.transform.position - transform.position).normalized;
        transform.position += dir * speed * Time.deltaTime;

        if (dir != Vector3.zero)
            transform.forward = dir;

        if (Vector3.Distance(transform.position, targetResource.transform.position) < 1f)
        {
            if (!isFarming)
                StartCoroutine(FarmRoutine());
        }
    }

    IEnumerator FarmRoutine()
    {
        isFarming = true;

        while (!GameManager.Instance.HasBear())
        {
            yield return new WaitForSeconds(5f);

            if (targetResource == null) break;

            var node = targetResource.GetComponent<ResourceNode>();

            if (node.type == ResourceNode.Type.Tree)
                DataHolding.Instance.AddResource("Arbre", 1);
            else
                DataHolding.Instance.AddResource("Rocher", 1);
        }

        isFarming = false;
    }

    void Flee()
    {
        isFleeing = true;
    }

    void GoToHouse()
    {
        if (house == null) return;

        Vector3 dir = (house.position - transform.position).normalized;
        transform.position += dir * speed * Time.deltaTime;

        if (dir != Vector3.zero)
            transform.forward = dir;

        if (Vector3.Distance(transform.position, house.position) < 0.01f)
        {
            GameManager.Instance.TryEnterHouse(this.gameObject);
        }
    }
}