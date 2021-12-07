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
    private float endGameTimer = 88f;

    public RelicHuntScript huntScript;
    public static bool collectedAllRelics = false;
    public static bool atGameOver = false;
    public static bool completedQandA = false;
    public static bool atGameComplete = false;
    public static bool hasHuntBegan = false;

    //vars used for player PP
    public static bool TriggerScarePP = false;
    public static bool TriggerScarePPAI = false;

    public static string currObjective = "";

    public static List<GameObject> stairs  = new List<GameObject>();
    public static int stairSpawnCount = 0;

    Light flashLight;
    public JumpScareHides jumpScareScript;

    CharacterMovementFirstPerson moveScript;
    CameraRotationFirstPerson camScript;

    public static bool BigScareHappening = false;

    // Start is called before the first frame update
    void Awake()
    {
        //makes the object this script is attached to non-destroyable on load
        DontDestroyOnLoad(gameObject);
        fileLoaded = "";

        huntScript = GetComponent<RelicHuntScript>();
        jumpScareScript = GetComponent<JumpScareHides>();

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
        hasHuntBegan = false;
        TriggerScarePP = false;
        jumpScareScript.enabled = false;
        TriggerScarePP = false;
        TriggerScarePPAI = false;
        BigScareHappening = false;
        AmbientClipController.pitchFloat = 1f;
        AmbientClipController.volumeFloat = 0.75f;
        AmbientClipController.AmbientNormal();

        if (actualScene.name == "Preload") // instanlty preloads to Main menu
        {
            SceneManager.LoadScene("MainMenu");
        }
        else if (actualScene.name == "MainMenu") // resets variables in here if in main menu
        {
            ResetVarsWhenMainMenu();
            Cursor.lockState = CursorLockMode.None;
            AudioListener.volume = 1;
        }
        else if(actualScene.name == "Between") //between scene
        {
            AmbientClipController.volumeFloat = 0f;
        }
        else if (actualScene.name == "RelicHunt") //RelicHunt scene
        {
            huntScript.enabled = false;
            collectedAllRelics = false;
            atGameOver = false;
            atGameComplete = false;
            hasHuntBegan = false;
            BigScareHappening = false;
            flashLight = GameObject.Find("Flashlight").GetComponent<Light>();
            flashLight.enabled = true;
            currObjective = "Current Objective:\nFind a way out of the house";
            jumpScareScript.enabled = true;
        }
        else if (actualScene.name == "QandA") //QandA scene
        {
            SaveToFile();
            moveScript = GameObject.Find("FPSController").GetComponent<CharacterMovementFirstPerson>();
            camScript = GameObject.Find("FPSController").GetComponent<CameraRotationFirstPerson>();
            atGameOver = false;
            atGameComplete = false;
            completedQandA = false; //change to false
            hasHuntBegan = false;
            BigScareHappening = false;
            stairs.Clear();
            stairSpawnCount = 0;
            endGameTimer = 88;
            if (GameObject.Find("HintLight")) GameObject.Find("HintLight").GetComponent<Light>().enabled = false;
            flashLight = GameObject.Find("Flashlight").GetComponent<Light>();
            flashLight.enabled = true;
            currObjective = "Current Objective:\nGo back to the first room";

        }
        else if (actualScene.name == "HellScene") //for cheaters
        {
            pSensitivity = 1;
            atGameOver = false;
            atGameComplete = false;
            completedQandA = false;
            hasHuntBegan = false;
            BigScareHappening = false;
            currObjective = "Current Objective:\nDie, Cheater";
            AudioListener.volume = 1;
        }
        
        //Below grabs persistanPath and automatically loads text rows from save data file into this script
        persistantPath = Application.persistentDataPath + "/" + fileLoaded;
        if (fileLoaded == "") return;
        else if (fileLoaded != "")
        {
            if (actualScene.name != "Preload" || actualScene.name != "MainMenu") 
            {
                LoadFromFile();
                AudioListener.volume = gameVolume;
            } 
        }

    }
    void Update()
    {
        //below if you have beaten the game and are on EndGame scene
        if (gameProgression == 4) //locks cursor and keeps volume at full, to hear the story scene well
        {
            Cursor.lockState = CursorLockMode.None;
            AudioListener.volume = 1;
        } 

        //before beating QandA level and not traveling down stairs max amount yet
        //WARNING! To change the amount of stairs to go down needs to be changed twice BELOW HERE
        //Also changed on line ~188 in QandA.cs & line ~112 in GameHUDActivations
        if (actualScene.name != "QandA" && stairSpawnCount < 7) return; //5
        if(stairSpawnCount >= 7) //5
        {
            endGameTimer -= 1 * Time.deltaTime;
            if(moveScript) moveScript.enabled = false;
            if (camScript) camScript.enabled = false;
            //black out screen play final audio to game then load credit scene HERE
            // This is handled somewhere ELSE ^ in QandA.cs
        }
        if (endGameTimer <= 0) //changes important value once clip is done playing 
        {
            atGameComplete = true;
            AmbientClipController.EndGameAmbient();
        }

        if(actualScene.name == "Between") Cursor.lockState = CursorLockMode.Locked; //locks cursor on car crash scene
    }

    //This function loads every line from SaveData Text file into a static variable to know where player is in this specific slot
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

    //Updates Player's progress throughout game, by rewriting the Save Data text file with the current static variables
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

    //made to be called in DoorOpen.cs, when player has collected all the relics and attempts to open the Basement Door
    public static void LoadQandAScene()
    {
        gameProgression = 3;
        currSceneName = "QandA";
        AudioController.StopSound();
        SceneManager.LoadScene("QandA");
    }

    //resets staic variables, so they are neutral in main menu
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

        HasBeenTamperedWith = false;
        stairs.Clear();
        stairSpawnCount = 0;
        huntScript.enabled = false;
        collectedAllRelics = false;
        atGameOver = false;
        atGameComplete = false;
        completedQandA = false;
        hasHuntBegan = false;
        BigScareHappening = false;
        TriggerScarePP = false;
        TriggerScarePPAI = false;
    }

    //Used to Destroy Stairs on final scene, saves Memory in scene
    public static void CheckStairCount() 
    {
        if (stairs.Count >= 3) 
        {
            GameObject removeThisStairs = stairs[0];
            stairs.RemoveAt(0);
            Destroy(removeThisStairs);
        }
    }
}
