using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//this is put on guievent gameobjects to help LookAt.cs identify interactable objects
public class GUIEvent : MonoBehaviour
{
    public bool ignoreMe = false;
    private void Start() //we want to ignore the mirror as an interactable object unless it is the QandA scene
    {
        if (gameObject.name == "Main_mirror" && NoDestroy.currSceneName != "QandA") ignoreMe = true;
    }
}
