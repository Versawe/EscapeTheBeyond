using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    LookAtDoor seeDoorScript;

    Quaternion doorLeftOpen;
    Quaternion doorRightOpen;

    public Quaternion doorStartRotRight;
    public Quaternion doorStartRotLeft;

    // Start is called before the first frame update
    void Awake()
    {
        Player = GameObject.Find("FPSController");

        seeDoorScript = Player.GetComponentInChildren<LookAtDoor>();
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
            leftDoor.transform.localRotation = Slide(leftDoor.transform.localRotation, doorStartRotLeft, swingSpeed);
            rightDoor.transform.localRotation = Slide(rightDoor.transform.localRotation, doorStartRotRight, swingSpeed);
            doOnceTimer -= Time.deltaTime;
            if (doOnceTimer <= 0)
            {
                isShutting = false;
                isOpen = false;
                doOnce = false;
                doOnceTimer = 0.5f;
            }
        }
        if (!isOpen && doOnce) // open animation and variable sets
        {
            leftDoor.transform.localRotation = Slide(leftDoor.transform.localRotation, doorLeftOpen, swingSpeed);
            rightDoor.transform.localRotation = Slide(rightDoor.transform.localRotation, doorRightOpen, swingSpeed);
            doOnceTimer -= Time.deltaTime;
            if (doOnceTimer <= 0)
            {
                isOpen = true;
                doOnce = false;
                doOnceTimer = 0.5f;
            }
        }
    }

    //slide function for smooth and realiabe rotation animations
    public Quaternion Slide(Quaternion current, Quaternion target, float percentLeft = 0.5f)
    {
        float p = 1 - Mathf.Pow(percentLeft, Time.deltaTime);
        return Quaternion.Lerp(current, target, p);
    }
}
