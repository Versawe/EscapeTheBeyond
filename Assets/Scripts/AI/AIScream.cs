using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

//this spawns a scream gameobject: a star like pattern gameobject attched to the ground
//the ai them iterates through each vector3 on this object and jumps around it.
//this is used for the Screaming state's logic
public class AIScream : MonoBehaviour
{

    public GameObject ScreamPattern;
    public List<GameObject> screamPoints;

    NavMeshAgent nm;

    IEnumerator co;

    private void OnEnable()
    {
        nm = GetComponent<NavMeshAgent>();
        nm.stoppingDistance = 0.01f; //shortens stopping distance so AI gets close to the scream points

        Instantiate(ScreamPattern, transform.position, transform.rotation); //spawns scream points
        for (int i = 1; i <= 5; i++) //adds the points to a list with their respective clone names
        {
            screamPoints.Add(GameObject.Find("ScreamPoint" + i.ToString()));
        }
        
        AddsToList(); //doubles the list for two rotations on scream
        AddsToList(); //triples list

        co = moveThroughPoints();
        StartCoroutine(co);
    }

    private void AddsToList()
    {
        for (int i = 0; i <= 4; i++)
        {
            screamPoints.Add(screamPoints[i]);
        }
    }

    private void OnDisable() //resets for next scream event
    {
        StopCoroutine(co);
        screamPoints.Clear();
        nm.stoppingDistance = 0.25f;
    }

    private IEnumerator moveThroughPoints() //coroutine that moves the ripper through the pattern or list of gameobjects
    {
        WaitForSeconds wait = new WaitForSeconds(0.40f);
        foreach (GameObject point in screamPoints)
        {
            nm.isStopped = false;
            nm.SetDestination(point.transform.position);
            nm.updateRotation = true;

            yield return wait;
        }
    }
}
