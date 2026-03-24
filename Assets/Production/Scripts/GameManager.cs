using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("Prefabs")]
    public GameObject humanPrefab;
    public GameObject bearPrefab;

    private List<GameObject> humans = new List<GameObject>();
    private GameObject currentBear;

    void Awake()
    {
        Instance = this;
    }

    // 🧍 Ajouter humain
    public void SpawnHuman(Vector3 position, Transform house)
    {
        GameObject human = Instantiate(humanPrefab, position, Quaternion.identity);

        HumanAI ai = human.GetComponent<HumanAI>();
        if (ai != null)
        {
            ai.house = house;
        }

        humans.Add(human);

        // 👇 spawn ours si 3 humains
        if (humans.Count >= 3 && currentBear == null)
        {
            StartCoroutine(SpawnBearWithDelay());
        }
    }

    IEnumerator SpawnBearWithDelay()
    {
        yield return new WaitForSeconds(Random.Range(2f, 5f));

        if (currentBear == null && humans.Count > 0)
        {
            Vector3 pos = humans[0].transform.position + new Vector3(3f, 0, 3f);

            currentBear = Instantiate(bearPrefab, pos, Quaternion.identity);

            BearAI ai = currentBear.GetComponent<BearAI>();
            if (ai != null)
            {
                ai.SetTargets(humans);
            }
        }
    }

    public void RemoveHuman(GameObject human)
    {
        humans.Remove(human);
    }

    public void KillBear(GameObject bear)
    {
        Destroy(bear);
        currentBear = null;
    }
}