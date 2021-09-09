using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlashLightOff : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Zone") 
        {
            Light FlashLight = GameObject.Find("Flashlight").GetComponent<Light>();
            FlashLight.enabled = false;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Zone")
        {
            Light FlashLight = GameObject.Find("Flashlight").GetComponent<Light>();
            FlashLight.enabled = true;
        }
    }
}
