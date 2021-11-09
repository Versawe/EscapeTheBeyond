using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlashLightOff : MonoBehaviour
{
    //this script is used in the QandA scene only and controls the post-processing and flashlight gameobject
    //thinking about using this for after beating the q&a game, spawning the demon to chase you to the basement door!
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Zone") 
        {
            Light FlashLight = GameObject.Find("Flashlight").GetComponent<Light>();
            FlashLight.enabled = false;
            AmbientClipController.AmbientScary();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Zone")
        {
            Light FlashLight = GameObject.Find("Flashlight").GetComponent<Light>();
            FlashLight.enabled = true;
            AmbientClipController.AmbientNormal();
        }
    }
}
