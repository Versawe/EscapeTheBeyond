using System.Collections.Generic;
using UnityEngine;

public class RelicHuntScript : MonoBehaviour
{
    public List<GameObject> AiSpawns = new List<GameObject>();
    List<GameObject> RelicList = new List<GameObject>();
    List<GameObject> ChosenList = new List<GameObject>();

    public GameObject RipperAIPrefab;
    public GameObject MutantAIPrefab;
    GameObject RelicHub;
    GameObject RipperAI;
    GameObject HintLight;

    public bool IsHuntStart = false;
    private int spawnCount = 0;
    GameHUDActivations hudScript;

    private GameObject Player;

    void OnEnable()
    {
        if (GameObject.Find("GameHUD")) hudScript = GameObject.Find("GameHUD").GetComponent<GameHUDActivations>();
        else hudScript = null;

        if (GameObject.Find("HintLight")) 
        {
            HintLight = GameObject.Find("HintLight");
            Destroy(HintLight);
        } 
        else HintLight = null;

        if (GameObject.Find("FPSController"))
        {
            Player = GameObject.Find("FPSController");
        }
        else Player = null;

        foreach (GameObject spawns in GameObject.FindGameObjectsWithTag("aiSpawn"))
        {
            AiSpawns.Add(spawns);
        }

        IsHuntStart = true;
        RipperAI = Instantiate(RipperAIPrefab, AiSpawns[Random.Range(0,AiSpawns.Count-1)].transform.position, AiSpawns[Random.Range(0, AiSpawns.Count-1)].transform.rotation);

        foreach(GameObject HubChild in GameObject.FindGameObjectsWithTag("relicSpawn")) 
        {
            RelicList.Add(HubChild);
        }

        ChosenList = CreateNewList(RelicList);

        foreach (GameObject point in ChosenList)
        {
            point.SetActive(true);
            point.GetComponent<RelicSpawnLocation>().enabled = true;
        }
        foreach (GameObject point in RelicList)
        {
            if (!point.GetComponent<RelicSpawnLocation>().isActiveAndEnabled) point.GetComponent<DestroyOBJ>().enabled = true;
        }

    }

    private void OnDisable()
    {
        RelicList.Clear();
        ChosenList.Clear();
        AiSpawns.Clear();
        IsHuntStart = false;
        spawnCount = 0;
    }

    private List<GameObject> CreateNewList(List<GameObject> thisList) 
    {
        List<GameObject> randomList = new List<GameObject>();

        for (int i = 0; i < 15; i++)
        {
            int randomIndex = Random.Range(0, thisList.Count);
            randomList.Add(thisList[randomIndex]);
            //print(thisList[randomIndex].name);
            thisList.RemoveAt(randomIndex);
        }
        //print(randomList.Count);
        return randomList;
    }

    public void SpawnAI() 
    {
        //find out which spawn place is farthest from player
        float dist1 = Vector3.Distance(Player.transform.position, AiSpawns[0].transform.position);
        float dist2 = Vector3.Distance(Player.transform.position, AiSpawns[1].transform.position);

        Transform farthestSpawn;
        if (dist1 > dist2)
        {
            farthestSpawn = AiSpawns[0].transform;
        }
        else
        {
            farthestSpawn = AiSpawns[1].transform;
        }

        //spawn Another AI
        spawnCount++;
        GameObject mutant = Instantiate(MutantAIPrefab, farthestSpawn.position, farthestSpawn.rotation);
        if (spawnCount == 1) mutant.name = "thing1";
        else mutant.name = "thing2";
    }
}
