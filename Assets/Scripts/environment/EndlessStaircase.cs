using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndlessStaircase : MonoBehaviour
{
    public GameObject NoReturnBox;
    public Transform NextSpawnLocation;
    public GameObject StaircaseChunk;

    private bool SpawnOnce = false;

    private void SpawnStairs() 
    {
        GameObject newStairs = Instantiate(StaircaseChunk, NextSpawnLocation.position, NextSpawnLocation.rotation);
        NoReturnBox.SetActive(true);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player" && !SpawnOnce) 
        {
            SpawnStairs();
            NoDestroy.stairs.Add(gameObject);
            NoDestroy.CheckStairCount();
            SpawnOnce = true;
        }
    }
}
