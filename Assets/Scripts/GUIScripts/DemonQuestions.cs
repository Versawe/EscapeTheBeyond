using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DemonQuestions : MonoBehaviour
{
    //reads text files and puts them into public string lists with matching indexes
    public TextAsset QuestionsTextFile;
    public TextAsset AnswersTextFile;

    public string[] QuestionsSplit;
    public string[] AnswersSplit;
    // Start is called before the first frame update
    void Start()
    {
        QuestionsSplit = QuestionsTextFile.text.Split('@');
        AnswersSplit = AnswersTextFile.text.Split('@');

        //print(QuestionsSplit[5]);
        //print(AnswersSplit[5]);
    }

}
