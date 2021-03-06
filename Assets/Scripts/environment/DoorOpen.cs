using System;
using UnityEngine;

//controls the functionality of opening and closing doors
public class DoorOpen : MonoBehaviour
{
    public GameObject thisDoor;
    public bool isOpen = false;
    public bool isShutting = false; // var made so when you slam the door on the AI, it doesn't phase through the door

    //private bool eDown = false; //may not be needed, keep for now

    private bool doOnce = false;
    private float doOnceTimer = 0.5f;

    private float swingSpeed = 0.001f;

    public GameObject Player;
    LookAtStuff seeDoorScript;
    public Quaternion doorStartRot;

    Quaternion doorXOpen1;
    Quaternion doorXOpen2;

    Quaternion doorZOpen1;
    Quaternion doorZOpen2;

    private string side = "";

    LockedDoor doorLocked;

    AudioSource DoorAudioSource;
    private bool thisDoorWasNoise = false;
    public AudioClip doorOpenClip;
    public AudioClip doorOpenClip2;
    public AudioClip doorCloseClip;
    public AudioClip doorLockedClip;
    bool playOnce = false;

    private bool triggerD = false;

    // Start is called before the first frame update
    void Awake()
    {
        //get objects & components
        Player = GameObject.Find("FPSController");

        DoorAudioSource = GetComponent<AudioSource>();
        seeDoorScript = Player.GetComponentInChildren<LookAtStuff>();

        doorLocked = GetComponent<LockedDoor>();
    }

    private void Start()
    {
        //starting door values
        doorXOpen1 = Quaternion.Euler(0f, 90f, 0f); // player.x greater
        doorXOpen2 = Quaternion.Euler(0f, 270f, 0f);

        doorZOpen1 = Quaternion.Euler(0f, 0f, 0f); //player.x greater
        doorZOpen2 = Quaternion.Euler(0f, -180f, 0f);

        isOpen = false;
    }
    // Update is called once per frame
    void Update()
    {
        if (doorLocked.TriggerDialogue) TriggerDialogue(); //triggers dialogue
        if (Input.GetKeyDown("e") && seeDoorScript.lookingAtName == gameObject.name && !seeDoorScript.IsInForms) //plays locked door clip
        {
            if (!DoorAudioSource.isPlaying) playOnce = false;
            if(doorLocked.IsLocked || doorLocked.TriggerDialogue || doorLocked.Puzzle2Trigger) if(!playOnce) PlayAudioOnce(doorLockedClip);
        }

        //stops door audio on pause and resumes on unpause
        if (DoorAudioSource.isPlaying && Time.timeScale == 0) PauseAudio();
        if (!DoorAudioSource.isPlaying && thisDoorWasNoise && Time.timeScale == 1) UnPauseAudio();

        if (doorLocked.IsLocked) return; //functions below open a door if it is not locked
        playerInput();
        if (doorLocked.Puzzle2Trigger && !NoDestroy.collectedAllRelics) return;
        whichSide();
        doorMove();
    }

    private void TriggerDialogue()
    {
        if (!triggerD && Input.GetKeyDown("e") && seeDoorScript.lookingAtName == gameObject.name)
        {
            // triggers audio clip for beginning of QandA scene
            AudioController.PlayDialogueSound(6);
            triggerD = true;
        }
    }

    private void playerInput()
    {
        //eDown = Input.GetKeyDown("e");
        if (Input.GetKeyDown("e") && seeDoorScript.lookingAtName == gameObject.name && !doorLocked.Puzzle2Trigger) //this opens the door through input
        {
            //eDown = true;
            doOnce = true;
        }
        else if (Input.GetKeyUp("e") && seeDoorScript.lookingAtName == gameObject.name && doorLocked.Puzzle2Trigger && !doorLocked.WasFirstOpenedTriggered && !NoDestroy.collectedAllRelics) //this is to trigger puzzle 2
        {
            GameObject thisOBJ = GameObject.Find("NoDestroyOBJ");
            thisOBJ.GetComponent<NoDestroy>().huntScript.enabled = true;
            thisOBJ.GetComponent<NoDestroy>().jumpScareScript.enabled = false;
            doorLocked.WasFirstOpenedTriggered = true;
        }
        else if (Input.GetKeyUp("e") && seeDoorScript.lookingAtName == gameObject.name && doorLocked.Puzzle2Trigger && doorLocked.WasFirstOpenedTriggered && NoDestroy.collectedAllRelics) 
        {
            NoDestroy.LoadQandAScene();
        }
    }

