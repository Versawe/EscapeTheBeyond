using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioController : MonoBehaviour
{
    //Audio Vars
    //in-game dialogue
    public static AudioSource DialogueSource;
    public List<AudioClip> audioClipList = new List<AudioClip>();
    public static AudioController script;
    // Start is called before the first frame update
    void Start()
    {
        DialogueSource = GetComponent<AudioSource>();
        script = GetComponent<AudioController>();
    }

    public static void PlaySound(string clipName) 
    {
        foreach (AudioClip clip in script.audioClipList)
        {
            if (clip.name == clipName) 
            {
                if (!DialogueSource.isPlaying)
                {
                    DialogueSource.clip = clip;
                    DialogueSource.Play();
                }
            }
        }
    }
    public static void StopSound()
    {
        if (DialogueSource.isPlaying) DialogueSource.Stop();
    }
}
