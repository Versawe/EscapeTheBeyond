using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeactivateWardrobeScripts : MonoBehaviour
{
    public GameObject rightDoor;
    public GameObject leftDoor;

    WardrobeOpen warScript;
    // Start is called before the first frame update
    void Start()
    {
        warScript = GetComponent<WardrobeOpen>();
        doorValues();
        warScript.enabled = false;
    }

    private void doorValues()
    {
        warScript.doorStartRotRight = Quaternion.Euler(0,0,0);
        warScript.doorStartRotLeft = Quaternion.Euler(0,0,0);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "scriptTrigger")
        {
            warScript.enabled = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "scriptTrigger")
        {
            warScript.enabled = false;
        }
    }
}
