using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnswerScript : MonoBehaviour
{
    public string id = "";
    public QuizManager quizManager;

    public void Answer()
    {
        quizManager.AddAnswer(id);
    }
}
