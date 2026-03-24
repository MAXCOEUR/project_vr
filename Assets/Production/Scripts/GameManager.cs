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
    public List<GameObject> trees = new List<GameObject>();
    public List<GameObject> rocks = new List<GameObject>();

    public int houseCapacity = 3;
    private int currentInHouse = 0;

    void Awake()
    {
        Instance = this;
    }


    public bool HasBear()
    {
        return currentBear != null;
    }

    public bool TryEnterHouse()
    {
        if (currentInHouse >= houseCapacity) return false;

        currentInHouse++;
        return true;
    }

    public void SpawnHuman(Vector3 position, Transform house)
    {
        GameObject human = Instantiate(humanPrefab, position, Quaternion.identity);

        HumanAI ai = human.GetComponent<HumanAI>();
        if (ai != null)
        {
            ai.house = house;
        }

        humans.Add(human);

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