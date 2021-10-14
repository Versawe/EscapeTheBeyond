using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioController : MonoBehaviour
{
    //Audio Vars
    //in-game dialogue
    public static AudioSource DialogueSource;
    public static AudioSource BGLoopSource;
    public GameObject theChild;

    public AudioClip staticLoop;
    public List<AudioClip> clipListRef = new List<AudioClip>();
    public List<AudioClip> clipList = new List<AudioClip>();
    public static AudioController script;

    private bool hintReset = false;
    // Start is called before the first frame update
    void Start()
    {
        DialogueSource = GetComponent<AudioSource>();
        BGLoopSource = theChild.GetComponent<AudioSource>();
        BGLoopSource.clip = staticLoop;
        script = GetComponent<AudioController>();

        print(DialogueSource.gameObject.name);
        print(BGLoopSource.gameObject.name);

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
            PlayFlashBackSound();
            print(clipList.Count);
            print(clipListRef.Count);
        }*/
    }

    public static void PlayFlashBackSound()
    {
        if (!BGLoopSource.isPlaying) BGLoopSource.Play();
        if (!DialogueSource.isPlaying)
        {
            int randIndex = Random.Range(0, script.clipList.Count);
            DialogueSource.clip = script.clipList[randIndex];
            DialogueSource.Play();
            script.clipList.RemoveAt(randIndex);
        }
    }
    public static void StopSound()
    {
        if (DialogueSource.isPlaying) DialogueSource.Stop();
        if (BGLoopSource.isPlaying) BGLoopSource.Stop();
    }
}
