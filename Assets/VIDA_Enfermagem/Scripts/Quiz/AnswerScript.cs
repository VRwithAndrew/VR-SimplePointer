using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnswerScript : MonoBehaviour
{
    public string Value = "";
    private QuizManager QuizManager;
    private GameObject Avatar;

    private void Start()
    {
        QuizManager = GameObject.FindWithTag("QuizManager").GetComponent<QuizManager>();
        Avatar = GameObject.FindWithTag("Avatar");
    }

    public void Answer()
    {
        Avatar.GetComponent<Animator>().Play("greeting", 0, 0.1f);
        QuizManager.AddAnswer(Value);
    }
}
