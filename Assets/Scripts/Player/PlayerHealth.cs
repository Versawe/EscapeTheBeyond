using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerHealth : MonoBehaviour
{
    public int health = 3;

    public GameObject heart;
    public GameObject heart1;
    public GameObject heart2;
    //public Button retryButt; // Replace with a Game Over Screen
    public GameObject flashLight;

    CameraRotationFirstPerson camScript;
    CharacterMovementFirstPerson pMoveScript;

    private void Awake()
    {
        pMoveScript = GetComponent<CharacterMovementFirstPerson>();
        camScript = GetComponentInChildren<CameraRotationFirstPerson>();
    }

    private void Start()
    {
        flashLight = GameObject.Find("Flashlight");
        heart = GameObject.Find("Heart1");
        heart1 = GameObject.Find("Heart2");
        heart2 = GameObject.Find("Heart3");

        if (SceneManager.GetActiveScene().name != "RelicHunt") 
        {
            flashLight.SetActive(false);
            this.enabled = false;
            return;
        }
        flashLight.SetActive(true);

        heart.SetActive(true);
        heart1.SetActive(true);
        heart2.SetActive(true);
        //retryButt.gameObject.SetActive(false);
    }

    private void Update()
    {
        if (health == 2) 
        {
            heart2.SetActive(false);
        }
        if (health == 1)
        {
            heart1.SetActive(false);
        }
        if(health < 1)
        {
            heart.SetActive(false);
            camScript.enabled = false;
            pMoveScript.enabled = false;
            pMoveScript.gameObject.GetComponent<ScareCam>().enabled = true;
            //retryButt.gameObject.SetActive(true);
            Cursor.lockState = CursorLockMode.None;
        }
    }

    public void RestartGame()
    {
        SceneManager.LoadScene("RelicHunt");
    }
}
