using UnityEngine;

public class CharacterMovementFirstPerson : MonoBehaviour
{
    private CharacterController cc;

    private float playerSpeedConst = 3.5f; //3.5f
    private float playerSpeed;
    
    private float playerSprintSpeedConst = 4.5f; //4.5f
    private float playerSprintSpeed;

    Vector3 inputDir;
    Vector3 copyInputVec;
    public bool isMoving = false;

    private float jumpSpeed = 3f;
    private float gravityWeight = 0f;
    Vector3 gravityDirection;

    private Vector3 moveDirection = Vector3.zero;
    private Vector3 allMovementVectors;

    public Transform cam;
    PlayerHiding hideScript;

    public bool IsSprinting = false;
    public float staminaActual;
    private float staminaMax = 3.5f;

    public bool IsExhausted = false;
    private float exhaustedTimer = 3f;
    public static bool IsBreathingHeavy = false;
   
    // Start is called before the first frame update
    void Start()
    {
        IsBreathingHeavy = false;
        cc = GetComponent<CharacterController>();
        hideScript = GetComponentInChildren<PlayerHiding>();

        playerSpeed = playerSpeedConst;
        playerSprintSpeed = playerSprintSpeedConst;
        staminaActual = staminaMax;
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.timeScale == 0 || NoDestroy.atGameComplete) return;
        if (hideScript.isHiding) LockHidingPlayer();
        StaminaManagement();
        if (hideScript.isHiding) return;
        MovePlayer();
        JumpPlayer();

        //applies input vector to apply to char controller
        if(!IsSprinting) allMovementVectors = inputDir * playerSpeed + gravityDirection * gravityWeight + moveDirection;
        if(IsSprinting && ! IsExhausted) allMovementVectors = inputDir * playerSprintSpeed + gravityDirection * gravityWeight + moveDirection;

        cc.Move(allMovementVectors * Time.deltaTime);

        if (IsExhausted && !AudioController.DialogueSource.isPlaying && !IsBreathingHeavy) 
        {
            AudioController.PlayDialogueSound(12);
            IsBreathingHeavy = true;
            AudioController.DialogueSource.volume = 0.5f;
        }
        if (IsBreathingHeavy && !IsExhausted && AudioController.DialogueSource.isPlaying) 
        {
            if(AudioController.DialogueSource.clip.name == "breathing") 
            {
                AudioController.StopSound();
                IsBreathingHeavy = false;
                AudioController.DialogueSource.volume = 1f;
            }
        }
        if (!AudioController.DialogueSource.isPlaying) 
        {
            AudioController.DialogueSource.volume = 1f;
            IsBreathingHeavy = false;
        } 
    }

    //movement for player
    private void MovePlayer()
    {

        //gets input from WASD using Unity's Vertical and Horizontal axis's
        float v = Input.GetAxisRaw("Vertical");
        float h = Input.GetAxisRaw("Horizontal");

        if (Mathf.Abs(h) > 0 && Mathf.Abs(v) > 0) 
        {
            playerSpeed = playerSpeedConst / 1.5f;
            playerSprintSpeed = playerSprintSpeedConst / 1.5f;
        } 
        else 
        {
            playerSpeed = playerSpeedConst;
            playerSprintSpeed = playerSprintSpeedConst;
        } 

        //applies this gameobjects forward and right vector to a vector 3 depending on the axis's numbers
        // Example: w = 1, s = -1, a = -1, d = 1

        inputDir = transform.forward * v + transform.right * h;

        //skidding functionality
        //maybe in the future

        //this statement rotates the character to be facing away from camera if movement is applied
        //CHANGED FOR FPS   
        Quaternion camRot = Quaternion.Euler(0, cam.eulerAngles.y, 0);
        transform.rotation = camRot;

        if (h != 0 || v != 0)
        {
            isMoving = true;
        }
        else isMoving = false;
    }

    //player jump ability
    private void JumpPlayer()
    {
        //gravity appylied
        gravityDirection = Vector3.down;

        if (!cc.isGrounded)
        {
            gravityWeight += 25f * Time.deltaTime;
        }

        //checks if spacebar clicked and only triggable once
        if (cc.isGrounded && Input.GetButtonDown("Jump"))
        {
            moveDirection.y = jumpSpeed;
            gravityWeight = 0;
        }
    }

    private void LockHidingPlayer() 
    {
        if (hideScript.lockedLocation) 
        {
            //transform.position = hideScript.lockedLocation.position;
            transform.position = Vector3.MoveTowards(transform.position, hideScript.lockedLocation.position, 0.1f);
        }
    }

    private void StaminaManagement() 
    {
        //is holding down shift
        bool IsHolding = Input.GetKey("left shift");

        //controlling stamina meter
        if (IsHolding && isMoving && !hideScript.isHiding) 
        {
            staminaActual -= 1 * Time.deltaTime;
            IsSprinting = true;
        }
        else //letting go of shift helps regain stamina and switches to not sprinting
        {
            if(!IsExhausted) staminaActual += 0.5f * Time.deltaTime;
            IsSprinting = false;
        }

        //not letting stam go above and below a certain amt
        if (staminaActual >= staminaMax) staminaActual = staminaMax;
        if (staminaActual <= 0) //no stam = not sprinting
        {
            IsSprinting = false;
            IsExhausted = true;
            staminaActual = 0;
        } 

        //is sprinting
        if (staminaActual > 0 && IsHolding) IsSprinting = true;

        if (IsExhausted) exhaustedTimer -= 1 * Time.deltaTime;

        if(exhaustedTimer <= 0) 
        {
            IsExhausted = false;
            exhaustedTimer = 3f;
        }
    }
}
