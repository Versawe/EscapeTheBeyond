using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AIScream : MonoBehaviour
{

    public GameObject ScreamPattern;
    public List<GameObject> screamPoints;

    NavMeshAgent nm;

    IEnumerator co;

    private void OnEnable()
    {
        nm = GetComponent<NavMeshAgent>();
        nm.stoppingDistance = 0.01f;

        Instantiate(ScreamPattern, transform.position, transform.rotation);
        for (int i = 1; i <= 5; i++)
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

    private void OnDisable()
    {
        StopCoroutine(co);
        screamPoints.Clear();
        nm.stoppingDistance = 0.5f;
    }

    private IEnumerator moveThroughPoints()
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
