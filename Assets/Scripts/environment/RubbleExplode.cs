using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//This script triggers when a door is destroyed so it spawns a random number of rubble and launches them slightly
public class RubbleExplode : MonoBehaviour
{
    private float numPlanks;
    public GameObject PlankFab;
    List<GameObject> planks = new List<GameObject>();
    private void OnEnable()
    {
        planks.Clear();
        numPlanks = Random.Range(7, 15); //between 7-14 will spawn
        int i = 0;

        while (i <= numPlanks) //spawns them and adds them to a list
        {
            Quaternion randRot = Quaternion.Euler(Random.Range(0, 360), Random.Range(0, 360), Random.Range(0, 360));
            GameObject aPlank = Instantiate(PlankFab, transform.position, randRot);
            planks.Add(aPlank);
            i++;
        }

        foreach (GameObject plank in planks) //pushes random vector on plank on spawn
        {
            Vector3 rand = Vector3.up * 7;
            plank.GetComponent<Rigidbody>().AddForce(rand, ForceMode.Impulse);
        }
    }
}
