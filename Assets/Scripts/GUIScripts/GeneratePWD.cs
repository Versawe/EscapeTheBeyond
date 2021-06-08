using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeneratePWD : MonoBehaviour
{
    private string stringOptions = "AaBbCcDdEeFfGgHhIiJjKkLlMmNnOoPpQqRrSsTtUuVvWwXxYyZz1234567890!@#$%^&*()-_+=";

    public string GeneratePWDFunction(int pwdLength)
    {
        string doorPWD = "";
        char pick;

        //Randomly Generates a password
        for (int i = 0; i < pwdLength; i++)
        {
            float random = UnityEngine.Random.Range(0, stringOptions.Length);
            pick = stringOptions[(int)random];
            doorPWD = doorPWD + pick;
        }

        GUIUtility.systemCopyBuffer = doorPWD;
        return doorPWD;   
    }

}
