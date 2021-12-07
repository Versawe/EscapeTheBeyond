using UnityEngine;

//this is attachted to the PLayer's Camera
//this controls the triggering of the scare scenes when the AI catches a player
public class ScareCam : MonoBehaviour
{
    public Camera FPSCamera;
    public GameObject ripperScene;
    public GameObject mutantScene;
    public string nameOfAI;
    public GameObject ScarePostP;

    PlayerHealth pHealth;
    CameraRotationFirstPerson camScript;
    CharacterMovementFirstPerson pMoveScript;

    public static bool stillCreepySound = false;

    private void Awake()
    {
        pHealth = GetComponent<PlayerHealth>();
        pMoveScript = GetComponent<CharacterMovementFirstPerson>();
        camScript = GetComponentInChildren<CameraRotationFirstPerson>();
    }

    private void OnEnable()
    {
        if (NoDestroy.currSceneName == "HellScene") pHealth.enabled = true;
        stillCreepySound = false;
        if (nameOfAI.Substring(0, 1) == "r") ripperScene.gameObject.SetActive(true);
        else mutantScene.gameObject.SetActive(true);
        ScarePostP.SetActive(true);
        FPSCamera.enabled = false;
        pMoveScript.enabled = false;
        camScript.enabled = false;
        stillCreepySound = true;
        NoDestroy.BigScareHappening = true;

    }
    private void OnDisable()
    {
        if (pHealth.health > 0) //gets caught but still alive aftewards
        {
            if (nameOfAI.Substring(0, 1) == "r") ripperScene.gameObject.SetActive(false); //caught by ripper
            else mutantScene.gameObject.SetActive(false); //caught by a zombie mutant
            ScarePostP.SetActive(false);
            FPSCamera.enabled = true;
            pMoveScript.enabled = true;
            camScript.enabled = true;
            nameOfAI = "";
        }
        stillCreepySound = false; //needs to be here to not glitch on game overs aka "does not change back to false"
        NoDestroy.BigScareHappening = false;
    }
}
