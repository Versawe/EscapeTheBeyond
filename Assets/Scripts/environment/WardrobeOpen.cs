using UnityEngine;

//controls the wardrobes function of opening and it's audio
public class WardrobeOpen : MonoBehaviour
{
    public GameObject thisDoor;
    public GameObject rightDoor;
    public GameObject leftDoor;
    public bool isOpen = false;
    public bool isShutting = false; // var made so when you slam the door on the AI, it doesn't phase through the door Might not be needed for wardrobe

    //private bool eDown = false; //seems to be not needed keep for now

    public bool doOnce = false;
    private float doOnceTimer = 0.5f;

    private float swingSpeed = 0.001f;

    //private float doorRotationValue = 90;
    public GameObject Player;
    LookAtStuff seeDoorScript;

    Quaternion doorLeftOpen;
    Quaternion doorRightOpen;

    public Quaternion doorStartRotRight;
    public Quaternion doorStartRotLeft;

    //audio vars
    AudioSource DoorAudioSource;
    private bool thisDoorWasNoise = false;
    public AudioClip doorOpenClip;
    public AudioClip doorCloseClip;
    bool playOnce = false;

    // Start is called before the first frame update
    void Awake()
    {
        Player = GameObject.Find("FPSController");

        DoorAudioSource = GetComponent<AudioSource>();
        seeDoorScript = Player.GetComponentInChildren<LookAtStuff>();
    }

    private void Start()
    {

        doorLeftOpen = Quaternion.Euler(0, 90f, 0);
        doorRightOpen = Quaternion.Euler(0, -90f, 0);

        isOpen = false;
    }
    // Update is called once per frame
    void Update()
    {
        playerInput();
        doorMove();

        //stops door audio on pause and resumes on unpause
        if (DoorAudioSource.isPlaying && Time.timeScale == 0) PauseAudio();
        if (!DoorAudioSource.isPlaying && thisDoorWasNoise && Time.timeScale == 1) UnPauseAudio();

    }

    private void playerInput()
    {
        //input trigger door open
        if (Input.GetKeyDown("e") && seeDoorScript.lookingAtName == gameObject.name)
        {
            //eDown = true;
            doOnce = true;
        }
        if (Input.GetKeyUp("e"))
        {
            //eDown = false;
        }
    }

    private void doorMove()
    {
        //both wardrobe doors open and close
        if (isOpen && doOnce) // close animation and variable sets
        {
            isShutting = true;
            if (!playOnce) PlayAudioOnce(doorCloseClip);
            leftDoor.transform.localRotation = Slide(leftDoor.transform.localRotation, doorStartRotLeft, swingSpeed);
            rightDoor.transform.localRotation = Slide(rightDoor.transform.localRotation, doorStartRotRight, swingSpeed);
            doOnceTimer -= Time.deltaTime;
            if (doOnceTimer <= 0)
            {
                isShutting = false;
                isOpen = false;
                doOnce = false;
                doOnceTimer = 0.5f;
                playOnce = false;
            }
        }
        if (!isOpen && doOnce) // open animation and variable sets
        {
            if (!playOnce) PlayAudioOnce(doorOpenClip);
            leftDoor.transform.localRotation = Slide(leftDoor.transform.localRotation, doorLeftOpen, swingSpeed);
            rightDoor.transform.localRotation = Slide(rightDoor.transform.localRotation, doorRightOpen, swingSpeed);
            doOnceTimer -= Time.deltaTime;
            if (doOnceTimer <= 0)
            {
                isOpen = true;
                doOnce = false;
                doOnceTimer = 0.5f;
                playOnce = false;
            }
        }
    }

    //slide function for smooth and realiabe rotation animations
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
