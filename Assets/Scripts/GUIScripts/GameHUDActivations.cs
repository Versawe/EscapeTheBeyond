using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;
using TMPro;

public class GameHUDActivations : MonoBehaviour
{
    public GameObject pausePanel;
    public GameObject RelicPanel;
    public GameObject HealthPanel;
    public GameObject OptionsPanel;
    public TextMeshProUGUI relicsCollectedDisplay;

    public bool isPaused = false;
    private bool optionsOn = false;

    public float relicCollected = 0;

    Scene currScene;

    GeneratePWD genPWD;
    private string doorPwd;
    // Start is called before the first frame update
    void Awake()
    {
        currScene = SceneManager.GetActiveScene();
        GUIAppearPerScene();
        genPWD = GetComponent<GeneratePWD>();
    }

    // Update is called once per frame
    void Update()
    {
        PauseGame();
        GUIAppearPerScene();
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
            UnityEngine.Cursor.lockState = CursorLockMode.None;
            pausePanel.SetActive(true);
            Time.timeScale = 0;
        }
        else if (!isPaused)
        {
            UnityEngine.Cursor.lockState = CursorLockMode.Locked;
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
    }

    public void BackButtonTrigger()
    {
        pausePanel.gameObject.SetActive(true);
        OptionsPanel.gameObject.SetActive(false);
    }
}
