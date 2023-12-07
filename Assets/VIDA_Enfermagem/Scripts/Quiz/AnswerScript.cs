using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnswerScript : MonoBehaviour
{
    public string Value = "";
    private QuizManager QuizManager;

    private void Start()
    {
        QuizManager = GameObject.FindWithTag("QuizManager").GetComponent<QuizManager>();
    }

    public void Answer()
    {
        QuizManager.AddAnswer(Value);
    }
}
