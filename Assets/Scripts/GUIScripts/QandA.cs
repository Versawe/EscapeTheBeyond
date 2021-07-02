using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.Rendering;

public class QandA : MonoBehaviour
{
    //reads text files and puts them into public string lists with matching indexes
    public TextAsset QuestionsTextFile;
    public TextAsset AnswersTextFile;
    public string[] QuestionsSplit;
    public string[] AnswersSplit;

    public TextMeshProUGUI QuestionText;
    public GameObject MCPanel;
    public GameObject TEPanel;

    private string currentQ;
    private string currentA;
    private string playerA;
    private int[] NoRepeats;
    // Start is called before the first frame update
    void Start()
    {
        QuestionsSplit = QuestionsTextFile.text.Split('@');
        AnswersSplit = AnswersTextFile.text.Split('@');
    }

    private void RandomQuestion(string q, string a) 
    {
        
    }

}
