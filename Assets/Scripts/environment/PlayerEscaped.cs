using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerEscaped : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Zone")
        {
            AmbientClipController.ForceUpdate = false;

        }
    }
}
