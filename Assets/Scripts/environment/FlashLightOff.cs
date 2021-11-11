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
        if (GameObject.Find("FPSController")) 
        {
            localPPScript = GameObject.Find("FPSController").GetComponent<LocalPostProcessing>();
        }
        else 
        {
            localPPScript = null;
        }
        
        if (GameObject.Find("EscapedMonster")) 
        {
            EscapedGO = GameObject.Find("EscapedMonster");
            EscapedGO.SetActive(false);
        }
        else 
        {
            EscapedGO = null;
        }
        
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Player") 
        {
            Light FlashLight = GameObject.Find("Flashlight").GetComponent<Light>();
            FlashLight.enabled = false;
            AmbientClipController.ForceUpdate = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            Light FlashLight = GameObject.Find("Flashlight").GetComponent<Light>();
            FlashLight.enabled = true;
            AmbientClipController.ForceUpdate = false;
            if (NoDestroy.completedQandA) 
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
