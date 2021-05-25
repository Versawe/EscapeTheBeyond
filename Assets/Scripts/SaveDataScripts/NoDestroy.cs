using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//this script is attached to the DontDestroyOnBuild GameObject
//the gameobject and this script is used just to transfer static variables
//this tells the in-game scene what the file game is
public class NoDestroy : MonoBehaviour
{
    //this tracks what file to look for and can be used in all scripts
    public static string fileLoaded;

    //intance of DDOB object
    private static GameObject instance;
    // Start is called before the first frame update
    void Awake()
    {
        // the dont destroy on build object gets reintiantiated when sent back to the original scene it was loaded on
        // this code creates and intance of the object and then deletes the copy
        if (instance == null)
        {
            instance = gameObject;
        }
        else
        {
            Destroy(gameObject);
        }

        //makes the object this script is attached to non-destroyable on load
        DontDestroyOnLoad(gameObject);
    }
}
