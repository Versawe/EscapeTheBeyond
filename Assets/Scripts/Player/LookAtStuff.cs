using UnityEngine;

//This Script controls the Player's ability to look at objects and interact with them
//Creates a raycast from the camera's centerpoint
public class LookAtStuff : MonoBehaviour
{
    //Raycast variables
    private Ray pSight;
    RaycastHit pSeeOBJ;
    private float sightDistance = 2f;
    public string lookingAtName; //IMPORTANT VAR (used in other scripts)

    PlayerHiding hideScript; //used to check if player is hiding in wardrobe and always setting lookingAtName to that wardrobe

    GUIEvent guiEventScript; //used to see if object being looked at is an interactve object that starts a gui event
    GameObject InteractText;
    GameHUDActivations HUDScript;
    CharacterMovementFirstPerson CharMove;
    CameraRotationFirstPerson CamRotate;
    public bool IsActivated = false;

    Vector3 lockedOnMirror;
    Quaternion lookAtMirror;
    public bool IsInForms = false;

    GameObject CurrRelic;
    RelicHuntScript relicScript;

    private bool firstTime = false;

    private bool doOnce = false;

    public GameObject visualRaycastPoint;
    public GameObject pickupOBJ;
    AudioSource pickUpAS;

    Camera cam;

    private void Start()
    {
        //grabs and checks for the existance of GameObjects
        pickUpAS = pickupOBJ.GetComponent<AudioSource>();

        if (GameObject.Find("HidingCheck") != null) hideScript = GameObject.Find("HidingCheck").GetComponent<PlayerHiding>();
        else hideScript = null;

        if (GameObject.Find("GameHUD") != null)
        {
            HUDScript = GameObject.Find("GameHUD").GetComponent<GameHUDActivations>();
            visualRaycastPoint = GameObject.Find("LookPoint");
        }
        else
        {
            HUDScript = null;
            visualRaycastPoint = null;
        }

        cam = GetComponent<Camera>();

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
        CurrRelic = null;
    }
    // Update is called once per frame
    void Update()
    {
        //updates location and direction of raycast to where camera is looking
        UpdateRaycast();
        //sets looking at name variable depending on what player is looking at
        PlayerLookingAt();

        LockVisualPoint();

        //if (IsActivated) HUDScript.isPaused = false; //whenever you are in a gui event, you cannot pause

        //allow you to click enter instead of just clicking button, user's choice
        if (IsInForms && Input.GetKeyDown(KeyCode.Return) && HUDScript.formBar.text != "")
        {
            HUDScript.EnterPasscode();
        }

        if (NoDestroy.gameProgression == 2 && GameObject.Find("NoDestroyOBJ")) relicScript = GameObject.Find("NoDestroyOBJ").GetComponent<RelicHuntScript>();
        else relicScript = null;

        /*if (NoDestroy.gameProgression == 2 && lookingAtName != "" && lookingAtName.Substring(0,1) == "R") ActivateGUIEvent(lookingAtName);
        else InteractText.SetActive(false);*/

        //if (NoDestroy.gameProgression == 2) return;
        //checks if the object is interactive
        if (NoDestroy.puzzleOneLoginAttempts > 2 && lookingAtName != "" && !NoDestroy.completedQandA) ActivateGUIEvent(lookingAtName);
        else InteractText.SetActive(false);

        if (NoDestroy.completedQandA) //unlocks player's controls after beating game of QandA
        {
            CharMove.enabled = true;
            CamRotate.enabled = true;
        }

        if (Input.GetKeyDown("escape")) doOnce = false;
    }

    private void LockVisualPoint()
    {
        //Locks the visual UI point to the end of the raycast casting out of camera to show the user what the camera is aiming at
        //next 2 lines hide the visual point in certain scenarios
        if (HUDScript.isPaused || IsActivated || NoDestroy.atGameComplete || NoDestroy.atGameOver || NoDestroy.stairSpawnCount >= 5 || hideScript.isHiding || hideScript.inBounds || NoDestroy.BigScareHappening || NoDestroy.gameProgression == 4) visualRaycastPoint.SetActive(false);
        if (HUDScript.isPaused || IsActivated || NoDestroy.atGameComplete || NoDestroy.atGameOver || NoDestroy.stairSpawnCount >= 5 || hideScript.isHiding || hideScript.inBounds || NoDestroy.BigScareHappening || NoDestroy.gameProgression == 4) return;
        else visualRaycastPoint.SetActive(true);
        if (visualRaycastPoint && lookingAtName != "") //this constanlty updates the position of the circle
        {
                Vector3 screenPoint = cam.WorldToScreenPoint(pSeeOBJ.point);
                visualRaycastPoint.transform.position = screenPoint;
        }
    }

