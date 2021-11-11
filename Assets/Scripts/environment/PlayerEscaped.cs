using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerEscaped : MonoBehaviour
{
    LocalPostProcessing localPPScript;
    //public GameObject Vendigo;

    private void OnEnable()
    {
        if (GameObject.Find("FPSController"))
        {
            localPPScript = GameObject.Find("FPSController").GetComponent<LocalPostProcessing>();
        }
        else
        {
            localPPScript = null;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            AmbientClipController.ForceUpdate = false;
            localPPScript.playerPP.SetActive(false);
            //Vendigo.SetActive(false);

        }
    }
}
