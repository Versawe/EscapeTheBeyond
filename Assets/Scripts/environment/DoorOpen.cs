using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    // Start is called before the first frame update
    void Awake()
    {
        Player = GameObject.Find("FPSController");

        seeDoorScript = Player.GetComponentInChildren<LookAtStuff>();

        doorLocked = GetComponent<LockedDoor>();
    }

    private void Start()
    {
        doorXOpen1 = Quaternion.Euler(0f, 90f, 0f); // player.x greater
        doorXOpen2 = Quaternion.Euler(0f, 270f, 0f);

        doorZOpen1 = Quaternion.Euler(0f, 0f, 0f); //player.x greater
        doorZOpen2 = Quaternion.Euler(0f, -180f, 0f);

        isOpen = false;
    }
    // Update is called once per frame
    void Update()
    {
        if (doorLocked.IsLocked) return;
        else if(doorLocked.IsLocked && doorLocked.Puzzle2Trigger) 
        {
            //trigger AI and Relic spawn!!!
            return;
        }
        playerInput();
        whichSide();
        doorMove();
    }

    private void playerInput()
    {
        //eDown = Input.GetKeyDown("e");
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
            transform.rotation = Slide(transform.rotation, doorStartRot, swingSpeed);
            doOnceTimer -= Time.deltaTime;
            if (doOnceTimer <= 0) 
            {
                isOpen = false;
                isShutting = false;
                doOnce = false;
                doOnceTimer = 0.5f;
            } 
        }
        if (thisDoor.gameObject.tag == "DoorX" && !isOpen && doOnce)
        {
            if (side == "x2")
            {
                transform.rotation = Slide(transform.rotation, doorXOpen2, swingSpeed);
                doOnceTimer -= Time.deltaTime;
                if (doOnceTimer <= 0)
                {
                    isOpen = true;
                    doOnce = false;
                    doOnceTimer = 0.5f;
                }
            }
            else if(side == "x1")
            {
                transform.rotation = Slide(transform.rotation, doorXOpen1, swingSpeed);
                doOnceTimer -= Time.deltaTime;
                if (doOnceTimer <= 0)
                {
                    isOpen = true;
                    doOnce = false;
                    doOnceTimer = 0.5f;
                }
            }
        }
        //door z's opens and closes
        if (thisDoor.gameObject.tag == "DoorZ" && isOpen && doOnce) // rotate to close position
        {
            isShutting = true;
            transform.rotation = Slide(transform.rotation, doorStartRot, swingSpeed);
            doOnceTimer -= Time.deltaTime;
            if (doOnceTimer <= 0)
            {
                isShutting = false;
                isOpen = false;
                doOnce = false;
                doOnceTimer = 0.5f;
            }
        }
        if (thisDoor.gameObject.tag == "DoorZ" && !isOpen && doOnce) //Opening door
        {
            if (side == "z1") //open if one sideA of door
            {
                transform.rotation = Slide(transform.rotation, doorZOpen1, swingSpeed);
                doOnceTimer -= Time.deltaTime;
                if (doOnceTimer <= 0)
                {
                    isOpen = true;
                    doOnce = false;
                    doOnceTimer = 0.5f;
                }
            }
            else if(side == "z2") //open if one sideB of door
            {
                transform.rotation = Slide(transform.rotation, doorZOpen2, swingSpeed);
                doOnceTimer -= Time.deltaTime;
                if (doOnceTimer <= 0)
                {
                    isOpen = true;
                    doOnce = false;
                    doOnceTimer = 0.5f;
                }
            }
        }
    }

    public Quaternion Slide(Quaternion current, Quaternion target, float percentLeft = 0.5f)
    {
        float p = 1 - Mathf.Pow(percentLeft, Time.deltaTime);
        return Quaternion.Lerp(current, target, p);
    }
}
