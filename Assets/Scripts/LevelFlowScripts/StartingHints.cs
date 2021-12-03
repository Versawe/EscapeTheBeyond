using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

//Script made to control when hints appear on the walls in the begininng of the Game
public class StartingHints : MonoBehaviour
{
    public GameObject hint1;
    public GameObject hint2;
    public GameObject hint3;

    private float timePassing = 0f;
    // Start is called before the first frame update
    void Start()
    {
        timePassing = 0;

        if(NoDestroy.puzzleOneLoginAttempts == 3) 
        {
            hint1.GetComponent<TextMeshPro>().text = "this is out of your CONTROL";
            hint2.GetComponent<TextMeshPro>().text = "we adVise you to not move forward";
            hint3.GetComponent<TextMeshPro>().text = "THEY cRAVE YOUR FLESH! tIME DOES NOT EXIST WHErE YOU'lL BE GOING! RUN FROM THE vOID";
        }
            
        hint1.SetActive(false);
        hint2.SetActive(false);
        hint3.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        timePassing += 1 * Time.deltaTime;
        if (timePassing >= 10f) hint1.SetActive(true);
        if (timePassing >= 20f) hint2.SetActive(true);
        if (timePassing >= 30f) hint3.SetActive(true);
    }
}
