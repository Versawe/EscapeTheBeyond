using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DeactivateDoorScripts : MonoBehaviour
{

    private float valueX = 180; //was 180
    private float valueZ = -90;

    DoorOpen openScript;

    Scene currScene;

    private void Start()
    {
        currScene = SceneManager.GetActiveScene();
        openScript = GetComponent<DoorOpen>();
        doorValues();
        if(currScene.name == "RelicHunt") openScript.enabled = false;
    }

    private void doorValues()
    {
        if (gameObject.tag == "DoorX")
        {
            transform.eulerAngles = new Vector3(0f, valueX, 0f);
            openScript.doorStartRot = Quaternion.Euler(0f, valueX, 0f);
        }
        if (gameObject.tag == "DoorZ")
        {
            transform.eulerAngles = new Vector3(0f, valueZ, 0f);
            openScript.doorStartRot = Quaternion.Euler(0f, valueZ, 0f);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "scriptTrigger")
        {
            openScript.enabled = true;
        }   
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "scriptTrigger")
        {
            openScript.enabled = false;
        }
    }
}
