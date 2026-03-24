using UnityEngine;
using System.Collections.Generic;

public class BearAI : MonoBehaviour
{
    public float speed = 2f;

    private List<GameObject> humans;

    public void SetTargets(List<GameObject> humanList)
    {
        humans = humanList;
    }

    void Update()
    {
        if (humans == null || humans.Count == 0) return;

        // nettoyer null
        humans.RemoveAll(h => h == null);

        if (humans.Count == 0) return;

        GameObject target = humans[0];

        if (target == null) return;

        Vector3 dir = (target.transform.position - transform.position).normalized;

        transform.position += dir * speed * Time.deltaTime;

        if (dir != Vector3.zero)
            transform.forward = dir;

        if (Vector3.Distance(transform.position, target.transform.position) < 0.5f)
        {
            GameManager.Instance.RemoveHuman(target);
            Destroy(target);
        }
    }

    void OnMouseDown()
    {
        GameManager.Instance.KillBear(gameObject);
    }
}