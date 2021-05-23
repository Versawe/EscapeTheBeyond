using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScareCam : MonoBehaviour
{

    public Camera FPSCamera;
    public GameObject scareScene;

    PlayerHealth pHealth;
    CameraRotationFirstPerson camScript;
    CharacterMovementFirstPerson pMoveScript;

    private void Awake()
    {
        pHealth = GetComponent<PlayerHealth>();
        pMoveScript = GetComponent<CharacterMovementFirstPerson>();
        camScript = GetComponentInChildren<CameraRotationFirstPerson>();
    }

    private void OnEnable()
    {
        scareScene.gameObject.SetActive(true);
        FPSCamera.enabled = false;
        pMoveScript.enabled = false;
        camScript.enabled = false;

    }
    private void OnDisable()
    {
        if (pHealth.health > 0) 
        {
            scareScene.gameObject.SetActive(false);
            FPSCamera.enabled = true;
            pMoveScript.enabled = true;
            camScript.enabled = true;
        }
        
    }
}
