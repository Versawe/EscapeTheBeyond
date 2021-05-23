﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Subsystems;

//Works Good!!! Also works good integrated with destroying doors inbetween
public class FindPoints : MonoBehaviour
{
    public List<GameObject> pointList;
    public List<GameObject> pointTargets = new List<GameObject>();

    public GameObject Monster;
    IEnumerator co;
    NavMeshAgent nm;
    public bool searchDone = false;

    private float pointRadius;
    public GameObject targetCopy;

    private void OnEnable()
    {
        Monster.GetComponent<RipperAIMain>().aiState = "Search";
        co = destroyDoors();
        nm = GetComponent<NavMeshAgent>();
        
        foreach (GameObject point in pointList)
        {
            Vector3 monVec = new Vector3(Monster.transform.position.x, Monster.transform.position.y, Monster.transform.position.z);
            Vector3 pointVec = new Vector3(point.transform.position.x, point.transform.position.y, point.transform.position.z);
           
            pointRadius = Vector3.Distance(monVec, pointVec);
            if (pointRadius <= 15)
            {
                //print(point.name + "passes first check");
                float yCheck = monVec.y - pointVec.y;
                if (yCheck <= 2 && yCheck >= -2)
                {
                    pointTargets.Add(point);
                    //print(point.name + "passes y check");
                }
            }
        }

        if (pointTargets.Count <= 0)
        {
            //print("no rooms near to check");
            pointTargets.Clear();
            searchDone = true;
            targetCopy = null;
            //StopCoroutine(co);
        }
        else 
        {
            StartCoroutine(co);
        } 

    }

    private void OnDisable()
    {
        StopCoroutine(co);
        searchDone = false;
        pointTargets.Clear();
        Monster.GetComponent<RipperAIMain>().aiState = "Patrol";
    }

    private IEnumerator destroyDoors()
    {
        WaitForSeconds wait = new WaitForSeconds(25f);
        foreach (GameObject point in pointTargets)
        {
            //print(point);
            targetCopy = point;
            nm.isStopped = false;
            nm.SetDestination(point.transform.position);
            nm.updateRotation = true;

            yield return wait;
        }
        pointTargets.Clear();
        searchDone = true;
        targetCopy = null;
        StopCoroutine(co);
    }

}
