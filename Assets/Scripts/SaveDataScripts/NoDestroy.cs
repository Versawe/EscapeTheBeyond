using System;
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
    public static bool HasBeenTamperedWith = false;

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

    // Start is called before the first frame update
    void Awake()
    {
        //makes the object this script is attached to non-destroyable on load
        DontDestroyOnLoad(gameObject);

        fileLoaded = "";
    }

    void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode) // runs on every scene load
    {
        //get current scene
        actualScene = SceneManager.GetActiveScene();

        if (actualScene.name == "Preload") // instanlty preloads to Main menu
        {
            SceneManager.LoadScene("MainMenu");
        }
        else if (actualScene.name == "MainMenu") // resets variables in here if in main menu
        {
            ResetVarsWhenMainMenu();
            Cursor.lockState = CursorLockMode.None;
        }
        else // currSceneName var always updates correctly
        {
            currSceneName = actualScene.name;
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
        if (actualScene.name == "Preload" || actualScene.name == "MainMenu") return;

        if(actualScene.name == "GlitchyStart") PuzzleOneTriggers();
    }

    private void PuzzleOneTriggers()
    {
        
    }

    public void LoadFromFile()
    {
        for (int i = 0; i < System.IO.File.ReadAllLines(persistantPath).Length; i++)
        {
            if(i == 0) fileName = System.IO.File.ReadAllLines(persistantPath)[i];
            if(i == 1) gameProgression = int.Parse(System.IO.File.ReadAllLines(persistantPath)[i]);
            if (i == 2) pSensitivity = float.Parse(System.IO.File.ReadAllLines(persistantPath)[i]);
            if(i == 3) gameVolume = float.Parse(System.IO.File.ReadAllLines(persistantPath)[i]);
            if(i == 4) currSceneName = System.IO.File.ReadAllLines(persistantPath)[i];
            if(i == 5) pSpawnGOName = System.IO.File.ReadAllLines(persistantPath)[i];
            if (i == 6) puzzleOneLoginAttempts = int.Parse(System.IO.File.ReadAllLines(persistantPath)[i]);
            if (i == 7) dateFileCreated = System.IO.File.ReadAllLines(persistantPath)[i];
            if (i == 8) dateFileModified = System.IO.File.ReadAllLines(persistantPath)[i];
        }
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
    }
}
