using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.Assertions.Must;
using UnityEngine.SceneManagement;

//this script is attached to the DontDestroyOnBuild GameObject
//the gameobject and this script is used just to transfer static variables
//this tells the in-game scene what the file game is
public class NoDestroy : MonoBehaviour
{
    //this tracks what file to look for and can be used in all scripts
    public static string fileLoaded;

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

    // Start is called before the first frame update
    void Awake()
    {
        //makes the object this script is attached to non-destroyable on load
        DontDestroyOnLoad(gameObject);

        //get current scene
        Scene currScene = SceneManager.GetActiveScene();

        if (currScene.name == "Preload") 
        {
            SceneManager.LoadScene("MainMenu");
        }
        
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
