using System.Collections.Generic;
using UnityEngine;

//chooses random gifs for the jumpscares and sets them to components
//makes it super cool and different everytime
public class JumpScareHides : MonoBehaviour
{
    private List<GameObject> jumpScares = new List<GameObject>();

    RelicHuntScript relicScript;

    private void Awake()
    {
        relicScript = GetComponent<RelicHuntScript>(); 
    }

    private void OnEnable()
    {
        if (jumpScares.Count > 0) jumpScares.Clear();
        if (NoDestroy.currSceneName != "RelicHunt") return;
        foreach (GameObject scare in GameObject.FindGameObjectsWithTag("JumpScare")) 
        {
            jumpScares.Add(scare);
            scare.SetActive(false);
            //print(scare.name);
        }
    }

    private void OnDisable()
    {
        //if (!relicScript.isActiveAndEnabled) print("skip this ish");
        if (!relicScript.isActiveAndEnabled) return;
        if (NoDestroy.currSceneName != "RelicHunt") return;
        foreach (GameObject scare in jumpScares)
        {
            scare.SetActive(true);
            //print(scare.name);
        }
        //print("hello?");
    }
}
