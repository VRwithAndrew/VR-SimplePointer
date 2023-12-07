
using UnityEngine;

[System.Serializable]

public class Question : MonoBehaviour
{
    public string QuestionText;
    public Alternative[] Alternatives;
    public int CorrectAlternative;
}
