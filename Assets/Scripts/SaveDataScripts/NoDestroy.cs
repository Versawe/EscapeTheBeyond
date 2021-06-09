using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.Windows;
using UnityEngine.Assertions.Must;
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
        Debug.Log("OnEnable called");
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        //get current scene
        actualScene = SceneManager.GetActiveScene();

        if (actualScene.name == "Preload")
        {
            SceneManager.LoadScene("MainMenu");
        }
        if (actualScene.name == "MainMenu")
        {
            ResetVarsWhenMainMenu();
        }

        persistantPath = Application.persistentDataPath + "/" + fileLoaded;
        if (fileLoaded == "") return;
        else if (fileLoaded != "")
        {
            if (actualScene.name != "Preload" || actualScene.name != "MainMenu") LoadFromFile();
        }

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

        print(fileName);
        print(fileName.GetType());
        print(gameProgression);
        print(gameProgression.GetType());
        print(pSensitivity);
        print(pSensitivity.GetType());
        print(gameVolume);
        print(gameVolume.GetType());
        print(currSceneName);
        print(currSceneName.GetType());
        print(pSpawnGOName);
        print(pSpawnGOName.GetType());
        print(puzzleOneLoginAttempts);
        print(puzzleOneLoginAttempts.GetType());
        print(dateFileCreated);
        print(dateFileCreated.GetType());
        print(dateFileModified);
        print(dateFileModified.GetType());
    }

    /*public void SaveToFile()
    {

    }*/

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

/* (WILL NOT BE USED IN THIS SCRIPT) (EXAMPLE OF EDITING SAVE FILE THROUGH GAME)(ON SPECIFIC LINE) 
    public void ChangeToCube()
    {
        //grabs the persistant path and file of the selected slot
        string persistantPath = Application.persistentDataPath + "/" + NoDestroy.fileLoaded;
        if (!System.IO.File.Exists(persistantPath)) return; //exits out if the file does not exist

        string newString = "";

        //loop creates a new string and replaces the row that has geometry name in it
        for (int i = 0; i < System.IO.File.ReadAllLines(persistantPath).Length; i++)
        {
            if (i != 1) newString = newString + System.IO.File.ReadAllLines(persistantPath)[i];
            else newString = newString + "\nCube";
        }

        //This deletes the old file and replaces it with the updated new file
        System.IO.File.Delete(persistantPath);
        System.IO.File.WriteAllText(persistantPath, newString);

        //loads scene to start game
        SceneManager.LoadScene("GameScene");
    }*/
