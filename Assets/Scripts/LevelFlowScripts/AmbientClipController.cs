using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class AmbientClipController : MonoBehaviour
{
    Scene actualScene;
    public AudioSource ambientSource;
    public List<AudioClip> ambientClips = new List<AudioClip>();
    //public List<AudioClip> intenseClips = new List<AudioClip>();

    GameHUDActivations hudScript;

    public static float pitchFloat = 1f;
    public static float volumeFloat = 0.75f;

    public static bool ForceUpdate = false;
    public static bool ForceHell = false;
    private float splitSecond = 1.25f;

    public static bool IsPitchSlide = true;
    public static bool IsSteroPanSlide = false;
    public static float stereoPanFloat = 0f;

    public static AmbientClipController thisScript;

    public AudioClip whisperHints;

    private void Awake()
    {
        thisScript = gameObject.GetComponent<AmbientClipController>();
        DontDestroyOnLoad(gameObject);
        ambientSource = GetComponent<AudioSource>();
    }

    void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    //On Scene Loads reset Values
    void OnSceneLoaded(Scene scene, LoadSceneMode mode) 
    {
        actualScene = SceneManager.GetActiveScene();
        pitchFloat = 1f;
        volumeFloat = 0.75f;
        splitSecond = 1.25f;
        stereoPanFloat = 0f;
        ForceUpdate = false;
        AmbientNormal();

        if (actualScene.name != "Preload" && actualScene.name != "EndGame") //No tracks play on preload or End Scene Audio player
        {
            RandomAmbientTrack();
        }
        else 
        {
            ambientSource.clip = null;
        }

        //gets GameHUD Object
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
        if (NoDestroy.gameProgression == 1 && NoDestroy.puzzleOneLoginAttempts == 1) //this doesnt work, and i don't care
        {
            ambientSource.clip = whisperHints;
            if(!ambientSource.isPlaying) PlaySound();
        }
        
        //update constantly updating audio source values with these floats
        ambientSource.pitch = pitchFloat;
        ambientSource.volume = volumeFloat;
        ambientSource.panStereo = stereoPanFloat;

        if (!hudScript) return; //if no hud script exists does not continue
        if (hudScript.isPaused) PauseSound(); //pauses ambient on pause screen
        if (!hudScript.isPaused) UnPauseSound(); //unpauses
        if (NoDestroy.atGameOver || NoDestroy.atGameComplete) EndGameAmbient(); // THIS or change to an end game track

        if (!NoDestroy.completedQandA) //before completing the question and answers game
        {
            //if you are getting scared it changes the ambient BG sound
            if (NoDestroy.TriggerScarePP || NoDestroy.TriggerScarePPAI || ScareCam.stillCreepySound || ForceUpdate)
            {
                AmbientScary();
            }
            else
            {
                AmbientNormal();
            }
        }
        else //after completeing the game
        {
            if (splitSecond > 0) //a slight delay to adjust values back to normal
            {
                ForceUpdate = false;
                AmbientNormal();
            } 
            splitSecond -= 1f * Time.deltaTime; //running float down

            //checking for situations on final scene and ajdusting the ambient AS values accordingly
            if (ForceUpdate && !ForceHell) AmbientScary();
            else if (!ForceUpdate && !ForceHell) AmbientNormal();
            else if (ForceHell) AmbientHell();
        }
        

    }

    public static void AmbientNormal()
    {
        //adjusting pitch and volume on ambient audio source
        if (pitchFloat < 1f) pitchFloat += 0.75f * Time.deltaTime;
        if (volumeFloat > 0.75f) volumeFloat -= 0.75f * Time.deltaTime;
    }

    public static void AmbientScary()
    {
        //adjusting pitch and volume on ambient audio source
        if (pitchFloat > 0.5f) pitchFloat -= 0.75f * Time.deltaTime;
        if (volumeFloat < 1f) volumeFloat += 0.75f * Time.deltaTime;
    }

    public static void AmbientHell() 
    {
        //full volume
        if (volumeFloat < 1f) volumeFloat += 0.75f * Time.deltaTime;

        //shifting pitch back and forth
        if (pitchFloat < 3f && !IsPitchSlide) pitchFloat += 0.5f * Time.deltaTime;
        if(!IsPitchSlide && pitchFloat >= 2.85f) IsPitchSlide = true;
        if (pitchFloat > -3f && IsPitchSlide) pitchFloat -= 0.5f * Time.deltaTime;
        if (IsPitchSlide && pitchFloat <= -2.85f) IsPitchSlide = false;

        //shift stereoPan
        if (stereoPanFloat < 1f && !IsSteroPanSlide) stereoPanFloat += 0.25f * Time.deltaTime;
        if (!IsSteroPanSlide && stereoPanFloat >= 0.90f) IsSteroPanSlide = true;
        if (stereoPanFloat > -1f && IsSteroPanSlide) stereoPanFloat -= 0.25f * Time.deltaTime;
        if (IsSteroPanSlide && stereoPanFloat <= -0.90f) IsSteroPanSlide = false;
    }

    //This funtion picks a random audio track at the start of each level load. Keeps things switched up between 3 different clips
    private void RandomAmbientTrack() 
    {
        int randomIndex = Random.Range(0, ambientClips.Count); //picks random index based on length of audio clips in list
        ambientSource.clip = ambientClips[randomIndex]; // sets that selected clip to the audio source

        PlaySound();
    }

    public static void EndGameAmbient() 
    {
        ForceUpdate = false;
        ForceHell = false;
        AmbientNormal();
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
    public static void StopSound()
    {
        if (thisScript.ambientSource.isPlaying) thisScript.ambientSource.Stop();
    }

}
