using UnityEngine;
using System.Collections.Generic;

public class CameraRotationFirstPerson : MonoBehaviour
{
    private Camera cam;
    LookAtStuff lookScript;

    private float yawSensitivity;
    private float pitchSensitivity;
    //private float pitchSensitivity = 5f;

    private float yaw = 0.0f;
    private float pitch = 0.0f;
    private float pitch_clamped;

    Quaternion CameraRotation;
    Vector3 StartLocalPosition;
    private float bobPace = 8f;
    private float bobOffest = 0.03f;

    private float tiltPace = 4.5f; //8f
    private float tiltOffest = 0.65f; //1.75f

    Quaternion StartLocalRotation;
    Quaternion rotateQuat;
    CharacterMovementFirstPerson charMoveScript;

    //footstep audio
    public AudioSource stepSource;
    public List<AudioClip> stepClips = new List<AudioClip>();

    // Start is called before the first frame update
    void Awake()
    {
        lookScript = GetComponent<LookAtStuff>();
        stepSource = GetComponent<AudioSource>();
        //at start set the player's sensitivity
        yawSensitivity = NoDestroy.pSensitivity;
        //pitchSensitivity = yawSensitivity - 1.5f;
        pitchSensitivity = yawSensitivity; //test

        //starting rotation per level
        if (NoDestroy.gameProgression == 1 || NoDestroy.gameProgression == 4) yaw = 90;
        else yaw = -90;

        charMoveScript = GetComponentInParent<CharacterMovementFirstPerson>();
        StartLocalPosition = transform.localPosition;
        StartLocalRotation = transform.localRotation;

    }

    private void OnEnable()
    {
        stepSource.enabled = true;
    }
    private void OnDisable()
    {
        stepSource.Stop();
        stepSource.enabled = false;

        if (lookScript.lookingAtName != "Main_mirror") return;
        yaw = 90;
        pitch = 13;
    }

    void Update()
    {
        if (NoDestroy.gameProgression == 4)
        {
            Cursor.lockState = CursorLockMode.None;
            charMoveScript.enabled = false;
            enabled = false;
        }

        //up the tilting on running
        if (charMoveScript.IsSprinting)
        {
            tiltPace = 5.75f;
            tiltOffest = 0.90f;
        }
        else 
        {
            tiltPace = 4.5f;
            tiltOffest = 0.65f;
        } 

        SensitivityChanges(); //update in game
    }

    private void SensitivityChanges()
    {
        yawSensitivity = NoDestroy.pSensitivity;
        //pitchSensitivity = yawSensitivity - 1.5f;
        pitchSensitivity = yawSensitivity;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        if (NoDestroy.atGameComplete) stepSource.Stop();
        if (Time.timeScale == 0 || NoDestroy.atGameComplete) return;
        RotateCamera();

        if (!charMoveScript.isMoving) //this may need to be in LateUpdate
        {
            transform.localPosition = Vector3.MoveTowards(transform.localPosition, StartLocalPosition, 0.01f);
            transform.localRotation = Quaternion.RotateTowards(transform.localRotation, StartLocalRotation, 0.01f);
        }
    }

    private void RotateCamera()
    {
        if (Time.timeScale == 0) return;
        //saves axis movement of x and y mouse movement
        float mx = Input.GetAxis("Mouse X");
        float my = Input.GetAxis("Mouse Y");

        // yaw and pitch values change determined on 
        // mousex and mousey movement and applied sensitivity to both
        yaw += mx * yawSensitivity;
        pitch -= my * pitchSensitivity;

        //clamp pitch, so camera doesn't rotate too far low or high to seem weird
        pitch_clamped = Mathf.Clamp(pitch, -89f, 45f);

        CameraRotation = transform.rotation = Quaternion.Euler(pitch_clamped, yaw, 0);
        // use the clamped pitch and yaw to rotate camera rig, entered in as euler angles through Quaternion class
        if (Time.timeScale > 0 && !charMoveScript.isMoving) transform.rotation = Quaternion.RotateTowards(transform.rotation, CameraRotation, 0.01f);
        else if (Time.timeScale > 0 && charMoveScript.isMoving) 
        {
            CameraLocalSway();
        } 
    }
    private void CameraLocalSway()
    {
        float moveSway = Mathf.Sin(Time.time * bobPace) * bobOffest;
        Vector3 swayVec = new Vector3(StartLocalPosition.x, StartLocalPosition.y + moveSway, StartLocalPosition.z);
        transform.localPosition = Vector3.MoveTowards(transform.localPosition, swayVec, 0.1f);

        float rotateSway = Mathf.Sin(Time.time * tiltPace) * tiltOffest;
        rotateQuat = Quaternion.Euler(StartLocalRotation.x, StartLocalRotation.y, StartLocalRotation.z + rotateSway);
        transform.rotation = CameraRotation * rotateQuat;

        //footstep sound audio logic below
        if (!charMoveScript.IsSprinting) 
        {
            if (rotateSway >= 0.6475f)
            {
                PlayFootStep(0);
                //print("FootStep 1");
            }
            else if (rotateSway <= -0.6475f)
            {
                PlayFootStep(1);
                //print("FootStep 2");
            }
        }
        else 
        {
            if (rotateSway >= 0.8975f)
            {
                PlayFootStep(2);
                //print("sprintStep 1");
            }
            else if (rotateSway <= -0.8975f)
            {
                PlayFootStep(3);
                //print("sprintStep 2");
            }
        }

        if (Input.GetKeyDown("right shift")) PlayFootStep(3);
    }

    private void PlayFootStep(int clipNum) 
    {
        stepSource.clip = null;
        stepSource.clip = stepClips[clipNum];
        if (!stepSource.isPlaying) stepSource.Play();
    }
}
