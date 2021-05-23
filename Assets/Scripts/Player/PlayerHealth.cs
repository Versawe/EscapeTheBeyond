using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerHealth : MonoBehaviour
{
    public int health = 3;

    public Image heart;
    public Image heart1;
    public Image heart2;
    public Button retryButt;

    CameraRotationFirstPerson camScript;
    CharacterMovementFirstPerson pMoveScript;

    private void Awake()
    {
        pMoveScript = GetComponent<CharacterMovementFirstPerson>();
        camScript = GetComponentInChildren<CameraRotationFirstPerson>();
    }

    private void Start()
    {
        heart.enabled = true;
        heart1.enabled = true;
        heart2.enabled = true;
        retryButt.gameObject.SetActive(false);
    }

    private void Update()
    {
        if (health == 2) 
        {
            heart2.enabled = false;
        }
        if (health == 1)
        {
            heart1.enabled = false;
        }
        if(health < 1)
        {
            heart.enabled = false;
            camScript.enabled = false;
            pMoveScript.enabled = false;
            pMoveScript.gameObject.GetComponent<ScareCam>().enabled = true;
            retryButt.gameObject.SetActive(true);
            Cursor.lockState = CursorLockMode.None;
        }
    }

    public void RestartGame()
    {
        SceneManager.LoadScene("test");
    }
}
