using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAtDoor : MonoBehaviour
{

    private Ray pSight;
    RaycastHit pSeeDoor;

    private float sightDistance = 2f;
    public string lookingAtName;

    PlayerHiding hideScript;


    private void Start()
    {
        if (GameObject.Find("HidingCheck") != null) hideScript = GameObject.Find("HidingCheck").GetComponent<PlayerHiding>();
        else hideScript = null;
    }
    // Update is called once per frame
    void Update()
    {
        //set orgin and direction of raycast
        //origin is camera location, direction is forward vector, so where the camera is facing
        pSight.origin = transform.position;
        pSight.direction = transform.forward;

        //draws in editor for reference
        Debug.DrawRay(pSight.origin, pSight.direction, Color.green);

        // statement to determine if you are in a wardrobe, or not. This helps set the lookingAtName to a wardrobe you are in
        //this was added so you do not need to look at the wardrobe to open and close it when inside it (better for gameplay)
        if(hideScript != null) // this if & else was used for when the player is not in the RelicHunt Scene
        {
            if (hideScript.inBounds && hideScript.wardrobeHiding != null)
            {
                lookingAtName = hideScript.wardrobeHiding.name;
            }
            else // else the lookingAtName will be the name of the GameObject the player is looking at (only used for doors & wardrobes currently)
            {
                if (Physics.Raycast(pSight, out pSeeDoor, sightDistance)) //what is the player looking at???
                {
                    if (pSeeDoor.collider == null)
                    {
                        lookingAtName = "";
                    }
                    //for wardrobes and door's name to get added to lookingAtName
                    if (pSeeDoor.collider.tag == "DoorX" || pSeeDoor.collider.tag == "DoorZ")
                    {
                        lookingAtName = pSeeDoor.collider.gameObject.name;
                    }
                    else if (pSeeDoor.collider.tag == "warDoor")
                    {
                        lookingAtName = pSeeDoor.collider.gameObject.transform.parent.name;
                    }
                    else //if not looking at any interactive thing
                    {
                        lookingAtName = "";
                    }
                }
            }
        }
        else
        {
            if (Physics.Raycast(pSight, out pSeeDoor, sightDistance)) //what is the player looking at???
            {
                if (pSeeDoor.collider == null)
                {
                    lookingAtName = "";
                }
                //for wardrobes and door's name to get added to lookingAtName
                if (pSeeDoor.collider.tag == "DoorX" || pSeeDoor.collider.tag == "DoorZ")
                {
                    lookingAtName = pSeeDoor.collider.gameObject.name;
                }
                else if (pSeeDoor.collider.tag == "warDoor")
                {
                    lookingAtName = pSeeDoor.collider.gameObject.transform.parent.name;
                }
                else //if not looking at any interactive thing
                {
                    lookingAtName = "";
                }
            }
        }
        

        //print(lookingAtName);
    }
}
