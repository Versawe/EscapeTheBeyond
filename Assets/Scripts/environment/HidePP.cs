using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//hides mesh renderer on patrol points
//not in use anymore
public class HidePP : MonoBehaviour
{
    MeshRenderer mr;

    // Start is called before the first frame update
    void Start()
    {
        mr = GetComponent<MeshRenderer>();

        mr.enabled = false;
    }

}
