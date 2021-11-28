using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIAudio : MonoBehaviour
{
    public GameObject SecondAudioSource;
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
    public AudioClip scareClip;

    private bool doOnce = false;
    private int randomIndex;
    private int randomIndex2;

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
        AIVoice = SecondAudioSource.GetComponent<AudioSource>();
        randomIndex = Random.Range(0, patrolClips.Count);
        randomIndex2 = Random.Range(0, chaseClips.Count);
    }

    // Update is called once per frame
    void Update()
    {
        if (hud.isPaused)
        {
            PauseFootSteps();
            PauseVoice();
        }
        if (NoDestroy.atGameOver) 
        {
            StopFootSteps();
            StopVoice();
        }
        if (main.isScaring) 
        {
            PlayVoice(scareClip);
            PauseFootSteps();
        } 
        if (hud.isPaused) return;

        //logic for footsteps here
        if (!main.destroyObj)
        {
            if (main.aiState == "Chase" || main.aiState == "Track")
            {
                //footstep sound logic
                if(main.IsRipper) PlayFootSteps(stepsChase, 1.25f);
                else PlayFootSteps(stepsChase, 1f);

                if (!main.isScaring) PlayVoice(chaseClips[randomIndex2]);

            }
            else if (main.aiState == "ChasePlus") 
            {
                //footstep sound logic
                //ONLY RIPPER
                PlayFootSteps(stepsChase, 1.5f);
                if (!main.isScaring) PlayVoice(chaseClips[randomIndex2]);
            }
            else if (main.aiState == "Patrol")
            {
                if (main.IsScreaming)
                {
                    StopFootSteps();
                    if (!main.isScaring) PlayVoice(scareClip);
                }
                else
                {
                    //footstep sound logic
                    if (main.IsRipper) PlayFootSteps(stepsPatrol, 1.75f);
                    else PlayFootSteps(stepsPatrol, 1f);

                    if (!main.isScaring) PlayVoice(patrolClips[randomIndex]);
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
                if(!main.isScaring) PlayVoice(patrolClips[randomIndex]);
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
        if (NoDestroy.atGameOver) return;
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

    public void PlayVoice(AudioClip clip)
    {
        if (NoDestroy.atGameOver) return;
        AIVoice.clip = clip;

        if (!AIVoice.isPlaying)
        {
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
