using System;
using System.Collections;
using System.Collections.Generic;
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

    //public Transform target;

    private GameObject player;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player");
        lookScript = GetComponent<LookAtStuff>();

        //at start set the player's sensitivity
        yawSensitivity = NoDestroy.pSensitivity;

        //starting rotation per level
        if (NoDestroy.gameProgression == 1) yaw = 90;
        else yaw = -90;
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
        float pitch_clamped = Mathf.Clamp(pitch, -89f, 45f);

        // use the clamped pitch and yaw to rotate camera rig, entered in as euler angles through Quaternion class 
        if(Time.timeScale > 0) transform.rotation = Quaternion.Euler(pitch_clamped, yaw, 0);
    }
}
