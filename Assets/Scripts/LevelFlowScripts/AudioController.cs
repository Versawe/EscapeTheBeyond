using System.Collections.Generic;
using UnityEngine;

public class AudioController : MonoBehaviour
{
    //Audio Vars
    //in-game dialogue
    public static AudioSource DialogueSource;
    public static AudioSource BGLoopSource;
    public static AudioSource UISource;
    public GameObject theChild;
    public GameObject theChild2;

    public AudioClip staticLoop;
    public List<AudioClip> clipListRef = new List<AudioClip>();
    public List<AudioClip> clipList = new List<AudioClip>();
    public static AudioController script;

    public AudioClip clickSound;

    public List<AudioClip> voiceList = new List<AudioClip>();

    private bool hintReset = false;

    public float BGGone = 4f;

    public bool PlayOnlyOnce = false;
    private float attemptedPlayTimer = 10f;

    // Start is called before the first frame update
    void Start()
    {
        DialogueSource = GetComponent<AudioSource>();
        BGLoopSource = theChild.GetComponent<AudioSource>();
        UISource = theChild2.GetComponent<AudioSource>();
        BGLoopSource.clip = staticLoop;
        script = GetComponent<AudioController>();

        foreach (AudioClip clip in clipListRef) 
        {
            clipList.Add(clip);
        }
    }

    void Update()
    {
        if(clipList.Count <= 0) 
        {
            hintReset = true;
        }
        if (hintReset) 
        {
            foreach (AudioClip clip in clipListRef)
            {
                clipList.Add(clip);
            }
            //print("list reset worked is now " + clipList.Count);
            hintReset = false;
        }

        /*if (Input.GetKeyDown("t")) //for testing
        {
            PlayFlashBackSound(40);
        }*/

        if (!DialogueSource.isPlaying && BGLoopSource.isPlaying) 
        {
            BGGone -= 1 * Time.deltaTime;  
        }
        if (BGGone <= 0 && BGLoopSource.isPlaying)
        {
            BGLoopSource.Stop();
        }

        if (PlayOnlyOnce) 
        {
            attemptedPlayTimer -= 1 * Time.deltaTime;

            if (attemptedPlayTimer <= 0)
            {
                attemptedPlayTimer = 10f;
                PlayOnlyOnce = false;
            }
        } 
    }

    public static void PlayDialogueSound(int num)
    {
        if(!BGLoopSource.isPlaying && !DialogueSource.isPlaying) 
        {
            DialogueSource.clip = script.voiceList[num];
            //print(DialogueSource.clip.length);
            DialogueSource.Play();
        }
        //to interupt heavy breathing sound for dialogue
        if(CharacterMovementFirstPerson.IsBreathingHeavy && DialogueSource.isPlaying) 
        {
            StopSound();
            DialogueSource.volume = 1f;
            DialogueSource.clip = script.voiceList[num];
            //print(DialogueSource.clip.length);
            DialogueSource.Play();
        }
    }

    public static void PlayFlashBackSound(int percentChance)
    {
        if (!script.PlayOnlyOnce)
        {
            float singleNum = percentChance * .10f;
            float randomNum = Random.Range(1, 11);
            print(randomNum);
            if (singleNum >= randomNum && !DialogueSource.isPlaying)
            {
                if (!BGLoopSource.clip) BGLoopSource.clip = script.staticLoop;
                if (!BGLoopSource.isPlaying) BGLoopSource.Play();
                if (!DialogueSource.isPlaying)
                {
                    int randIndex = Random.Range(0, script.clipList.Count);
                    DialogueSource.clip = script.clipList[randIndex];
                    DialogueSource.Play();
                    script.clipList.RemoveAt(randIndex);
                }
                script.BGGone = 4f;
                //to interupt heavy breathing sound for dialogue
                if (CharacterMovementFirstPerson.IsBreathingHeavy && DialogueSource.isPlaying)
                {
                    StopSound();
                    DialogueSource.volume = 1f;
                    int randIndex = Random.Range(0, script.clipList.Count);
                    DialogueSource.clip = script.clipList[randIndex];
                    DialogueSource.Play();
                    script.clipList.RemoveAt(randIndex);
                }
            }
            script.PlayOnlyOnce = true;
        }
    }

    public static void PauseSound() 
    {
        if (DialogueSource.isPlaying) DialogueSource.Pause();
        if (BGLoopSource.isPlaying) BGLoopSource.Pause();
    }

    public static void UnPauseSound()
    {
        if (!DialogueSource.isPlaying) DialogueSource.UnPause();
        if (!BGLoopSource.isPlaying) BGLoopSource.UnPause();
    }

    public static void StopSound()
    {
        if (DialogueSource.isPlaying) DialogueSource.Stop();
        if (BGLoopSource.isPlaying) BGLoopSource.Stop();
        DialogueSource.clip = null;
        BGLoopSource.clip = null;
    }

    public static void ClickSound() 
    {
        UISource.clip = script.clickSound;
        UISource.Play();
    }
}
