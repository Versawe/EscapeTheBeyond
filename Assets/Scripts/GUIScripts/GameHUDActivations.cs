using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class GameHUDActivations : MonoBehaviour
{
    //UI Object references
    public GameObject pausePanel;
    public GameObject RelicPanel;
    public GameObject HealthPanel;
    public GameObject OptionsPanel;
    public GameObject PasscodePanel;
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

    // Start is called before the first frame update
    void Awake()
    {
        currScene = SceneManager.GetActiveScene();
        genPWD = GetComponent<GeneratePWD>();
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
        PauseGame();
        GUIAppearPerScene();
        TrackingSlideBars();

        if (TextHint == null) return;
        else
        {
            if (NoDestroy.puzzleOneLoginAttempts != 2 && TextHint != null)
            {
                TextHint.SetActive(false);
            }
            else TextHint.SetActive(true);
        }

    }

    private void TrackingSlideBars()
    {
        if (optionsOn)
        {
            NoDestroy.pSensitivity = SensitivitySlider.value;
            NoDestroy.gameVolume = VolumeSlider.value;
            SliderDisplay2.text = NoDestroy.pSensitivity.ToString();
            SliderDisplay1.text = NoDestroy.gameVolume.ToString();
        }
    }

    private void GUIAppearPerScene()
    {
        if (currScene.name == "RelicHunt")
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
        }
        else if (escClick && isPaused)
        {
            isPaused = false;
        }

        if (isPaused && !optionsOn)
        {
            Cursor.lockState = CursorLockMode.None;
            pausePanel.SetActive(true);
            Time.timeScale = 0;
        }
        else if (!isPaused && !pLookAtScript.IsActivated)
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
        VolumeSlider.value = NoDestroy.gameVolume;
        SliderDisplay2.text = NoDestroy.pSensitivity.ToString();
        SliderDisplay1.text = NoDestroy.gameVolume.ToString();
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

            SceneManager.LoadScene(NoDestroy.currSceneName);
        }
        else // passcode incorrect
        {
            Cursor.lockState = CursorLockMode.Locked;
            pLookAtScript.IsActivated = false;
        } 
        formBar.text = "";
    }
}
