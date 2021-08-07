using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GUIEvent : MonoBehaviour
{
    public bool ignoreMe = false;
    private void Start()
    {
        if (gameObject.name == "Main_mirror" && NoDestroy.currSceneName != "QandA") ignoreMe = true;
    }
}
