using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnswerScript : MonoBehaviour
{
    public bool isCorrect = false;
    public Quiz quizManager;

    public void Answer()
    {
        if(isCorrect)
        {
            Debug.Log("Correct");
            quizManager.Correct();
        } 
        else
        {
            Debug.Log("Wrong");
        }
    }
}
