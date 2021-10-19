using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class GameHUDActivations : MonoBehaviour
{
    //UI Object references
    public GameObject pausePanel;
    public TextMeshProUGUI objectiveTextDisplay;
    public GameObject RelicPanel;
    public GameObject HealthPanel;
    public GameObject OptionsPanel;
    public GameObject PasscodePanel;
    public GameObject GameOverPanel;
    public GameObject CreditsPanel;
    public Slider SensitivitySlider;
    public Slider VolumeSlider;
    public TextMeshProUGUI SliderDisplay1;
    public TextMeshProUGUI SliderDisplay2;
    public TextMeshProUGUI relicsCollectedDisplay;

    public GameObject TextHint; //the 2/3 text display in game during puzzle 1 

    //variables for UI to interact with
    public bool isPaused = false;
    private bool optionsOn = false;
    public float relicCollected = 0;

    //how script tracks 
    Scene currScene;
    NoDestroy GameControllerScript;

    GeneratePWD genPWD;
    private string doorPwd;
    public TMP_InputField formBar;
    LookAtStuff pLookAtScript;

    public QandA Puzzle3Script;

    private float delayTimer = 1.5f;

    // Start is called before the first frame update
    void Awake()
    {
        currScene = SceneManager.GetActiveScene();
        genPWD = GetComponent<GeneratePWD>();

        Puzzle3Script = GetComponent<QandA>();

        GUIAppearPerScene();

        if (GameObject.Find("NoDestroyOBJ")) GameControllerScript = GameObject.Find("NoDestroyOBJ").GetComponent<NoDestroy>();
        else GameControllerScript = null;

        if (GameObject.Find("Hint")) TextHint = GameObject.Find("Hint");
        else TextHint = null;

        if (GameObject.Find("FPSController")) pLookAtScript = GameObject.Find("FPSController").GetComponentInChildren<LookAtStuff>();
        else pLookAtScript = null;
    }

    private void Start()
    {
        if (NoDestroy.puzzleOneLoginAttempts == 3 && NoDestroy.gameProgression == 1) doorPwd = genPWD.GeneratePWDFunction(15);
    }

    // Update is called once per frame
    void Update()
    {
        if (NoDestroy.atGameComplete) //at game complete
        {
            CreditsPanel.SetActive(true);
            Cursor.lockState = CursorLockMode.None;
            return;   
        }

        CheckForEndGame();
        if (NoDestroy.atGameOver) return;

        if(NoDestroy.stairSpawnCount < 15) PauseGame();
        GUIAppearPerScene();
        TrackingSlideBars();

        //when you cycle through all the questions from the Q&A.cs script (this will never be used in game due to winning or losing)
        if (pLookAtScript.IsActivated && Puzzle3Script.QuestionsList.Count <= 0 && pLookAtScript.lookingAtName == "Main_mirror")
        {
            pLookAtScript.IsActivated = false;
            Cursor.lockState = CursorLockMode.Locked;
        } 

        //if TextHint GO does or does not exist
        if (TextHint == null) return;
        else
        {
            if (NoDestroy.puzzleOneLoginAttempts != 2 && TextHint != null)
            {
                TextHint.SetActive(false);
                NoDestroy.currObjective = "Current Objective:\nEnter the passcode and unlock the door";
            }
            else
            {
                TextHint.SetActive(true);
                NoDestroy.currObjective = "Current Objective:\nEXIT the room";
                delayTimer -= 1 * Time.deltaTime;
                if (delayTimer <= 0 && delayTimer > -1) AudioController.PlayDialogueSound(0);
                

            }
        }

    }

    private void TrackingSlideBars()
    {
        if (optionsOn)
        {
            NoDestroy.pSensitivity = SensitivitySlider.value;
            NoDestroy.gameVolume = VolumeSlider.value/10;
            float fullNum = NoDestroy.gameVolume * 10;
            SliderDisplay1.text =fullNum.ToString();
            SliderDisplay2.text = NoDestroy.pSensitivity.ToString();
            AudioListener.volume = NoDestroy.gameVolume;
        }
    }

    private void GUIAppearPerScene()
    {
        if (currScene.name == "RelicHunt" || currScene.name == "HellScene")
        {
            RelicPanel.SetActive(true);
            HealthPanel.SetActive(true);
            relicsCollectedDisplay.text = relicCollected.ToString();
        }
        else
        {
            RelicPanel.SetActive(false);
            HealthPanel.SetActive(false);
        }
    }

    private void PauseGame()
    {
        bool escClick = Input.GetKeyDown("escape");

        if (escClick && !isPaused && !pLookAtScript.IsActivated)
        {
            isPaused = true;
            AudioController.PauseSound();
        }
        else if (escClick && isPaused)
        {
            isPaused = false;
            AudioController.UnPauseSound();
            Cursor.lockState = CursorLockMode.Locked; // Don't really need it in EDITOR esc auto unlocks cursor. Unpausing should work on build
        }

        if (isPaused && !optionsOn)
        {
            Cursor.lockState = CursorLockMode.None;
            pausePanel.SetActive(true);
            objectiveTextDisplay.text = NoDestroy.currObjective;
            Time.timeScale = 0;
        }
        else if (!isPaused && !pLookAtScript.IsActivated && !NoDestroy.atGameOver)
        {
            Cursor.lockState = CursorLockMode.Locked;
            pausePanel.SetActive(false);
            optionsOn = false;
            OptionsPanel.SetActive(false);
            Time.timeScale = 1f;
        }
    }

    public void GameOptionsTrigger()
    {
        optionsOn = true;
        pausePanel.gameObject.SetActive(false);
        OptionsPanel.gameObject.SetActive(true);
        SensitivitySlider.value = NoDestroy.pSensitivity;
        VolumeSlider.value = NoDestroy.gameVolume*10;
        SliderDisplay2.text = NoDestroy.pSensitivity.ToString();
        float decNum = NoDestroy.gameVolume / 10;
        SliderDisplay1.text = decNum.ToString();
    }

    public void BackButtonTrigger()
    {
        pausePanel.gameObject.SetActive(true);
        OptionsPanel.gameObject.SetActive(false);
    }

    public void ExitToMenu()
    {
        isPaused = false;
        Time.timeScale = 1f;

        if (NoDestroy.puzzleOneLoginAttempts == 2) NoDestroy.puzzleOneLoginAttempts = 3;

        GameControllerScript.SaveToFile();
        NoDestroy.fileLoaded = "";
        AudioController.StopSound();
        SceneManager.LoadScene("MainMenu");
    }

    public void EnterPasscode()
    {
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
        GameOverPanel.SetActive(true);
    }

    public void RetryCurrLevel() 
    {
        AudioController.StopSound();
        if (NoDestroy.HasBeenTamperedWith) SceneManager.LoadScene("HellScene");
        else SceneManager.LoadScene(NoDestroy.currSceneName);
    }
}
