using UnityEngine;

public class CharacterMovementFirstPerson : MonoBehaviour
{
    private CharacterController cc;

    private float playerSpeed = 5f;

    Vector3 inputDir;
    Vector3 copyInputVec;
    public bool isMoving = false;

    private float jumpSpeed = 3f;
    private float gravityWeight = 0f;
    Vector3 gravityDirection;

    private Vector3 moveDirection = Vector3.zero;

    public Transform cam;
    // Start is called before the first frame update
    void Start()
    {
        cc = GetComponent<CharacterController>();

    }

    // Update is called once per frame
    void Update()
    {
        if (Time.timeScale == 0 || NoDestroy.atGameComplete) return;
        MovePlayer();
        JumpPlayer();

        //applies input vector to apply to char controller
        Vector3 allMovementVectors = inputDir * playerSpeed + gravityDirection * gravityWeight + moveDirection;

        cc.Move(allMovementVectors * Time.deltaTime);
    }

    //movement for player
    private void MovePlayer()
    {

        //gets input from WASD using Unity's Vertical and Horizontal axis's
        float v = Input.GetAxisRaw("Vertical");
        float h = Input.GetAxisRaw("Horizontal");

        if (Mathf.Abs(h) > 0 && Mathf.Abs(v) > 0) playerSpeed = 3.5f;
        else playerSpeed = 5;

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
}
