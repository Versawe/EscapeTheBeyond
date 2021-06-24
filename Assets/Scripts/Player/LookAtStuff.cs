using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LookAtStuff : MonoBehaviour
{
    //Raycast variables
    private Ray pSight;
    RaycastHit pSeeDoor;
    private float sightDistance = 2f;
    public string lookingAtName; //IMPORTANT VAR (used in other scripts)

    PlayerHiding hideScript; //used to check if player is hiding in wardrobe and always setting lookingAtName to that wardrobe

    GUIEvent guiEventScript; //used to see if object being looked at is an interactve obeject that starts a gui event
    GameObject InteractText;
    GameHUDActivations HUDScript;
    CharacterMovementFirstPerson CharMove;
    CameraRotationFirstPerson CamRotate;
    public bool IsActivated = false;

    private void Start()
    {
        if (GameObject.Find("HidingCheck") != null) hideScript = GameObject.Find("HidingCheck").GetComponent<PlayerHiding>();
        else hideScript = null;

        if (GameObject.Find("GameHUD") != null) HUDScript = GameObject.Find("GameHUD").GetComponent<GameHUDActivations>();
        else HUDScript = null;

        if (GetComponentInParent<CharacterMovementFirstPerson>()) CharMove = GetComponentInParent<CharacterMovementFirstPerson>();
        else CharMove = null;

        if (GetComponent<CameraRotationFirstPerson>()) CamRotate = GetComponent<CameraRotationFirstPerson>();
        else CamRotate = null;

        if (GameObject.Find("InteractiveText"))
        {
            InteractText = GameObject.Find("InteractiveText");
            InteractText.SetActive(false);
        }
        else InteractText = null;

        //null from start
        guiEventScript = null;
    }
    // Update is called once per frame
    void Update()
    {
        //updates location and direction of raycast to where camera is looking
        UpdateRaycast();
        //sets looking at name variable depending on what player is looking at
        PlayerLookingAt();

        //checks if the object is interactive
        if(NoDestroy.puzzleOneLoginAttempts > 2 && NoDestroy.gameProgression == 1) ActivateGUIEvent(lookingAtName);
    }

    private void PlayerLookingAt()
    {
        // statement to determine if you are in a wardrobe, or not. This helps set the lookingAtName to a wardrobe you are in
        //this was added so you do not need to look at the wardrobe to open and close it when inside it (better for gameplay)
        if (hideScript != null) // this if & else was used for when the player is not in the RelicHunt Scene
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
                    else //if not looking at any interactive thing for insurance
                    {
                        lookingAtName = "";
                    }
                }
                else //if not hitting anything at all
                {
                    lookingAtName = "";
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
                else //if not looking at any interactive thing for insurance
                {
                    lookingAtName = "";
                }
            }
            else //if not hitting anything at all
            {
                lookingAtName = "";
            }
        }

        //draws in editor for reference
        Debug.DrawRay(pSight.origin, pSight.direction, Color.green);
    }

    private void UpdateRaycast()
    {
        //set orgin and direction of raycast
        //origin is camera location, direction is forward vector, so where the camera is facing
        pSight.origin = transform.position;
        pSight.direction = transform.forward;
    }

    private void ActivateGUIEvent(string GUIEventName)
    {
        if (GameObject.Find(GUIEventName) && lookingAtName != "" && !HUDScript.isPaused)
        {
            GameObject GUIEventOBJ = GameObject.Find(GUIEventName);

            if (GUIEventOBJ.GetComponent<GUIEvent>())
            {
                guiEventScript = GUIEventOBJ.GetComponent<GUIEvent>();
                InteractText.SetActive(true);
            }
            else
            {
                guiEventScript = null;
                InteractText.SetActive(false);
            }

            InteractiveInput();
        }
        else
        {
            guiEventScript = null;
            InteractText.SetActive(false);
        }
    }

    private void InteractiveInput()
    {
        //toggles when e was pressed and unpressed
        bool activate = Input.GetKeyDown("e");
        if (activate && !IsActivated)
        {
            IsActivated = true;
        }
        else if (activate && IsActivated)
        {
            IsActivated = false;
        }

        //what happens when successfully activated or deactivated
        if (IsActivated)
        {
            Cursor.lockState = CursorLockMode.None;
            CharMove.enabled = false;
            CamRotate.enabled = false;
            HUDScript.PasscodePanel.SetActive(true);
            HUDScript.isPaused = false;
            InteractText.SetActive(false);
        }
        else
        {
            Cursor.lockState = CursorLockMode.Locked;
            CharMove.enabled = true;
            CamRotate.enabled = true;
            HUDScript.PasscodePanel.SetActive(false);
            InteractText.SetActive(true);
        }
    }
}
