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

    private bool doOnce = false;

    private void Update()
    {
        FailRelicHunt();    
    }

    void OnEnable()
    {
        AudioController.PlayDialogueSound(3);
        doOnce = false;

        foreach (GameObject light in GameObject.FindGameObjectsWithTag("Lamp")) //turns off all lights c:
        {
            light.SetActive(false);
        }

        if (GameObject.Find("GameHUD")) hudScript = GameObject.Find("GameHUD").GetComponent<GameHUDActivations>(); //get HUD script
        else hudScript = null;

        if (GameObject.Find("HintLight")) //turns off hint light
        {
            HintLight = GameObject.Find("HintLight");
            Destroy(HintLight);
        } 
        else HintLight = null;

        if (GameObject.Find("FPSController")) //grab player GameObject
        {
            Player = GameObject.Find("FPSController");
        }
        else Player = null;

        foreach (GameObject spawns in GameObject.FindGameObjectsWithTag("aiSpawn")) //adds AI spawnpoints to list
        {
            AiSpawns.Add(spawns);
        }

        IsHuntStart = true; //hunt begins
        //spawns Ripper at random ai spawn location, or maybe only the first one?
        RipperAI = Instantiate(RipperAIPrefab, AiSpawns[Random.Range(0,AiSpawns.Count-1)].transform.position, AiSpawns[Random.Range(0, AiSpawns.Count-1)].transform.rotation);

        foreach(GameObject HubChild in GameObject.FindGameObjectsWithTag("relicSpawn")) //gets possibly relic spawn locations added to a list
        {
            RelicList.Add(HubChild);
        }

        ChosenList = CreateNewList(RelicList); //randomly generates a randomly ordered list to switch up relic spawn locations

        foreach (GameObject point in ChosenList) //sets the spawn points active and activates the scripts attached so player can interact
        {
            point.SetActive(true);
            point.GetComponent<RelicSpawnLocation>().enabled = true;
        }
        foreach (GameObject point in RelicList) //this deletes the other gameobjects not in use from the scene to free up memory
        {
            if (!point.GetComponent<RelicSpawnLocation>().isActiveAndEnabled) point.GetComponent<DestroyOBJ>().enabled = true;
        }

        foreach(GameObject gif in GameObject.FindGameObjectsWithTag("GIF")) 
        {
            gif.GetComponent<GIFTrigger>().enabled = true;
        }

        NoDestroy.currObjective = "Current Objective:\nCollect all 15 relics to craft a key for the locked door"; //sets pause menu objective text
    }

    private void OnDisable() //on script disabled
    {
        RelicList.Clear();
        ChosenList.Clear();
        AiSpawns.Clear();
        IsHuntStart = false;
        doOnce = false;
        spawnCount = 0;
    }

    //nice function to take a list and choose 15 relic spots from the list at random and creates new list from it to use
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

    //function to run when spawning a new AI monster
    //it will check the players location and spawn them farthest from the player
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

    public void FailRelicHunt() 
    {

        if (!NoDestroy.atGameOver) return;
        
        if(doOnce == false) 
        {
            foreach (GameObject gif in GameObject.FindGameObjectsWithTag("GIF"))
            {
                gif.SetActive(false);
            }
            foreach (GameObject js in GameObject.FindGameObjectsWithTag("JumpScare"))
            {
                js.SetActive(false);
            }

            doOnce = true;
        }
    }

}
