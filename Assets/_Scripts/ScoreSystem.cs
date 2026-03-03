using UnityEngine;
using UnityEngine.UI;


public class ScoreSystem : MonoBehaviour
{
    public Text scoreText;

    public void UpdateScore(int matches, int turns)
    {
        int score = (matches * 100) - (turns * 10);
        if (score < 0) score = 0;

        if (scoreText != null)
            scoreText.text = "Score\n" + score;
    }
}
