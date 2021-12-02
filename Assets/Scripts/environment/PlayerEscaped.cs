using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Script Made for at the END of the Final chase event in QandA
public class PlayerEscaped : MonoBehaviour
{
    LocalPostProcessing localPPScript; //player's local Post-Processing
    public GameObject Demon; // Demon Gameobject

    private void OnEnable()
    {
        //grabs player's GameObject
        if (GameObject.Find("FPSController"))
        {
            localPPScript = GameObject.Find("FPSController").GetComponent<LocalPostProcessing>();
        }
        else
        {
            localPPScript = null;
        }
    }

    //Once the Player enters the basement door!
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            //scary stuff activates
            AmbientClipController.ForceHell = true;
            Demon.SetActive(false); //turns demon chasing player off

        }
    }
}
