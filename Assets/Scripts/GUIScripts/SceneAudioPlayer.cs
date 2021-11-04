using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class SceneAudioPlayer : MonoBehaviour
{
    public GameObject StoryScenePanel;

    AudioSource source;
    List<AudioClip> storyClips = new List<AudioClip>();

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
        clipName = clicked.GetComponentInChildren<TextMeshProUGUI>().text;
    }

    private void PlaySound()
    {
        if (!source.isPlaying)
        {
            source.Play();
        }
    }

    private void PauseSound()
    {
        if (source.isPlaying) source.Pause();
    }

    private void StopSound()
    {
        if (source.isPlaying) source.Stop();
    }
}
