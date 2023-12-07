using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class QuizManager : MonoBehaviour
{
    public TMP_Text questionTextDisplay;

    public List<Question> questions;
    public GameObject[] options;

    public int currentQuestion = -1;
    public double score = 0;

    private List<string> expectedAnswers = new List<string>{ "1", "2", "3" };
    private List<string> userAnswers;

    private GameManagerScript gameManager;

    public void AddAnswer(string id)
    {
        userAnswers.Add(id);
        GenerateQuestion();
    }

    public void PrepareGame()
    {
        for (int i = 0; i < options.Length; i++)
        {
            options[i].SetActive(true);
        }

        currentQuestion = -1;
        userAnswers = new List<string>();
        GenerateQuestion();
    }

    private void Start()
    {
        gameManager = GameObject.FindWithTag("GameManager").GetComponent<GameManagerScript>();
    }

    private void GenerateQuestion()
    {
        currentQuestion += 1;
        if(currentQuestion == questions.Count){
            CalculateScore();
            gameManager.EndGame();
            return;
        }

        questionTextDisplay.text = questions[currentQuestion].QuestionText;
        SetAlternatives();
    }

    private void SetAlternatives()
    {
        for (int i = 0; i < options.Length; i++)
        {
            options[i].transform.GetChild(0).GetComponent<TMP_Text>().text = questions[currentQuestion].Alternatives[i].Text;
            options[i].GetComponent<AnswerScript>().id = questions[currentQuestion].Alternatives[i].Id;
        }
    }


    private void CalculateScore()
    {
        int m = expectedAnswers.Count;
        int n = userAnswers.Count;

        int[,] matrix = new int[m + 1, n + 1];

        for (int i = 0; i <= m; i++)
        {
            for (int j = 0; j <= n; j++)
            {
                if (i == 0 || j == 0)
                    matrix[i, j] = 0;
                else if (expectedAnswers[i - 1] == userAnswers[j - 1])
                    matrix[i, j] = matrix[i - 1, j - 1] + 1;
                else
                    matrix[i, j] = System.Math.Max(matrix[i - 1, j], matrix[i, j - 1]);
            }
        }

        int lcsLength = matrix[m, n];
        score = ((double)lcsLength / m) * 10;
    }

}
