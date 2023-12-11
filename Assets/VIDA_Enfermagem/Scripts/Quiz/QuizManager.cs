using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class QuizManager : MonoBehaviour
{
    public GameObject[] options;

    public int currentQuestion = -1;
    public double score = 0;

    private List<string> expectedAnswers;
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

        expectedAnswers = new List<string>() { 
            "Higienizar as mãos",
            "Ler o pedido ou a etiqueta de solicitação do exame",
            "Apresentar-se ao paciente",
            "Identificar o paciente",
            "Explicar o procedimento ao paciente",
            "Certificar-se que o paciente cumpriu as recomendações de acordo com o(s) exame(s) a serem colhidos (jejum, repouso, uso de medicamentos, entre outras)"
        };

        GenerateQuestion();
    }

    private void Start()
    {
        gameManager = GameObject.FindWithTag("GameManager").GetComponent<GameManagerScript>();
    }

    private void GenerateQuestion()
    {
        currentQuestion += 1;
        if(currentQuestion == expectedAnswers.Count){
            CalculateScore();
            gameManager.EndGame();
            return;
        }

        SetAlternatives();
    }

    private void SetAlternatives()
    {
        List<string> alternatives = new List<string>(expectedAnswers);
        alternatives.Remove(expectedAnswers[currentQuestion]);

        System.Random rand = new System.Random();
        alternatives = alternatives.OrderBy(x => rand.Next()).ToList();
        alternatives = alternatives.Take(3).ToList();

        alternatives.Insert(rand.Next(0, alternatives.Count+1), expectedAnswers[currentQuestion]);
        
        for (int i = 0; i < options.Length; i++)
        {
            options[i].transform.GetChild(0).GetComponent<TMP_Text>().text = alternatives[i];
            options[i].GetComponent<AnswerScript>().Value = alternatives[i];
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
