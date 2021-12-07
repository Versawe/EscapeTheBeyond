using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//This script is what generates the random passcode for the door in the GlitchyStart scene
//and will copy it to you clipboard
public class GeneratePWD : MonoBehaviour
{
    private string stringOptions = "AaBbCcDdEeFfGgHhIiJjKkLlMmNnOoPpQqRrSsTtUuVvWwXxYyZz1234567890!@#$%^&*()-_+= "; //useable chars for pwd

    public string GeneratePWDFunction(int pwdLength) //takes in length of pwd
    {
        string doorPWD = "";
        char pick;

        //Randomly Generates a password
        for (int i = 0; i <= pwdLength; i++) //makes random pwd
        {
            float random = UnityEngine.Random.Range(0, stringOptions.Length); //grab random number with max length being char count on stringOptions
            pick = stringOptions[(int)random]; //grabs the place in the string and puts into pick
            doorPWD = doorPWD + pick; //appends to the string, then repeats the loop
        }

        GUIUtility.systemCopyBuffer = doorPWD; //copies to clipboard
        return doorPWD; //returns so we know the answer
    }
}