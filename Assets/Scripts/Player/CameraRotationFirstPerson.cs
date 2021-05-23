using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraRotationFirstPerson : MonoBehaviour
{
    private Camera cam;

    private float yawSensitivity = 4.5f;
    private float pitchSensitivity = 2.5f;


    private float yaw = 0.0f;
    private float pitch = 0.0f;

    //public Transform target;

    private GameObject player;

    // Start is called before the first frame update
    void Start()
    {

        player = GameObject.Find("Player");

        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void LateUpdate()
    {

        RotateCamera();
    }

    private void RotateCamera()
    {
        //saves axis movement of x and y mouse movement
        float mx = Input.GetAxis("Mouse X");
        float my = Input.GetAxis("Mouse Y");

        // yaw and pitch values change determined on 
        // mousex and mousey movement and applied sensitivity to both
        yaw += mx * yawSensitivity;
        pitch -= my * pitchSensitivity;

        //clamp pitch, so camera doesn't rotate too far low or high to seem weird
        float pitch_clamped = Mathf.Clamp(pitch, -89f, 45f);

        // use the clamped pitch and yaw to rotate camera rig, entered in as euler angles through Quaternion class 
        transform.rotation = Quaternion.Euler(pitch_clamped, yaw, 0);
    }
}
