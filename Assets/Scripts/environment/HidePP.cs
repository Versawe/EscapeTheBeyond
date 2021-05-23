using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
