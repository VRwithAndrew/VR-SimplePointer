using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using TMPro;

public class GameManagerScript : MonoBehaviour
{
    public GameObject endGameText;
    public GameObject scoreText;

    public QuizManager quizManager;

    void Start()
    {
        quizManager.PrepareGame();
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void EndGame()
    {
        SetActiveObjectsWithTag("QuestionnaireAlternatives", false);
        SetActiveObjectsWithTag("QuestionnaireText", false);
        endGameText.SetActive(true);
        scoreText.SetActive(true);

        scoreText.transform.GetChild(0).GetComponent<TMP_Text>().text = "Pontuação: " + System.Math.Round(quizManager.score,2);

    }

    private void SetActiveObjectsWithTag(string tag, bool active)
    {
        GameObject[] objects = GameObject.FindGameObjectsWithTag(tag);

        for (int i = 0; i < objects.Length; i++)
        {
            objects[i].SetActive(active);
        }
    }
}
