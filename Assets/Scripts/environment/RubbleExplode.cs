using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RubbleExplode : MonoBehaviour
{
    private float numPlanks;
    public GameObject PlankFab;
    List<GameObject> planks = new List<GameObject>();
    private void OnEnable()
    {
        planks.Clear();
        numPlanks = Random.Range(7, 15);
        int i = 0;

        while (i <= numPlanks) 
        {
            Quaternion randRot = Quaternion.Euler(Random.Range(0, 360), Random.Range(0, 360), Random.Range(0, 360));
            GameObject aPlank = Instantiate(PlankFab, transform.position, randRot);
            planks.Add(aPlank);
            i++;
        }

        foreach (GameObject plank in planks) 
        {
            Vector3 rand = Vector3.up * 7;
            plank.GetComponent<Rigidbody>().AddForce(rand, ForceMode.Impulse);
        }
    }
}
