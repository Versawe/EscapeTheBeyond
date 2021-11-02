using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    // Start is called before the first frame update
    void Start()
    {
        gameHudScript = GameObject.Find("GameHUD").GetComponent<GameHUDActivations>();

        gifSource = GetComponent<AudioSource>();
        sr = GetComponent<SpriteRenderer>();
        sr.sprite = blackOut;

        anim = GetComponent<Animator>();
        anim.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (!IsZone) return;

        if (IsZone) randomTimer -= 1 * Time.deltaTime;

        if(IsZone && randomTimer <= 0)
        {
            anim.enabled = true;
            if(!playOnce) PlaySound();
            if(playOnce) UnPauseSound();
        }

        if (gameHudScript.isPaused) PauseSound();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player") 
        {
            IsZone = true;
            randomTimer = Random.Range(0.25f,12);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            IsZone = false;
            randomTimer = 1;
            anim.enabled = false;
            sr.sprite = blackOut;
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
