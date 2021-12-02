using UnityEngine;

public class FlashLightOff : MonoBehaviour
{
    //this script is used in the QandA scene only and controls the post-processing and flashlight gameobject
    //thinking about using this for after beating the q&a game, spawning the demon to chase you to the basement door!
    public GameObject Demon;
    LocalPostProcessing localPPScript;

    public GameObject EscapedGO;

    private void Start()
    {
        //Gets player GO
        if (GameObject.Find("FPSController")) 
        {
            localPPScript = GameObject.Find("FPSController").GetComponent<LocalPostProcessing>();
        }
        else 
        {
            localPPScript = null;
        }
        
        //Gets Demon GO
        if (GameObject.Find("EscapedMonster")) 
        {
            EscapedGO = GameObject.Find("EscapedMonster");
            EscapedGO.SetActive(false); //hides him in front of mirror, for good spawning point
        }
        else 
        {
            EscapedGO = null;
        }
        
    }

    //Once you enter room Post-Processing and Audio changes
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Player") 
        {
            Light FlashLight = GameObject.Find("Flashlight").GetComponent<Light>();
            FlashLight.enabled = false;
            AmbientClipController.ForceUpdate = true;
            AudioController.DialogueSource.Stop();
            AudioController.PlayDialogueSound(14);
        }
    }

    //On you exit room things change back to normal
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            //controls flashlight being on or off during scene
            Light FlashLight = GameObject.Find("Flashlight").GetComponent<Light>();
            FlashLight.enabled = true;
            AmbientClipController.ForceUpdate = false;
            AudioController.DialogueSource.Stop();
            AudioController.PlayDialogueSound(13);
            if (NoDestroy.completedQandA) //If you complete the QandA game things panic again because player will be getting chased!
            {
                AmbientClipController.ForceUpdate = true;
                localPPScript.playerPP.SetActive(true);
                EscapedGO.SetActive(true);
                EscapedGO.GetComponent<PlayerEscaped>().enabled = true;
                Demon.SetActive(true);
                gameObject.SetActive(false);
            } 
        }
    }
}
