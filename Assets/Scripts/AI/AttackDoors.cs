using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AttackDoors : MonoBehaviour
{
    private string[] tag_list = { "DoorX", "DoorZ"};
    RipperAIMain mainScript;

    private void Start()
    {
        mainScript = GetComponentInParent<RipperAIMain>();
    }

    private void OnTriggerEnter(Collider other)
    {
        foreach(string tags in tag_list)
        {
            if (other.gameObject.tag == tags)
            {
                if (!mainScript.IsScreaming)
                {
                    mainScript.doorTarget = other.gameObject;
                    mainScript.destroyObj = true;

                    Vector3 point1 = other.gameObject.transform.GetChild(2).position;
                    Vector3 point2 = other.gameObject.transform.GetChild(3).position;
                    Vector3 doorPoint = other.gameObject.transform.GetChild(4).position;
                    float dispt1 = Vector3.Distance(transform.position, point1);
                    float dispt2 = Vector3.Distance(transform.position, point2);
                    if (dispt1 < dispt2) mainScript.targetMoveObj = other.gameObject.transform.GetChild(2).gameObject;
                    if (dispt2 < dispt1) mainScript.targetMoveObj = other.gameObject.transform.GetChild(3).gameObject;
                    mainScript.targetRotateObj = other.gameObject.transform.GetChild(4).gameObject;
                }
            }
        }
        
        if (other.gameObject.tag == "warDoor") 
        {
            mainScript.doorTarget = other.gameObject.transform.parent.gameObject;
            mainScript.destroyObj = true;
        }
    }
}