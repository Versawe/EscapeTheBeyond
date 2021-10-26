using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpScareHides : MonoBehaviour
{
    private List<GameObject> jumpScares = new List<GameObject>();
    private void OnEnable()
    {
        if (NoDestroy.currSceneName != "RelicHunt") return;
        foreach (GameObject scare in GameObject.FindGameObjectsWithTag("JumpScare")) 
        {
            jumpScares.Add(scare);
            scare.SetActive(false);
            print(scare.name);
        }
    }

    private void OnDisable()
    {
        if (NoDestroy.currSceneName != "RelicHunt") return;
        foreach (GameObject scare in jumpScares)
        {
            scare.SetActive(true);
            print(scare.name);
        }
        print("hello?"); //needs testing...
    }
}
