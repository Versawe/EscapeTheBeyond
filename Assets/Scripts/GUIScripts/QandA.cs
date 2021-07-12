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
    //reads the text file and puts them into public string lists with matching indexes
    public TextAsset DatabaseTextFile;
    
    public string[] DataBaseSplit;
    private List<string> QuestionsList = new List<string>();
    private List<string> AnswersList = new List<string>();
    private List<string> QTypeList = new List<string>();
    private List<string> OtherOptionsList = new List<string>();
    private List<string> MCChoices = new List<string>();

    public TextMeshProUGUI QuestionText;
    public GameObject MCPanel;
    public GameObject TEPanel;
    public GameObject StrikesPanel;

    public Button ButtA;
    public Button ButtB;
    public Button ButtC;
    public Button ButtD;
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
        //Creates an array from the text file
        DataBaseSplit = DatabaseTextFile.text.Split('\n');    
        MCChoices.Clear();
    }
    void OnEnable()
    {
        for (int i = 0; i < DataBaseSplit.Length - 1; i++)
        {
            string[] newSplit;
            newSplit = DataBaseSplit[i].Split('@');
            QuestionsList.Add(newSplit[0]);
            AnswersList.Add(newSplit[1]);
            QTypeList.Add(newSplit[2]);
            OtherOptionsList.Add(newSplit[3]);
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
        StrikesPanel.SetActive(false);
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
        StrikesPanel.SetActive(true);

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

    public void AnswerQuestion(Button buttonClicked)
    {
        if(questionType[0] == 'M') 
        {
            
        }
        else if (questionType[0] == 'M')
        {

        }
        else //this should never happen
        {
            print("Error line 176 of QandA.cs");
        }
    }

    private void WrongQuestion() 
    {
        
    }
}
