using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SceneAudioPlayer : MonoBehaviour
{
    public GameObject StoryScenePanel;
    AudioSource source;
    public List<AudioClip> storyClips = new List<AudioClip>();

    NoDestroy GameControllerScript;

    List<GameObject> PlayerButtons = new List<GameObject>();
    public TextMeshProUGUI PauseButton;

    public string selectedClip = "";
    public string clipName = "";

    public GameObject MirrorScreen;
    SpriteRenderer sr;
    Animator ac;

    public List<RuntimeAnimatorController> animCons = new List<RuntimeAnimatorController>();
    
    void Awake() 
    {
        source = GetComponent<AudioSource>();
        if (GameObject.Find("NoDestroyOBJ")) GameControllerScript = GameObject.Find("NoDestroyOBJ").GetComponent<NoDestroy>();
        else GameControllerScript = null;

        if (GameObject.Find("MirrorScreenOBJ"))
        {
            MirrorScreen = GameObject.Find("MirrorScreenOBJ");
            sr = MirrorScreen.GetComponent<SpriteRenderer>();
            ac = MirrorScreen.GetComponent<Animator>();
        }
        else 
        {
            MirrorScreen = null;
        } 
    }

    private void OnEnable()
    {
        source.enabled = true;
        StoryScenePanel.SetActive(true);

        foreach (GameObject state in GameObject.FindGameObjectsWithTag("StateButtons")) 
        {
            PlayerButtons.Add(state);
            state.SetActive(false);
        }
    }

    private void Update()
    {
        if (source.clip != null) 
        {
            ShowStateButtons();
        }
    }

    private void HideStateButtons()
    {
        foreach (GameObject button in PlayerButtons)
        {
            button.SetActive(false);
        }
    }
    private void ShowStateButtons()
    {
        foreach (GameObject button in PlayerButtons)
        {
            button.SetActive(true);
        }
    }

    private void HighlightButtonText(string buttonClicked)
    {
        foreach (GameObject scene in GameObject.FindGameObjectsWithTag("SceneButtons"))
        {
            if (scene.gameObject.name == buttonClicked) scene.transform.GetChild(0).GetComponent<TextMeshProUGUI>().color = Color.red;
            else scene.transform.GetChild(0).GetComponent<TextMeshProUGUI>().color = Color.white;
        }
    }

    public void SelectClip(Button clicked)
    {
        HighlightButtonText(clicked.name);
        clipName = clicked.GetComponentInChildren<TextMeshProUGUI>().text;
        if(clipName == "Scene 1\nAccident") 
        {
            source.clip = storyClips[0];
            sr.enabled = true;
            ac.runtimeAnimatorController = animCons[0];
        }
        else if (clipName == "Scene 2\nAppointment") 
        {
            source.clip = storyClips[1];
            sr.enabled = true;
            ac.runtimeAnimatorController = animCons[1];
        }
        else if (clipName == "Scene 3\nProcedure") 
        {
            source.clip = storyClips[2];
            sr.enabled = true;
            ac.runtimeAnimatorController = animCons[2];
        }
        else if (clipName == "Scene 4\nTragedy")
        {
            source.clip = storyClips[3];
            sr.enabled = true;
            ac.runtimeAnimatorController = animCons[3];
        }
        else 
        {
            sr.enabled = false;
            ac.runtimeAnimatorController = null;
            return;
        }
        PlaySound(); //plays audio drama clip within this class
        PauseButton.color = Color.white;
    }

    public void PlaySound()
    {
        AudioController.ClickSound(); //ui click plz
        ShowStateButtons();
        if (!source.isPlaying)
        {
            source.Play();
            PauseButton.color = Color.white;
        }
    }

    public void PauseSound()
    {
        AudioController.ClickSound(); //ui click plz
        if (source.isPlaying) 
        {
            source.Pause();
            PauseButton.color = Color.red;
        } 
    }

    public void StopSound()
    {
        AudioController.ClickSound(); //ui click plz
        HideStateButtons();
        sr.enabled = false;
        ac.runtimeAnimatorController = null;
        PauseButton.color = Color.white;
        if (source.isPlaying) source.Stop(); //stop clip
        source.clip = null; //clear clip
        HighlightButtonText("");
    }

    public void ExitToMenu()
    {
        AudioController.ClickSound();
        NoDestroy.fileLoaded = "";
        AudioController.StopSound();
        SceneManager.LoadScene("MainMenu");
    }
}
