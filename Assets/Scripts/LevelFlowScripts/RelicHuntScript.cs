using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RelicHuntScript : MonoBehaviour
{
    GameObject RelicHub;
    List<GameObject> RelicList = new List<GameObject>();
    List<GameObject> ChosenList = new List<GameObject>();
    GameObject RipperAI;
    //bool IsHuntStart = false;

    void OnEnable()
    {
        RipperAI = GameObject.Find("ripperAI");
        if(RipperAI != null) RipperAI.SetActive(false);
        RelicHub = GameObject.Find("RelicSpawnLocations");

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
}
