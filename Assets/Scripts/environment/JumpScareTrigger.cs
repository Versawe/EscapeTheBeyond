using System.Collections.Generic;
using UnityEngine;
//using UnityEditor.Animations;

public class JumpScareTrigger : MonoBehaviour
{
    public GameObject ScareObj;
    
    //Anim Controllers
    public List<RuntimeAnimatorController> scareGifs = new List<RuntimeAnimatorController>();

    Animator currAnim;
    SpriteRenderer sr;

    private float scareChance = 20f; //actual > chance ? scare!
    private float scareActual = 0f;
    private float scareTimer = 0f;
    private bool IsTriggered = false;

    //audio vars
    public List<AudioClip> clipList = new List<AudioClip>();
    private AudioSource gifSource;
    private bool playOnce = false;

    GameHUDActivations gameHudScript;

    // Start is called before the first frame update
    void Start()
    {
        gameHudScript = GameObject.Find("GameHUD").GetComponent<GameHUDActivations>();
        gifSource = GetComponent<AudioSource>();
        currAnim = GetComponentInChildren<Animator>();
        sr = GetComponentInChildren<SpriteRenderer>();
        sr.sprite = null;

        ScareObj.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (!IsTriggered) return;
        scareTimer -= 1 * Time.deltaTime;
        if (scareTimer <= 0) 
        {
            scareActual = 0;
            scareTimer = 0;
            currAnim.runtimeAnimatorController = null;
            sr.sprite = null;
            StopSound();
            ScareObj.SetActive(false);
            IsTriggered = false;
            NoDestroy.TriggerScarePP = false;
        }

        if (gameHudScript.isPaused) PauseSound();
        else UnPauseSound();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            if (!IsTriggered) 
            {
                scareActual = Random.Range(1, scareChance+1);
                scareTimer = Random.Range(0.25f, 2f);
                //print("Do Once!");
            }
            if (scareActual >= scareChance && !IsTriggered)
            {
                float chooseAnim = Random.Range(0, scareGifs.Count);
                currAnim.runtimeAnimatorController = scareGifs[(int)chooseAnim];
                ScareObj.SetActive(true);
                //print("Triggered, do once!");
                IsTriggered = true;
                PlaySound();
                NoDestroy.TriggerScarePP = true;
            }
            if (!IsTriggered) 
            {
                IsTriggered = false;
                sr.sprite = null;
                //print("Not triggered, do once!");
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            NoDestroy.TriggerScarePP = false;
            IsTriggered = false;
            sr.sprite = null;
            scareActual = 0;
            scareTimer = 0;
            currAnim.runtimeAnimatorController = null;
            StopSound();
            ScareObj.SetActive(false);
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
