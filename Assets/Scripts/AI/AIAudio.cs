using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIAudio : MonoBehaviour
{
    AIMain main;
    AudioSource FootStepSource;
    AudioSource AIVoice;
    public AudioClip stepsPatrol;
    public AudioClip stepsChase;
    public AudioClip bashDoor;

    private FindPoints searchScript;

    //to tell if pausing
    private GameHUDActivations hud;

    public List<AudioClip> patrolClips = new List<AudioClip>();
    public List<AudioClip> chaseClips = new List<AudioClip>();
    public AudioClip scareClips; 

    private void Awake()
    {
        if (GameObject.Find("GameHUD")) hud = GameObject.Find("GameHUD").GetComponent<GameHUDActivations>();
        else hud = null;
    }
    // Start is called before the first frame update
    void Start()
    {
        main = GetComponent<AIMain>();
        FootStepSource = GetComponent<AudioSource>();
        searchScript = GetComponent<FindPoints>();
        AIVoice = GetComponentInChildren<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if (hud.isPaused || main.isScaring || NoDestroy.atGameOver)
        {
            PauseFootSteps();
            PauseVoice();
        }
        if (hud.isPaused) return;

        PlayVoice(); //logic for voice done in function!

        //logic for footsteps here
        if (!main.destroyObj)
        {
            if (main.aiState == "Chase" || main.aiState == "Track")
            {
                //footstep sound logic
                if(main.IsRipper) PlayFootSteps(stepsChase, 1.25f);
                else PlayFootSteps(stepsChase, 1f);

            }
            else if (main.aiState == "ChasePlus") 
            {
                //footstep sound logic
                //ONLY RIPPER
                PlayFootSteps(stepsChase, 1.5f);
            }
            else if (main.aiState == "Patrol")
            {
                if (main.IsScreaming)
                {
                    StopFootSteps();
                }
                else
                {
                    //footstep sound logic
                    if (main.IsRipper) PlayFootSteps(stepsPatrol, 1.75f);
                    else PlayFootSteps(stepsPatrol, 1f);
                }
            }
            else if (main.aiState == "Search")
            {
                if (searchScript.targetCopy)
                {
                    float distanceBetweenTarget = Vector3.Distance(transform.position, searchScript.targetCopy.transform.position);
                    if (distanceBetweenTarget <= 0.1f)
                    {
                        //footstep sound logic
                        StopFootSteps();
                    }
                    else
                    {
                        //footstep sound logic
                        if (main.IsRipper) PlayFootSteps(stepsPatrol, 1.75f);
                        else PlayFootSteps(stepsPatrol, 1f);

                    }
                }
            }
        }
        else 
        {
            //footstep sound logic
            if(main.IsRipper) PlayFootSteps(bashDoor, 1.25f);
            else PlayFootSteps(bashDoor, 0.75f);
        }
    }

    public void PlayFootSteps(AudioClip clip, float pitch) 
    {
        FootStepSource.clip = clip;
        FootStepSource.pitch = pitch;
        if (!FootStepSource.isPlaying) 
        {
            FootStepSource.Play();
        } 
    }
    public void PauseFootSteps()
    {
        if (FootStepSource.isPlaying) FootStepSource.Pause();
    }
    public void StopFootSteps()
    {
        if (FootStepSource.isPlaying) 
        {
            FootStepSource.Stop();
            FootStepSource.clip = null;
            FootStepSource.pitch = 1f;
        } 
    }

    public void PlayVoice()
    {
        if (!AIVoice.isPlaying)
        {
            if (main.aiState == "Patrol" || main.aiState == "Search" && !main.isScaring && !main.IsScreaming) 
            {
                int randomIndex = Random.Range(0, patrolClips.Count);
                AIVoice.clip = patrolClips[randomIndex];
            }
            else if (main.aiState == "Chase" || main.aiState == "Track" || main.aiState == "ChasePlus" && !main.isScaring && !main.IsScreaming) 
            {
                int randomIndex = Random.Range(0, chaseClips.Count);
                AIVoice.clip = chaseClips[randomIndex];
            }else if (main.isScaring) 
            {
                AIVoice.clip = scareClips;
            }
            else if (main.IsScreaming) 
            {
                AIVoice.clip = scareClips;
            }

            AIVoice.Play();
        }
    }
    public void PauseVoice()
    {
        if (AIVoice.isPlaying) AIVoice.Pause();
    }
    public void StopVoice()
    {
        if (AIVoice.isPlaying)
        {
            AIVoice.Stop();
            AIVoice.clip = null;
        }
    }
}
