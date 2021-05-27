using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
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
    private string fileName;
    private int gameProgression;
    private float pSensitivity;
    private float gameVolume;
    private string currSceneName;
    private string pSpawnGOName;
    private int puzzleOneLoginAttempts;
    private int puzzleTwoRelicsCollected;
    private int puzzleThreeQuestionsCorrect;
    private string dateFileCreated;
    private string dateFileModified;

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