    //script that helps set isLookingAt to the name of the Gameobject that the player is looking at
    private void PlayerLookingAt()
    {
        if (IsActivated) return;
        // statement to determine if you are in a wardrobe, or not. This helps set the lookingAtName to a wardrobe you are in
        //this was added so you do not need to look at the wardrobe to open and close it when inside it (better for gameplay)
        if (hideScript != null) // this if & else was used for when the player is not in the RelicHunt Scene. if = RelicHunt, else = Not
        {
            if (hideScript.inBounds && hideScript.wardrobeHiding != null)
            {
                lookingAtName = hideScript.wardrobeHiding.name;
            }
            else // else the lookingAtName will be the name of the GameObject the player is looking at (only used for doors & wardrobes currently)
            {
                if (Physics.Raycast(pSight, out pSeeOBJ, sightDistance)) //what is the player looking at???
                {
                    if (pSeeOBJ.collider == null)
                    {
                        lookingAtName = "";
                    }
                    //for wardrobes and door's name to get added to lookingAtName
                    if (pSeeOBJ.collider.tag == "DoorX" || pSeeOBJ.collider.tag == "DoorZ")
                    {
                        lookingAtName = pSeeOBJ.collider.gameObject.name;
                    }
                    else if (pSeeOBJ.collider.tag == "mirror") //excluded from relic hunt scene for now
                    {
                        lookingAtName = pSeeOBJ.collider.gameObject.name;
                    }
                    else if (pSeeOBJ.collider.tag == "warDoor")
                    {
                        lookingAtName = pSeeOBJ.collider.gameObject.transform.parent.name;
                    }
                    else if (pSeeOBJ.collider.tag == "relic") //for collectable relics
                    {
                        lookingAtName = pSeeOBJ.collider.gameObject.name;
                        CurrRelic = pSeeOBJ.collider.gameObject;
                    }
                    else if (pSeeOBJ.collider.tag == "Hint") //for notepad
                    {
                        lookingAtName = pSeeOBJ.collider.gameObject.name;
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
            if (Physics.Raycast(pSight, out pSeeOBJ, sightDistance)) //what is the player looking at???
            {
                if (pSeeOBJ.collider == null)
                {
                    lookingAtName = "";
                }
                //for wardrobes and door's name to get added to lookingAtName
                if (pSeeOBJ.collider.tag == "DoorX" || pSeeOBJ.collider.tag == "DoorZ")
                {
                    lookingAtName = pSeeOBJ.collider.gameObject.name;
                }
                else if (pSeeOBJ.collider.tag == "warDoor")
                {
                    lookingAtName = pSeeOBJ.collider.gameObject.transform.parent.name;
                }
                else if(pSeeOBJ.collider.tag == "Hint") //for notepad
                {
                    lookingAtName = pSeeOBJ.collider.gameObject.name;
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

    //This Function lookes for the raycast hitting any Gameobjects with a GUIEVENTNAME script attached to it and shows the 'e to interact' text
    private void ActivateGUIEvent(string GUIEventName)
    {
        //print(GUIEventName);
        if (GameObject.Find(GUIEventName) && !HUDScript.isPaused)
        {
            GameObject GUIEventOBJ = GameObject.Find(GUIEventName);
            if (GUIEventOBJ.GetComponent<GUIEvent>().ignoreMe) return;
            lookingAtName = GUIEventName;
            if (GUIEventOBJ.GetComponent<GUIEvent>())
            {
                guiEventScript = GUIEventOBJ.GetComponent<GUIEvent>();
                InteractText.SetActive(true);
            }
            else
            {
                guiEventScript = null;
                CurrRelic = null;
                InteractText.SetActive(false);
            }

            if (GUIEventName == "Main_mirror")
            {
                lockedOnMirror = new Vector3(GUIEventOBJ.transform.position.x - 2, GUIEventOBJ.transform.position.y - 0.35f, GUIEventOBJ.transform.position.z);
                Vector3 dir = GUIEventOBJ.transform.position - transform.position;
                lookAtMirror = Quaternion.LookRotation(dir, Vector3.up);
            }
            InteractiveInput();
        }
        else
        {
            guiEventScript = null;
            CurrRelic = null;
            InteractText.SetActive(false);
        }
    }

    //This Function determines what is to happen when the player is looking at something and clicks 'e'
    private void InteractiveInput()
    {
        //toggles when e was pressed and unpressed
        bool activate = Input.GetKeyDown("e");
        bool deactivate = Input.GetKeyDown("escape");
        if (activate && !IsActivated && lookingAtName.Substring(0, 1) != "R")
        {
            IsActivated = true;
        }
        else if (deactivate && IsActivated && lookingAtName != "Main_mirror")
        {
            IsActivated = false;
        }
        else if (activate && !IsActivated && lookingAtName.Substring(0, 1) == "R")
        {
            CollectRelic();
        }
        else if (activate && !IsActivated && lookingAtName.Substring(0,1) == "N") 
        {
            IsActivated = true;
        }
        else if (deactivate && IsActivated && lookingAtName.Substring(0, 1) == "N")
        {
            IsActivated = false;
        }

        //what happens when successfully activated or deactivated
        if (IsActivated)
        {
            if (lookingAtName == "Main_mirror" && !NoDestroy.completedQandA) //if you are looking at the mirror for puzzle 3
            {
                Cursor.lockState = CursorLockMode.None;
                CharMove.enabled = false;
                CamRotate.enabled = false;
                //HUDScript.isPaused = false;
                InteractText.SetActive(false);
                HUDScript.Puzzle3Script.enabled = true;

                //slides player into position for QandA game
                Vector3 SlideVect = Vector3.MoveTowards(transform.parent.position, lockedOnMirror, 8f * Time.deltaTime);
                transform.parent.position = SlideVect;
                Quaternion EaseRotation = Quaternion.RotateTowards(transform.rotation, lookAtMirror, 180f * Time.deltaTime);
                transform.rotation = EaseRotation;
            }
            else if (lookingAtName == "Notepad" && !NoDestroy.atGameOver) //lets player read note
            {
                HUDScript.NotePadHintPanel.SetActive(true);
                CharMove.enabled = false;
                CamRotate.enabled = false;
                AudioController.PlayFlashBackSound(100);
            }
            else //if you are looking at the door with the code
            {
                if (!firstTime) // play dialogue audio clip for first attempt at opeing door with passcode
                {
                    AudioController.PlayDialogueSound(1);
                    firstTime = true;
                }
                if (!doOnce) 
                {
                    AudioController.ClickSound();
                    doOnce = true;
                }
                Cursor.lockState = CursorLockMode.None;
                CharMove.enabled = false;
                CamRotate.enabled = false;
                HUDScript.PasscodePanel.SetActive(true);
                IsInForms = true;
                HUDScript.formBar.ActivateInputField();
                HUDScript.isPaused = false;
                InteractText.SetActive(false);
            }
        }
        else
        {
            Cursor.lockState = CursorLockMode.Locked; //exiting out of isactivated
            CharMove.enabled = true;
            CamRotate.enabled = true;
            HUDScript.formBar.text = "";
            IsInForms = false;
            HUDScript.PasscodePanel.SetActive(false);
            HUDScript.NotePadHintPanel.SetActive(false);
            HUDScript.Puzzle3Script.enabled = false;
            InteractText.SetActive(true);
        }
    }

    private void CollectRelic() //function called when player collects a relic in the relic hunt scene
    {
        pickUpAS.Play();
        Destroy(CurrRelic);
        CurrRelic = null;
        HUDScript.relicCollected++;
        AudioController.PlayFlashBackSound(70); //chance at giving a dialogue hint..
        if (HUDScript.relicCollected == 1 && AudioController.DialogueSource.isPlaying) AudioController.StopSound();
        if (HUDScript.relicCollected == 1 && !AudioController.DialogueSource.isPlaying) AudioController.PlayDialogueSound(4);
        if (HUDScript.relicCollected == 5 || HUDScript.relicCollected == 10) relicScript.SpawnAI();
        if (HUDScript.relicCollected >= 15)
        {
            NoDestroy.collectedAllRelics = true;
            NoDestroy.currObjective = "Current Objective:\nYou have the crafted key with the relics you collect\nGo back to locked door, quick!";
            if (AudioController.DialogueSource.isPlaying) AudioController.StopSound(); //if an audio hint is playing it will be interuptted
            AudioController.PlayDialogueSound(5); //protagonist tells player to get back to the door
        } 
    }
}
