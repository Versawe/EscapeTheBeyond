using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpScareHides : MonoBehaviour
{
    private List<GameObject> jumpScares = new List<GameObject>();
    private void OnEnable()
    {
        foreach (GameObject scare in GameObject.FindGameObjectsWithTag("JumpScare")) 
        {
            jumpScares.Add(scare);
            scare.SetActive(false);
        }
    }
}
