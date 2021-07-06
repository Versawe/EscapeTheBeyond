using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.Rendering;
using System.Linq;

public class QandA : MonoBehaviour
{
    //reads text files and puts them into public string lists with matching indexes
    public TextAsset QuestionsTextFile;
    public TextAsset AnswersTextFile;
    public TextAsset QTypeTextFile;
    public TextAsset OtherOptionsTextFile;
    public string[] QuestionsSplit;
    public string[] AnswersSplit;
    public string[] QTypeSplit;
    public string[] OtherOptionsSplit;
    private List<string> QuestionsList = new List<string>();
    private List<string> AnswersList = new List<string>();
    private List<string> QTypeList = new List<string>();
    private List<string> OtherOptionsList = new List<string>();
    private List<string> MCChoices = new List<string>();

    public TextMeshProUGUI QuestionText;
    public GameObject MCPanel;
    public GameObject TEPanel;
    public TextMeshProUGUI textA;
    public TextMeshProUGUI textB;
    public TextMeshProUGUI textC;
    public TextMeshProUGUI textD;

    private string currentQ;
    private string currentA;
    private string questionType;
    private string playerA;

    // Start is called before the first frame update
    void Start()
    {
        //Creates an array from both of the text files
        QuestionsSplit = QuestionsTextFile.text.Split('@');
        AnswersSplit = AnswersTextFile.text.Split('@');

        //creates lists from the arrays
        for(int i = 0; i < QuestionsSplit.Length - 1; i++) 
        {
            QuestionsList.Add(QuestionsSplit[i]);
        }
        for (int i = 0; i < AnswersSplit.Length - 1; i++)
        {
            AnswersList.Add(AnswersSplit[i]);
        }
        for (int i = 0; i < QTypeSplit.Length - 1; i++)
        {
            QTypeList.Add(QTypeSplit[i]);
        }
        for (int i = 0; i < OtherOptionsSplit.Length - 1; i++)
        {
            OtherOptionsList.Add(OtherOptionsSplit[i]);
        }
    }

    private void Update()
    {
        //setup for testing when function is called
        if (Input.GetKeyUp("r") && QuestionsList.Count > 0) 
        {
            RandomQuestion(QuestionsList);
        }
        else if(Input.GetKeyUp("r") && QuestionsList.Count <= 0) // not needed
        {
            print("out");
        }
    }

    public void RandomQuestion(List<string> list) //made to call for when a random Q&A needs to be generated
    {
        float rand = Random.Range(0, list.Count); //generates random number between 0 and current length of the list (list is always changing)

        //grabs random number from 0th ele to list.Count
        currentQ = QuestionsList[(int) rand];
        currentA = AnswersList[(int) rand];
        questionType = QTypeList[(int) rand];

        //check if Multiple choice or text entry

        //orient a mixture of the answer and the options to be placed in A,B,C, and D button's text (Exclude if text entry question)

        //Display GUI elements correctly

        //at end remove i from each list so no repeats and lists will sync back up, and clear mcChoices
        QuestionsList.RemoveAt((int)rand);
        AnswersList.RemoveAt((int)rand);
        QTypeList.RemoveAt((int)rand);
        OtherOptionsList.RemoveAt((int)rand);
        MCChoices.Clear();

        //print(currentQ);
        //print(currentA);
    }

}
