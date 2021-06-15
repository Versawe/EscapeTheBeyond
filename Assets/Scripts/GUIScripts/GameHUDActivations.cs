using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class GameHUDActivations : MonoBehaviour
{
    public GameObject pausePanel;
    public GameObject RelicPanel;
    public GameObject HealthPanel;
    public GameObject OptionsPanel;
    public Slider SensitivitySlider;
    public Slider VolumeSlider;
    public TextMeshProUGUI SliderDisplay1;
    public TextMeshProUGUI SliderDisplay2;

    public TextMeshProUGUI relicsCollectedDisplay;

    public GameObject TextHint;

    public bool isPaused = false;
    private bool optionsOn = false;

    public float relicCollected = 0;

    Scene currScene;
    NoDestroy GameControllerScript;

    
    private string doorPwd;
    // Start is called before the first frame update
    void Awake()
    {
        currScene = SceneManager.GetActiveScene();
        GUIAppearPerScene();
        if (GameObject.Find("NoDestroyOBJ")) GameControllerScript = GameObject.Find("NoDestroyOBJ").GetComponent<NoDestroy>();
        else GameControllerScript = null;

        if (GameObject.Find("2 out 3"))
        {
            TextHint = GameObject.Find("2 out 3");
            if (NoDestroy.puzzleOneLoginAttempts == 2) TextHint.SetActive(true);
            else TextHint.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        PauseGame();
        GUIAppearPerScene();
        TrackingSlideBars();
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

        if (escClick && !isPaused)
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
        else if (!isPaused)
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
}
