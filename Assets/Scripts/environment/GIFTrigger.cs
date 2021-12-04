using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//This controls when the gifs blocking the windows turns on
public class GIFTrigger : MonoBehaviour
{
    private float randomTimer = 1;
    private bool IsZone = false;

    SpriteRenderer sr;
    public Sprite blackOut;
    Animator anim;

    GameHUDActivations gameHudScript;

    public List<AudioClip> clipList = new List<AudioClip>();

    private AudioSource gifSource;
    private bool playOnce = false;

    private float randomExistanceTimer;
    private float existanceTimer;
    // Start is called before the first frame update
    void Start()
    {
        //Grabs the values
        randomExistanceTimer = Random.Range(20f, 30f);
        existanceTimer = randomExistanceTimer;
        gameHudScript = GameObject.Find("GameHUD").GetComponent<GameHUDActivations>();

        gifSource = GetComponent<AudioSource>();
        sr = GetComponent<SpriteRenderer>();
        sr.sprite = blackOut;

        anim = GetComponent<Animator>();
        anim.enabled = false;

        enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        //check if the player is in the zone, usually the room of the gif
        if (!IsZone) return;

        if (IsZone) randomTimer -= 1 * Time.deltaTime;

        //enables the gif
        if(IsZone && randomTimer <= 0 && !gameHudScript.isPaused)
        {
            anim.enabled = true;
            if(!playOnce) PlaySound();
            if(playOnce) UnPauseSound();
        }//existance timer
        if (IsZone && randomTimer <= 0 && !gameHudScript.isPaused)
        {
            existanceTimer -= 1 * Time.deltaTime;
        }

        if(existanceTimer <= 0)// stops gif from existing
        {
            IsZone = false;
            randomTimer = 1;
            anim.enabled = false;
            sr.sprite = blackOut;
            StopSound();
            existanceTimer = randomExistanceTimer;
        }

        if (gameHudScript.isPaused) PauseSound();

    }

    private void OnTriggerEnter(Collider other) //player entering trigger volume
    {
        if (other.gameObject.tag == "Player") 
        {
            IsZone = true;
            randomTimer = Random.Range(0.25f,20);
        }
    }

    private void OnTriggerExit(Collider other) //player exiting trigger volume
    {
        if (other.gameObject.tag == "Player")
        {
            IsZone = false;
            randomTimer = 1;
            anim.enabled = false;
            sr.sprite = blackOut;
            existanceTimer = randomExistanceTimer;
            StopSound();
        }
    }

    private void PlaySound()
    {
        if (!gifSource.isPlaying)
        {
            int randIndex = Random.Range(0, clipList.Count);
            gifSource.clip = clipList[randIndex];
            playOnce = true;
            gifSource.Play();
        }
    }

    private void PauseSound()
    {
        if (gifSource.isPlaying) gifSource.Pause();
    }

    private void UnPauseSound()
    {
        if (!gifSource.isPlaying) gifSource.UnPause();
    }

    private void StopSound()
    {
        if (gifSource.isPlaying) gifSource.Stop();
        playOnce = false;
        gifSource.clip = null;
    }
}
