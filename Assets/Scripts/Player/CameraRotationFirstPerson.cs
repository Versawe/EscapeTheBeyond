using UnityEngine;

public class CameraRotationFirstPerson : MonoBehaviour
{
    private Camera cam;
    LookAtStuff lookScript;

    private float yawSensitivity = 5f;
    private float pitchSensitivity = 3.5f;
    //private float pitchSensitivity = 5f;

    private float yaw = 0.0f;
    private float pitch = 0.0f;
    private float pitch_clamped;

    Quaternion CameraRotation;
    Vector3 StartLocalPosition;
    private float bobPace = 8f;
    private float bobOffest = 0.03f;

    private float tiltPace = 8f; //3.5f
    private float tiltOffest = 1.75f; //1.75f

    Quaternion StartLocalRotation;
    Quaternion rotateQuat;
    CharacterMovementFirstPerson charMoveScript;

    // Start is called before the first frame update
    void Start()
    {
        lookScript = GetComponent<LookAtStuff>();

        //at start set the player's sensitivity
        yawSensitivity = NoDestroy.pSensitivity;

        //starting rotation per level
        if (NoDestroy.gameProgression == 1) yaw = 90;
        else yaw = -90;

        charMoveScript = GetComponentInParent<CharacterMovementFirstPerson>();
        StartLocalPosition = transform.localPosition;
        StartLocalRotation = transform.localRotation;
        
    }

    private void OnDisable()
    {
        if (lookScript.lookingAtName != "Main_mirror") return;
        yaw = 90;
        pitch = 13;
    }

    void Update()
    {
        SensitivityChanges(); //update in game
    }

    private void SensitivityChanges()
    {
        yawSensitivity = NoDestroy.pSensitivity;
        pitchSensitivity = yawSensitivity - 1.5f;
        //pitchSensitivity = yawSensitivity;
    }

    // Update is called once per frame
    void LateUpdate()
    {
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
    }
}
