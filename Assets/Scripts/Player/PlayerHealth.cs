using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//Keeps track of player's health and UI icons for health
public class PlayerHealth : MonoBehaviour
{
    public int health = 3;

    public GameObject heart;
    public GameObject heart1;
    public GameObject heart2;

    CameraRotationFirstPerson camScript;
    CharacterMovementFirstPerson pMoveScript;

    private void Awake()
    {
        pMoveScript = GetComponent<CharacterMovementFirstPerson>();
        camScript = GetComponentInChildren<CameraRotationFirstPerson>();
    }

    private void OnEnable()
    {
        /*heart = GameObject.Find("Heart1");
        heart1 = GameObject.Find("Heart2");
        heart2 = GameObject.Find("Heart3");*/

        if (NoDestroy.currSceneName == "RelicHunt") //if in the correct scene set the icons to show
        {
            heart.SetActive(true);
            heart1.SetActive(true);
            heart2.SetActive(true);
        }
        else if(NoDestroy.HasBeenTamperedWith) //only have one life otherwise
        {
            health = 1;
        }

    }

    private void Update()
    {
        //below will deactivate certain elements when player's life changes
        if (health == 2) 
        {
            heart2.SetActive(false);
        }
        if (health == 1)
        {
            heart2.SetActive(false);
            heart1.SetActive(false);
        }
        if(health < 1)
        {
            heart.SetActive(false);
            camScript.enabled = false;
            pMoveScript.enabled = false;
            pMoveScript.gameObject.GetComponent<ScareCam>().enabled = true;
            NoDestroy.atGameOver = true;
            Cursor.lockState = CursorLockMode.None;
        }

    }

}
