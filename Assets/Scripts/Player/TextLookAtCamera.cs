using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TextLookAtCamera : MonoBehaviour
{
    MeshRenderer mr;
    public TextMeshPro tm;

    public GameObject playerCamera;
    public GameObject ripper;

    AIMain AIscript;

    // Start is called before the first frame update
    void Start()
    {
        playerCamera = GameObject.Find("FirstPersonCharacter");
        AIscript = GetComponentInParent<AIMain>();
        mr = GetComponent<MeshRenderer>();
        mr.enabled = true;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 relativePos = playerCamera.transform.position - transform.position;

        Quaternion rotation = UnityEngine.Quaternion.LookRotation(-relativePos, Vector3.up);

        transform.rotation = rotation;

        if (AIscript.aiState == "Patrol" && !AIscript.IsScreaming || AIscript.aiState == "Chase" || AIscript.aiState == "ChasePlus" || AIscript.aiState == "Track" || AIscript.aiState == "Search" || AIscript.aiState == "Still")
        {
            tm.text = AIscript.aiState;
        }else if (AIscript.aiState == "Patrol" && AIscript.IsScreaming)
        {
            tm.text = "Screaming";
        } 
    }
}
