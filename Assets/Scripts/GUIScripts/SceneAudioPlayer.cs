using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class SceneAudioPlayer : MonoBehaviour
{
    public GameObject StoryScenePanel;

    AudioSource source;
    public List<AudioClip> storyClips = new List<AudioClip>();

    public string selectedClip = "";
    public string clipName = "";
    void Awake() 
    {
        source = GetComponent<AudioSource>();
    }

    private void OnEnable()
    {
        source.enabled = true;
        StoryScenePanel.SetActive(true);
    }

    public void SelectClip(Button clicked) 
    {
        AudioController.ClickSound(); //ui click plz
        clipName = clicked.GetComponentInChildren<TextMeshProUGUI>().text;
        if(clipName == "Scene 1") 
        {
            source.clip = storyClips[0];
        }
        else if (clipName == "Scene 2") 
        {
            source.clip = storyClips[1];
        }
        else if (clipName == "Scene 3") 
        {
            source.clip = storyClips[2];
        }
        else if (clipName == "Scene 4") 
        {
            source.clip = storyClips[3];
        }
        else 
        {
            return;
        }
        PlaySound(); //plays audio drama clip within this class
    }

    public void PlaySound()
    {
        AudioController.ClickSound(); //ui click plz
        if (!source.isPlaying)
        {
            source.Play();
        }
    }

    public void PauseSound()
    {
        AudioController.ClickSound(); //ui click plz
        if (source.isPlaying) source.Pause();
    }

    public void StopSound()
    {
        AudioController.ClickSound(); //ui click plz
        if (source.isPlaying) source.Stop();
    }
}
