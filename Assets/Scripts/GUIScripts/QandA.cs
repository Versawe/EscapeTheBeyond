using System.Collections;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class QandA : MonoBehaviour
{
    //reads the text file and puts them into public string lists with matching indexes
    public TextAsset DatabaseTextFile;
    
    public string[] DataBaseSplit;
    public List<string> QuestionsList = new List<string>();
    private List<string> AnswersList = new List<string>();
    private List<string> QTypeList = new List<string>();
    private List<string> OtherOptionsList = new List<string>();
    private List<string> MCChoices = new List<string>();

    public TextMeshProUGUI QuestionText;
    public GameObject MCPanel;
    public GameObject TEPanel;
    public GameObject StrikesPanel;
    public TextMeshProUGUI StrikesText;
    private float strikeCount = 0;
    private float correctAnswers = 0;

    public Button EnterButt;
    public Button ButtA;
    public Button ButtB;
    public Button ButtC;
    public Button ButtD;
    public TextMeshProUGUI textA;
    public TextMeshProUGUI textB;
    public TextMeshProUGUI textC;
    public TextMeshProUGUI textD;
    public TMP_InputField formInput;
    private bool IsInForms = false;

    private string currentQ;
    private string currentA; //for form questions this will contain Regex that will be compared to playerA
    private string questionType;
    private string otherOptions; 
    private string playerA;

    private float wrongTimer = 1.5f;
    private bool WasWrongGuess = false;
    private float rightTimer = 1.5f;
    private bool WasRightGuess = false;

    LookAtStuff lookScript;
    public GameObject PPVOff;

    private void Awake()
    {
        //Creates an array from the text file
        DataBaseSplit = DatabaseTextFile.text.Split('\n');    
        MCChoices.Clear();

        if (GameObject.Find("FPSController")) lookScript = GameObject.Find("FPSController").GetComponentInChildren<LookAtStuff>();
        else lookScript = null;
    }
    void OnEnable()
    {
        if (NoDestroy.currSceneName != "QandA") PPVOff = null;
        //each enable script will resplit the databasesplit array by the '@' delimeter and put the texts in their respective Lists
        for (int i = 0; i < DataBaseSplit.Length - 1; i++)
        {
            string[] newSplit;
            newSplit = DataBaseSplit[i].Split('@');
            QuestionsList.Add(newSplit[0]);
            AnswersList.Add(newSplit[1]);
            QTypeList.Add(newSplit[2]);
            OtherOptionsList.Add(newSplit[3]);
        }
        //Starts off Questions
        RandomQuestion(QuestionsList);
    }

    private void OnDisable()
    {
        //resets all Lists so questions can repeat on next attempt & deactivates GUI's
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
        //to be able to click enter instead of clicking form question buttons
        if (IsInForms && Input.GetKeyDown(KeyCode.Return) && formInput.text != "")
        {
            AnswerQuestion(EnterButt);
        }
        //delay timer after wrong guess (will be used for jumpscares)
        if (WasWrongGuess)
        {
            wrongTimer -= 1 * Time.deltaTime;
        }
        if (wrongTimer <= 0)
        {
            print("Wrong answer");
            strikeCount++;
            WasWrongGuess = false;
            if (CheckEndGUISession()) return;
            RandomQuestion(QuestionsList);
            wrongTimer = 1.5f;
        }

        //delay timer after right guess (used for suspense build)
        if (WasRightGuess)
        {
            rightTimer -= 1 * Time.deltaTime;
        }
        if (rightTimer <= 0)
        {
            print("Correct answer");
            correctAnswers++;
            WasRightGuess = false;
            if(CheckEndGUISession()) return;
            RandomQuestion(QuestionsList);
            rightTimer = 1.5f;
        }

        //sets the number of strikes to the gui display for players
        StrikesText.text = strikeCount.ToString();
    }

    public void RandomQuestion(List<string> list) //made to call for when a random Q&A needs to be generated
    {
        float rand = Random.Range(0, list.Count); //generates random number between 0 and current length of the list (list is always changing)
        formInput.text = "";

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
            IsInForms = true;
            //show correct ui
            TEPanel.SetActive(true);
            MCPanel.SetActive(false);
            formInput.ActivateInputField();
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

    //this function reorders the multiple choice options and answer to randomly display options
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

    //this is used for regular expression checks on form answers
    private bool DoesRegexMatch(string regexString, string checkMatch)
    {
        Regex reg = new Regex(regexString, RegexOptions.IgnoreCase);
        if (reg.IsMatch(checkMatch)) return true;
        else return false;
    }

    //attached to all buttons and used to determine if player's answer is corrrect
    public void AnswerQuestion(Button buttonClicked)
    {
        if(questionType[0] == 'M') // check answer for multiple choice questions
        {
            playerA = buttonClicked.GetComponentInChildren<TextMeshProUGUI>().text;
            if (playerA == currentA)
            {
                MCPanel.SetActive(false);
                WasRightGuess = true;
            }
            else
            {
                MCPanel.SetActive(false);
                WasWrongGuess = true;
            }
        }
        else if (questionType[0] == 'F') // check answer for form questions
        {
            IsInForms = false;
            playerA = formInput.text;
            if (DoesRegexMatch(currentA, playerA))
            {
                TEPanel.SetActive(false);
                WasRightGuess = true;
            }
            else
            {
                TEPanel.SetActive(false);
                WasWrongGuess = true;
            }
            formInput.text = "";
        }
        else //this should never happen
        {
            print("Error line 176 of QandA.cs");
        }
    }
    
    public bool CheckEndGUISession() 
    {
        if (strikeCount >= 3) 
        {
            wrongTimer = 1.5f;
            WasWrongGuess = false;
            NoDestroy.atGameOver = true;
            TEPanel.SetActive(false);
            MCPanel.SetActive(false);
            StrikesPanel.SetActive(false);
            QuestionText.enabled = false;
            return true;
        }
        else if (correctAnswers >= 2) //15 for real # correct for now, keep at 2 for testing purposes
        {
            rightTimer = 1.5f;
            WasRightGuess = false;
            TEPanel.SetActive(false);
            MCPanel.SetActive(false);
            StrikesPanel.SetActive(false);
            QuestionText.enabled = false;
            lookScript.IsActivated = false;
            NoDestroy.completedQandA = true;
            PPVOff.SetActive(false);
            return true;
        }
        else 
        {
            return false;
        }
    }
}