    //check which side on what door you are on when you open it
    private void whichSide()
    {
        if (thisDoor.gameObject.tag == "DoorX") 
        {
            if (Player.transform.position.z > transform.position.z)
            {
                if (!doOnce) side = "x2";
            }
            else
            {
                if (!doOnce) side = "x1";

            }
        }
        
        if (thisDoor.gameObject.tag == "DoorZ")
        {
            if (Player.transform.position.x > transform.position.x)
            {
                if (!doOnce) side = "z1";
            }
            else
            {
                if (!doOnce) side = "z2";
            }
        }
    }

    private void doorMove()
    {
        //door x opens and closes
        if (thisDoor.gameObject.tag == "DoorX" && isOpen && doOnce)
        {
            isShutting = true;
            if (!playOnce) PlayAudioOnce(doorCloseClip);
            transform.rotation = Slide(transform.rotation, doorStartRot, swingSpeed);
            doOnceTimer -= Time.deltaTime;
            if (doOnceTimer <= 0)
            {
                JustShut();
            }
        }
        if (thisDoor.gameObject.tag == "DoorX" && !isOpen && doOnce)
        {
            if (!playOnce) PlayAudioOnce(doorOpenClip);
            if (side == "x2")
            {
                transform.rotation = Slide(transform.rotation, doorXOpen2, swingSpeed);
                doOnceTimer -= Time.deltaTime;
                if (doOnceTimer <= 0)
                {
                    JustOpened();
                }
            }
            else if(side == "x1")
            {
                transform.rotation = Slide(transform.rotation, doorXOpen1, swingSpeed);
                doOnceTimer -= Time.deltaTime;
                if (doOnceTimer <= 0)
                {
                    JustOpened();
                }
            }
        }
        //door z's opens and closes
        if (thisDoor.gameObject.tag == "DoorZ" && isOpen && doOnce) // rotate to close position
        {
            isShutting = true;
            if (!playOnce) PlayAudioOnce(doorCloseClip);
            transform.rotation = Slide(transform.rotation, doorStartRot, swingSpeed);
            doOnceTimer -= Time.deltaTime;
            if (doOnceTimer <= 0)
            {
                JustShut();
            }
        }
        if (thisDoor.gameObject.tag == "DoorZ" && !isOpen && doOnce) //Opening door
        {
            if (!playOnce) PlayAudioOnce(doorOpenClip2);
            if (side == "z1") //open if one sideA of door
            {
                transform.rotation = Slide(transform.rotation, doorZOpen1, swingSpeed);
                doOnceTimer -= Time.deltaTime;
                if (doOnceTimer <= 0)
                {
                    JustOpened();
                }
            }
            else if(side == "z2") //open if one sideB of door
            {
                transform.rotation = Slide(transform.rotation, doorZOpen2, swingSpeed);
                doOnceTimer -= Time.deltaTime;
                if (doOnceTimer <= 0)
                {
                    JustOpened();
                }
            }
        }
    }

    private void JustOpened() //vars to adjust when door was just opened
    {
        isOpen = true;
        doOnce = false;
        playOnce = false;
        doOnceTimer = 0.5f;
    }

    private void JustShut() //vars to adjust when door was just shut
    {
        isOpen = false;
        isShutting = false;
        doOnce = false;
        playOnce = false;
        doOnceTimer = 0.5f;
    }

    //smooth rotate
    public Quaternion Slide(Quaternion current, Quaternion target, float percentLeft = 0.5f)
    {
        float p = 1 - Mathf.Pow(percentLeft, Time.deltaTime);
        return Quaternion.Lerp(current, target, p);
    }

    //door audio clip code below
    private void PlayAudioOnce(AudioClip thisClip)
    {
        DoorAudioSource.clip = thisClip;
        DoorAudioSource.Play();
        playOnce = true;
    }

    private void PauseAudio() 
    {
        if (DoorAudioSource.isPlaying) DoorAudioSource.Pause();
        thisDoorWasNoise = true;
    }

    private void UnPauseAudio()
    {
        if (!DoorAudioSource.isPlaying && thisDoorWasNoise) 
        {
            DoorAudioSource.Play();
        }
        thisDoorWasNoise = false;
    }
}
