using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
//this script is attached to the DontDestroyOnBuild GameObject
//the gameobject and this script is used just to transfer static variables
//this tells the in-game scene what the file game is
public class NoDestroy : MonoBehaviour
{
    //this tracks what file to look for and can be used in all scripts
    public static string fileLoaded = "";

    //bool to check for tampered save data
    public static bool HasBeenTamperedWith = false; //used only in SlotSave.cs (probably won't need because i could load prank level from that script)

    //Save Data Variables (IN ORDER ON TXT FILE)
    public static string fileName;
    public static int gameProgression;
    public static float pSensitivity;
    public static float gameVolume;
    public static string currSceneName;
    public static string pSpawnGOName;
    public static int puzzleOneLoginAttempts;
    public static string dateFileCreated;
    public static string dateFileModified;

    Scene actualScene;

    string persistantPath;
    private float endGameTimer = 15;

    private PlayerHealth pHealth;
    public RelicHuntScript huntScript;
    public static bool collectedAllRelics = false;
    public static bool atGameOver = false;
    public static bool completedQandA = false;
    public static bool atGameComplete = false;

    public static string currObjective = "";

    public static List<GameObject> stairs  = new List<GameObject>();
    public static int stairSpawnCount = 0;

    public static bool playOnce = false;

    Light flashLight;

    //Audio Vars
    public AudioSource EventAS;
    public AudioClip StaticNoiseClip;

    // Start is called before the first frame update
    void Awake()
    {
        //makes the object this script is attached to non-destroyable on load
        DontDestroyOnLoad(gameObject);
        fileLoaded = "";
        EventAS = GetComponent<AudioSource>();
        huntScript = GetComponent<RelicHuntScript>();
    }
    private void Start()
    {
        fileLoaded = "";
        huntScript = GetComponent<RelicHuntScript>();
    }

    void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode) // runs on every scene load
    {
        //get current scene
        actualScene = SceneManager.GetActiveScene();
        currSceneName = actualScene.name;
        atGameOver = false;
        atGameComplete = false;
        completedQandA = false;
        playOnce = false;

        if (actualScene.name == "Preload") // instanlty preloads to Main menu
        {
            SceneManager.LoadScene("MainMenu");
        }
        else if (actualScene.name == "MainMenu") // resets variables in here if in main menu
        {
            ResetVarsWhenMainMenu();
            Cursor.lockState = CursorLockMode.None;
        }
        else if (actualScene.name == "RelicHunt")
        {
            huntScript.enabled = false;
            collectedAllRelics = false;
            atGameOver = false;
            atGameComplete = false;
            pHealth = GameObject.Find("FPSController").GetComponent<PlayerHealth>();
            pHealth.enabled = true;
            flashLight = GameObject.Find("Flashlight").GetComponent<Light>();
            flashLight.enabled = true;
            currObjective = "Current Objective:\nFind a way out of the house";
        }
        else if (actualScene.name == "QandA")
        {
            SaveToFile();
            atGameOver = false;
            atGameComplete = false;
            completedQandA = false; //change to false
            stairs.Clear();
            stairSpawnCount = 0;
            endGameTimer = 15;
            if(GameObject.Find("HintLight")) GameObject.Find("HintLight").GetComponent<Light>().enabled = false;
            flashLight = GameObject.Find("Flashlight").GetComponent<Light>();
            flashLight.enabled = true;
            currObjective = "Current Objective:\nGo back to the first room";
        }
        else if (actualScene.name == "HellScene")
        {
            pSensitivity = 1;
            atGameOver = false;
            atGameComplete = false;
            completedQandA = false;
            currObjective = "Current Objective:\nDie, Cheater";
        }
        
        //Below grabs persistanPath and automatically loads text rows from save data file into this script
        persistantPath = Application.persistentDataPath + "/" + fileLoaded;
        if (fileLoaded == "") return;
        else if (fileLoaded != "")
        {
            if (actualScene.name != "Preload" || actualScene.name != "MainMenu") LoadFromFile();
        }

    }

    void Update()
    {
        if (actualScene.name != "QandA" && stairSpawnCount < 15) return;
        if(stairSpawnCount >= 15)
        {
            endGameTimer -= 1 * Time.deltaTime;
            //black out screen play final audio to game then load credit scene HERE
        }
        if (endGameTimer <= 0) 
        {
            atGameComplete = true;
        }

    }

    public void LoadFromFile()
    {
        fileName = System.IO.File.ReadAllLines(persistantPath)[0];
        gameProgression = int.Parse(System.IO.File.ReadAllLines(persistantPath)[1]);
        pSensitivity = float.Parse(System.IO.File.ReadAllLines(persistantPath)[2]);
        gameVolume = float.Parse(System.IO.File.ReadAllLines(persistantPath)[3]);
        currSceneName = System.IO.File.ReadAllLines(persistantPath)[4];
        pSpawnGOName = System.IO.File.ReadAllLines(persistantPath)[5];
        puzzleOneLoginAttempts = int.Parse(System.IO.File.ReadAllLines(persistantPath)[6]);
        dateFileCreated = System.IO.File.ReadAllLines(persistantPath)[7];
        dateFileModified = System.IO.File.ReadAllLines(persistantPath)[8];
    }

    public void SaveToFile()
    {
        if (!System.IO.File.Exists(persistantPath)) return; //exits out if the file does not exist

        string newString = "";

        //Re-creates new file for a saving function
        newString = fileName + "\n" + gameProgression.ToString() + "\n" + pSensitivity.ToString() + "\n" + 
            gameVolume.ToString() + "\n" + currSceneName + "\n" + pSpawnGOName + "\n" + puzzleOneLoginAttempts.ToString();

        //updates date modified and created, so saving is not considered tampering
        DateTime dateCreated = System.IO.File.GetCreationTime(persistantPath);
        DateTime dateModified = System.IO.File.GetLastWriteTime(persistantPath);
        dateFileCreated = dateCreated.ToString("O").Substring(0, 18);
        dateFileModified = dateModified.ToString("O").Substring(0, 18);

        //This deletes the old file and replaces it with the updated new file
        System.IO.File.Delete(persistantPath);
        System.IO.File.WriteAllText(persistantPath, newString + "\n" + dateFileCreated + "\n" + dateFileModified);

        //This helps us know that the game changed the save data file, not the player in the explorer
        System.IO.File.SetCreationTime(persistantPath, dateCreated);
        System.IO.File.SetLastWriteTime(persistantPath, dateModified);
    }

    public static void LoadQandAScene()
    {
        gameProgression = 3;
        currSceneName = "QandA";
        SceneManager.LoadScene("QandA");
    }

    public void ResetVarsWhenMainMenu()
    {
        fileName = "";
        gameProgression = 0;
        pSensitivity = 0;
        gameVolume = 0;
        currSceneName = "";
        pSpawnGOName = "";
        puzzleOneLoginAttempts = 0;
        dateFileCreated = "";
        dateFileModified = "";
        currObjective = "";

        playOnce = false;
        HasBeenTamperedWith = false;
        stairs.Clear();
        stairSpawnCount = 0;
        huntScript.enabled = false;
        collectedAllRelics = false;
        atGameOver = false;
        atGameComplete = false;
        completedQandA = false;
    }

    public static void CheckStairCount() 
    {
        if (stairs.Count >= 3) 
        {
            GameObject removeThisStairs = stairs[0];
            stairs.RemoveAt(0);
            Destroy(removeThisStairs);
        }
    }

    public void RunNoDestroyAudio(AudioClip thisClip)
    {
        EventAS.clip = thisClip;
        EventAS.Play();
        playOnce = true;
    }
}
