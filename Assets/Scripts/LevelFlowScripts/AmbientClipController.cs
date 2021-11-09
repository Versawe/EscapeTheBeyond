using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class AmbientClipController : MonoBehaviour
{
    Scene actualScene;
    AudioSource ambientSource;
    public List<AudioClip> ambientClips = new List<AudioClip>();
    //public List<AudioClip> intenseClips = new List<AudioClip>();

    GameHUDActivations hudScript;

    public static float pitchFloat = 1f;
    public static float volumeFloat = 0.75f;

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
        ambientSource = GetComponent<AudioSource>();
    }

    void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode) 
    {
        actualScene = SceneManager.GetActiveScene();
        pitchFloat = 1f;
        volumeFloat = 0.75f;
        if (actualScene.name != "Preload" && actualScene.name != "MainMenu") 
        {
            RandomAmbientTrack();
        }
        else 
        {
            ambientSource.clip = null;
        }

        if (GameObject.Find("GameHUD")) 
        {
            hudScript = GameObject.Find("GameHUD").GetComponent<GameHUDActivations>();
        }
        else 
        {
            hudScript = null;
        }
    }

    private void Update()
    {
        if (!hudScript) return;
        if (hudScript.isPaused) PauseSound();
        if (!hudScript.isPaused) UnPauseSound();
        if (NoDestroy.atGameOver) StopSound(); // or change to an end game track

        if (NoDestroy.currSceneName != "QandA") 
        {
            //if you are getting scared it changes the ambient BG sound
            if (NoDestroy.TriggerScarePP || NoDestroy.TriggerScarePPAI || ScareCam.stillCreepySound)
            {
                AmbientScary();
            }
            else
            {
                AmbientNormal();
            }
        }

        ambientSource.pitch = pitchFloat;
        ambientSource.volume = volumeFloat;

    }

    public static void AmbientNormal()
    {
        pitchFloat = 1f;
        volumeFloat = 0.75f;
    }

    public static void AmbientScary()
    {
        pitchFloat = 0.5f;
        volumeFloat = 1f;
    }

    private void RandomAmbientTrack()
    {
        int randomIndex = Random.Range(0, ambientClips.Count);
        ambientSource.clip = ambientClips[randomIndex];

        PlaySound();
    }

    public void PlaySound()
    {
        ambientSource.Play();
    }
    public void PauseSound()
    {
        if (ambientSource.isPlaying) ambientSource.Pause();
    }
    public void UnPauseSound()
    {
        if (!ambientSource.isPlaying) ambientSource.UnPause();
    }
    public void StopSound()
    {
        if (ambientSource.isPlaying) ambientSource.Stop();
    }

}
