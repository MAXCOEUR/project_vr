using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("Prefabs")]
    public GameObject humanPrefab;
    public GameObject bearPrefab;

    [Header("Bear Spawn Settings")]
    public float bearSpawnDelayMin = 8f;
    public float bearSpawnDelayMax = 15f;
    public int humansRequiredForBear = 3;

    private List<GameObject> humans = new List<GameObject>();
    private GameObject currentBear;

    public List<GameObject> trees = new List<GameObject>();
    public List<GameObject> rocks = new List<GameObject>();
    private int currentInHouse = 0;

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        StartCoroutine(BearSpawnLoop());
    }

    IEnumerator BearSpawnLoop()
    {
        while (true)
        {
            float wait = Random.Range(bearSpawnDelayMin, bearSpawnDelayMax);
            yield return new WaitForSeconds(wait);


            humans.RemoveAll(h => h == null);

            if (currentBear == null && humans.Count >= humansRequiredForBear)
            {
                Vector3 pos = humans[0].transform.position + new Vector3(3f, 0, 3f);

                currentBear = Instantiate(bearPrefab, pos, Quaternion.identity);

                BearAI ai = currentBear.GetComponent<BearAI>();
                if (ai != null)
                {
                    ai.SetTargets(humans);
                }

                Debug.Log("🐻 Ours spawn !");
            }
        }
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
        DataHolding.Instance.AddResource("Human", 1);
    }

    public void RemoveHuman(GameObject human)
    {
        humans.Remove(human);
        DataHolding.Instance.deadHuman();
    }

    public void KillBear(GameObject bear)
    {
        if (bear == currentBear)
        {
            Destroy(currentBear);
            currentBear = null;
            Debug.Log("🐻 Ours tué → respawn plus tard");
        }
        else
        {
            Destroy(bear);
        }
    }

    public bool HasBear()
    {
        return currentBear != null;
    }
    public bool TryEnterHouse()
    {
                int houseLevel = DataHolding.Instance.houseCurrentLevel;
        int houseCapacity = DataHolding.Instance.capacityHouses[houseLevel];
        if (currentInHouse >= houseCapacity) return false;

        currentInHouse++;
        return true;
    }
}