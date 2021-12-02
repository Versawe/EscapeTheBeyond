using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class GameHUDActivations : MonoBehaviour
{
    //UI Object references
    public GameObject pausePanel;
    public GameObject pausePanel2;
    public TextMeshProUGUI objectiveTextDisplay;
    public GameObject RelicPanel;
    public GameObject HealthPanel;
    public GameObject OptionsPanel;
    public GameObject PasscodePanel;
    public GameObject GameOverPanel;
    public GameObject CreditsPanel;
    public GameObject NotePadHintPanel;
    public GameObject ConfirmationPanel;

    public Slider SensitivitySlider;
    public Slider VolumeSlider;
    public TextMeshProUGUI SliderDisplay1;
    public TextMeshProUGUI SliderDisplay2;
    public TextMeshProUGUI relicsCollectedDisplay;

    public GameObject StaminaBar;

    public GameObject TextHint; //the 2/3 text display in game during puzzle 1
    public GameObject pauseHint;

    //variables for UI to interact with
    public bool isPaused = false;
    private bool optionsOn = false;
    private bool confirmationOn = false;
    public float relicCollected = 0;

    //how script tracks 
    Scene currScene;
    NoDestroy GameControllerScript;

    GeneratePWD genPWD;
    private string doorPwd;
    public TMP_InputField formBar;
    LookAtStuff pLookAtScript;
    CharacterMovementFirstPerson moveScript;

    public QandA Puzzle3Script;

    private float delayTimer = 1.5f;

    SceneAudioPlayer storyPlayer;

    // Start is called before the first frame update
    void Awake()
    {
        //grab stuff
        currScene = SceneManager.GetActiveScene();
        genPWD = GetComponent<GeneratePWD>();

        Puzzle3Script = GetComponent<QandA>();
        storyPlayer = GetComponent<SceneAudioPlayer>();

        GUIAppearPerScene();

        if (GameObject.Find("NoDestroyOBJ")) GameControllerScript = GameObject.Find("NoDestroyOBJ").GetComponent<NoDestroy>();
        else GameControllerScript = null;
                
        if (GameObject.Find("Hint")) TextHint = GameObject.Find("Hint");
        else TextHint = null;

        if (GameObject.Find("FPSController"))
        {
            pLookAtScript = GameObject.Find("FPSController").GetComponentInChildren<LookAtStuff>();
            moveScript = GameObject.Find("FPSController").GetComponent<CharacterMovementFirstPerson>();
        }
        else 
        {
            pLookAtScript = null;
            moveScript = null;
        } 
    }

    private void Start()
    {
        //generates password for door code
        if (NoDestroy.puzzleOneLoginAttempts == 3 && NoDestroy.gameProgression == 1) doorPwd = genPWD.GeneratePWDFunction(15);
        if (NoDestroy.gameProgression == 4) 
        {
            storyPlayer.enabled = true;
            enabled = false;
        } 
    }

    // Update is called once per frame
    void Update()
    {
        if (NoDestroy.atGameComplete) //at game complete
        {
            if (!AudioController.DialogueSource.isPlaying)//after end game audio scene is done playing
            {
                CreditsPanel.SetActive(true);
                NoDestroy.gameProgression = 4;
                NoDestroy.currSceneName = "EndGame";
                Cursor.lockState = CursorLockMode.None;

                return;
            }
        }

        CheckForEndGame();
        if (NoDestroy.atGameOver) return;

        if(NoDestroy.stairSpawnCount < 7) PauseGame(); //5
        GUIAppearPerScene();
        TrackingSlideBars();

        //when you cycle through all the questions from the Q&A.cs script (this will never be used in game due to winning or losing)
        if (pLookAtScript.IsActivated && Puzzle3Script.QuestionsList.Count <= 0 && pLookAtScript.lookingAtName == "Main_mirror")
        {
            pLookAtScript.IsActivated = false;
            Cursor.lockState = CursorLockMode.Locked;
        } 

        //if TextHint GO does or does not exist
        if (!TextHint) return;
        else
        {
            if (NoDestroy.puzzleOneLoginAttempts == 2)
            {
                TextHint.SetActive(true);
                pauseHint.SetActive(true);
                NoDestroy.currObjective = "Current Objective:\nEXIT the game..\nI meant...\nEXIT the room...";
                delayTimer -= 1 * Time.deltaTime;
                if (delayTimer <= 0 && delayTimer > -1) AudioController.PlayDialogueSound(0);
            }
            else
            {
                TextHint.SetActive(false);
                NoDestroy.currObjective = "Current Objective:\nEnter the passcode and unlock the door";
            }

            if (isPaused && NoDestroy.puzzleOneLoginAttempts == 2) pauseHint.SetActive(false);
            else if (!isPaused && NoDestroy.puzzleOneLoginAttempts == 2) pauseHint.SetActive(true);
            else pauseHint.SetActive(false);
        }
    }

    private void TrackingSlideBars() //tracks what the in-game pause slider bars should be set at based on static vars in NoDestroy
    {
        if (optionsOn)
        {
            NoDestroy.pSensitivity = SensitivitySlider.value;
            NoDestroy.gameVolume = VolumeSlider.value/10;
            float fullNum = NoDestroy.gameVolume * 10;
            SliderDisplay1.text =fullNum.ToString();
            SliderDisplay2.text = NoDestroy.pSensitivity.ToString();
            AudioListener.volume = NoDestroy.gameVolume;
        } //controls stamina slider being visual or now and locking in value
        if (moveScript.staminaActual < 3.5f && !moveScript.IsExhausted && !isPaused && moveScript.isActiveAndEnabled)
        {
            StaminaBar.SetActive(true);
            StaminaBar.GetComponent<Slider>().value = moveScript.staminaActual;
        }
        else
        {
            StaminaBar.SetActive(false);
        }
        
    }

    private void GUIAppearPerScene() //Makes Relic Hunt UI Elements enabled if playing relicHunt scene
    {
        if (currScene.name == "RelicHunt" && NoDestroy.hasHuntBegan && !isPaused) //displays relic hud in certain scenes
        {
            RelicPanel.SetActive(true);
            HealthPanel.SetActive(true);
            relicsCollectedDisplay.text = relicCollected.ToString();
        }
        else if (currScene.name == "RelicHunt" && NoDestroy.hasHuntBegan && isPaused) //hide in-game HUD if game is paused
        {
            RelicPanel.SetActive(false);
            HealthPanel.SetActive(false);
        }
        else if(currScene.name != "RelicHunt")
        {
            RelicPanel.SetActive(false);
            HealthPanel.SetActive(false);
        }
    }

    //logic for pausing game
    private void PauseGame()
    {
        bool escClick = Input.GetKeyDown("escape");
        if (Input.GetKeyDown("escape")) AudioController.ClickSound();

        if (escClick && !isPaused && !pLookAtScript.IsActivated)
        {
            isPaused = true;
            AudioController.PauseSound();
        }
        else if (escClick && isPaused)
        {
            isPaused = false;
            AudioController.UnPauseSound();
            ConfirmationPanel.SetActive(false);
            Cursor.lockState = CursorLockMode.Locked; // Don't really need it in EDITOR esc auto unlocks cursor. Unpausing should work on build
        }

        if (isPaused && !optionsOn && !confirmationOn)
        {
            Cursor.lockState = CursorLockMode.None;
            pausePanel.SetActive(true);
            pausePanel2.SetActive(true);
            objectiveTextDisplay.text = NoDestroy.currObjective;
            Time.timeScale = 0;
        } 
        else if (isPaused && confirmationOn) 
        {
            pausePanel.SetActive(false);
            pausePanel2.SetActive(false);
        }
        else if (!isPaused && !pLookAtScript.IsActivated && !NoDestroy.atGameOver)
        {
            if (NoDestroy.gameProgression != 4) Cursor.lockState = CursorLockMode.Locked;
            pausePanel.SetActive(false);
            pausePanel2.SetActive(false);
            optionsOn = false;
            confirmationOn = false;
            OptionsPanel.SetActive(false);
            Time.timeScale = 1f;
        }
    }

    //triggers game options
    //made for button click
    public void GameOptionsTrigger()
    {
        AudioController.ClickSound();
        optionsOn = true;
        pausePanel.gameObject.SetActive(false);
        pausePanel2.gameObject.SetActive(false);
        OptionsPanel.gameObject.SetActive(true);
        SensitivitySlider.value = NoDestroy.pSensitivity;
        VolumeSlider.value = NoDestroy.gameVolume*10;
        SliderDisplay2.text = NoDestroy.pSensitivity.ToString();
        float decNum = NoDestroy.gameVolume / 10;
        SliderDisplay1.text = decNum.ToString();
    }

    //made for button click
    public void BackButtonTrigger()
    {
        AudioController.ClickSound();
        pausePanel.gameObject.SetActive(true);
        OptionsPanel.gameObject.SetActive(false);
        GameControllerScript.SaveToFile();
    }

    //logic for entering passcode and UI enabling/disabling
    public void EnterPasscode()
    {
        AudioController.ClickSound();
        string formGuess = formBar.text;

        if(formGuess == doorPwd) // passcode correct
        {
            NoDestroy.gameProgression += 1;
            NoDestroy.currSceneName = "RelicHunt";
            GameControllerScript.SaveToFile();
            AudioController.StopSound();

            SceneManager.LoadScene(NoDestroy.currSceneName);
        }
        else // passcode incorrect
        {
            Cursor.lockState = CursorLockMode.Locked;
            pLookAtScript.IsActivated = false;
        } 
        formBar.text = "";
    }

    //Game Over Functions
    private void CheckForEndGame()
    {
        if (!NoDestroy.atGameOver) return;
        AudioController.StopSound();
        Cursor.lockState = CursorLockMode.None;
        GameControllerScript.SaveToFile();
        if(!confirmationOn) GameOverPanel.SetActive(true);
        StaminaBar.SetActive(false);
        RelicPanel.SetActive(false);
        HealthPanel.SetActive(false);
    }

    //made for button click, grabs from static var
    public void RetryCurrLevel() 
    {
        AudioController.ClickSound();
        AudioController.StopSound();
        if (NoDestroy.HasBeenTamperedWith) SceneManager.LoadScene("HellScene");
        else SceneManager.LoadScene(NoDestroy.currSceneName);
    }

    //made for button click
    //opens confirmation window first
    public void ExitToMenu()
    {
        AudioController.ClickSound();
        ConfirmationPanel.SetActive(true);
        confirmationOn = true;
        if (!NoDestroy.atGameOver)
        {
            pausePanel.SetActive(false);
            pausePanel2.SetActive(false);
        }
        else
        {
            GameOverPanel.SetActive(false);
        }
    }
    //made for button click
    //exits to main menu
    public void ExitToMenuConfirm() 
    {
        AudioController.ClickSound();
        isPaused = false;
        Time.timeScale = 1f;

        if (NoDestroy.puzzleOneLoginAttempts == 2) NoDestroy.puzzleOneLoginAttempts = 3;

        GameControllerScript.SaveToFile();
        NoDestroy.fileLoaded = "";
        AudioController.StopSound();
        SceneManager.LoadScene("MainMenu");
    }
    //made for button click goes back to first pause panel
    public void CancelExitToMenu() 
    {
        AudioController.ClickSound();
        ConfirmationPanel.SetActive(false);
        confirmationOn = false;
        if (!NoDestroy.atGameOver) 
        {
            pausePanel.SetActive(true);
            pausePanel2.SetActive(true);
        }
        else 
        {
            GameOverPanel.SetActive(true);
        }
    }
}
