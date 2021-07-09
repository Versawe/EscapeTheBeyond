using System.Collections;
using System.Text.RegularExpressions;
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
    public string[] OtherOptionsSplit; //also contains regex
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
    private string currentA; //for form questions this will contain Regex that will be compared to playerA
    private string questionType;
    private string otherOptions; 
    private string playerA;

    private void Awake()
    {
        //Creates an array from both of the text files
        QuestionsSplit = QuestionsTextFile.text.Split('\n');
        AnswersSplit = AnswersTextFile.text.Split('\n');
        QTypeSplit = QTypeTextFile.text.Split('\n');
        OtherOptionsSplit = OtherOptionsTextFile.text.Split('\n');
        MCChoices.Clear();
    }
    void OnEnable()
    {
        //creates lists from the arrays
        for (int i = 0; i < QuestionsSplit.Length - 1; i++) 
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

        RandomQuestion(QuestionsList);
    }

    private void OnDisable()
    {
        QuestionsList.Clear();
        AnswersList.Clear();
        QTypeList.Clear();
        OtherOptionsList.Clear();
        QuestionText.gameObject.SetActive(false);
        MCPanel.SetActive(false);
        TEPanel.SetActive(false);
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
        otherOptions = OtherOptionsList[(int)rand];
        
        //check if Multiple choice or text entry
        if(questionType[0] == 'M')
        {
            //show correct ui
            TEPanel.SetActive(false);
            MCPanel.SetActive(true);

            //get the options and answer in a new list in a random order
            string[] otherArray = otherOptions.Split(',');
            List<string> newList = new List<string>();
            foreach (string ele in otherArray)
            {
                newList.Add(ele);
            }
            MCChoices = AddReorderLists(currentA, newList);
            textA.text = MCChoices[0];
            textB.text = MCChoices[1];
            textC.text = MCChoices[2];
            textD.text = MCChoices[3];
        }
        else if(questionType[0] == 'F')
        {
            //show correct ui
            TEPanel.SetActive(true);
            MCPanel.SetActive(false);
        }
        //display correct question
        QuestionText.gameObject.SetActive(true);
        QuestionText.text = currentQ;

        //at end remove i from each list so no repeats and lists will sync back up, and clear mcChoices
        QuestionsList.RemoveAt((int)rand);
        AnswersList.RemoveAt((int)rand);
        QTypeList.RemoveAt((int)rand);
        OtherOptionsList.RemoveAt((int)rand);
        MCChoices.Clear();
    }

    private List<string> AddReorderLists(string answer, List<string> List2) 
    {
        List<string> aList = new List<string>();
        List<string> randomList = new List<string>();

        aList.Add(answer);

        foreach (string s in List2) 
        {
            aList.Add(s);
        }
        for (int i = 0; i < 4; i++) 
        {
            int randomIndex = Random.Range(0, aList.Count);
            randomList.Add(aList[randomIndex]);
            aList.RemoveAt(randomIndex);
        }
        return randomList;
    }

    private bool DoesRegexMatch(string regexString, string checkMatch)
    {
        Regex reg = new Regex(regexString, RegexOptions.IgnoreCase);
        if (reg.IsMatch(checkMatch)) return true;
        else return false;
    }
}
