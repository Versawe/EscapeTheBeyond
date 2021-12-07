using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

//This Script controls the Demon AI that spawns from the mirror in QandA
//A simple AI that only chases player
public class DemonScareEvent : MonoBehaviour
{
    NavMeshAgent nm;
    GameObject Player;
    public GameObject ScareScene;

    public bool IsScaring = false;
    private float scareDis = 1.75f;
    private float scareTimer = 2.5f;

    FindNewPath newPath;

    AudioSource source;
    public AudioClip clip;
    private bool doOnce = false;

    GameHUDActivations hud;

    // Start is called before the first frame update
    void Start()
    {
        //grabs stuff
        Player = GameObject.Find("FPSController");
        nm = GetComponent<NavMeshAgent>();
        newPath = GetComponent<FindNewPath>();
        source = GetComponent<AudioSource>();

        if (GameObject.Find("GameHUD")) hud = GameObject.Find("GameHUD").GetComponent<GameHUDActivations>();
    }

    // Update is called once per frame
    void Update()
    {
        TriggerScare();
        nm.SetDestination(newPath.newTarget); //AI will always chase player
        //nm.SetDestination(Player.transform.position);

        //controls audio on pause and unpause
        if (hud.isPaused) source.Pause();
        else source.UnPause();
    }

    //If AI gets close enough it ends the game
    private void TriggerScare()
    {
        float dist = Vector3.Distance(Player.transform.position, transform.position);
        if (dist <= scareDis)
        {
            IsScaring = true;
            ScareScene.SetActive(true);
        }

        if (IsScaring)
        {
            scareTimer -= 1 * Time.deltaTime;
            //player cant move anymore
            Player.GetComponentInChildren<CameraRotationFirstPerson>().enabled = false;
            Player.GetComponent<CharacterMovementFirstPerson>().enabled = false;
            Player.transform.GetChild(3).gameObject.SetActive(false); //turns of Post-processing on player object
            source.clip = clip;
            if (!doOnce) //plays audio once
            { 
                source.Play();
                doOnce = true;
            }
        }

        //sets game over and stops audio
        if (IsScaring && scareTimer <= 0)
        {
            NoDestroy.atGameOver = true;
            source.Stop();
            //play game over sound maybe?
        }
    }
}
