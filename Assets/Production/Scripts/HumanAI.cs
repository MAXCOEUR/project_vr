using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class HumanAI : MonoBehaviour
{
    public Transform house;
    public float speed = 2f;
    private Vector3 wanderDir;
    private float wanderTimer;
    private GameObject targetResource;
    private bool isFleeing = false;
    private bool isFarming = false;
    private ResourceNode node;

    void Start()
    {
        FindResource();
    }

    void Update()
    {
        if (GameManager.Instance.HasBear())
        {
            Flee();
            return;
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
            Wander();
            FindResource();
        }
    }

void FindResource()
{
    var gm = GameManager.Instance;

    targetResource = null;
    node = null;

    gm.trees.RemoveAll(t => t == null);
    gm.rocks.RemoveAll(r => r == null);

    List<ResourceNode> available = new List<ResourceNode>();

    foreach (var t in gm.trees)
    {
        var n = t.GetComponent<ResourceNode>();
        if (n != null && !n.isOccupied)
            available.Add(n);
    }

    foreach (var r in gm.rocks)
    {
        var n = r.GetComponent<ResourceNode>();
        if (n != null && !n.isOccupied)
            available.Add(n);
    }

    if (available.Count == 0)
    {
        return;
    }
    node = available[Random.Range(0, available.Count)];
    targetResource = node.gameObject;

    node.isOccupied = true;
}

void MoveToResource()
{
    if (targetResource == null)
    {
        node = null;
        return;
    }

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

        if (node == null)
            break;

        switch (node.type)
        {
            case ResourceNode.Type.Tree:
                DataHolding.Instance.AddResource("Arbre", 1);
                break;

            case ResourceNode.Type.Rock:
                DataHolding.Instance.AddResource("Rocher", 1);
                break;
        }
    }

    isFarming = false;

    if (node != null)
        node.isOccupied = false;

    targetResource = null;
    node = null;
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

        if (Vector3.Distance(transform.position, house.position) < 1f)
        {
            if (GameManager.Instance.TryEnterHouse())
            {
                Destroy(gameObject);
            }
        }
    }

    void Wander()
{
    wanderTimer -= Time.deltaTime;

    if (wanderTimer <= 0f)
    {
        wanderDir = new Vector3(
            Random.Range(-1f, 1f),
            0f,
            Random.Range(-1f, 1f)
        ).normalized;

        wanderTimer = Random.Range(2f, 4f);
    }

    transform.position += wanderDir * speed * 0.5f * Time.deltaTime;

    if (wanderDir != Vector3.zero)
        transform.forward = wanderDir;
}
}