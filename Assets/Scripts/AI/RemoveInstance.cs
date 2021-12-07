using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

//another destroying script
//destroys game object attached to after 10 seconds
//this is used to destroy the created searchPoint that the AI tracks after losing sight of the player for 10 seconds
public class RemoveInstance : MonoBehaviour
{
    private float timer = 10;

    // Update is called once per frame
    void Update()
    {
        timer -= Time.deltaTime;
        if (timer <= 0)
        {
            Destroy(gameObject);
        }
    }
}
