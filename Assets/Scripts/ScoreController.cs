using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ScoreController : MonoBehaviour
{
    public TMPro.TextMeshProUGUI scoreText;
    private int currentScore = 0;
    public void IncrementScore()
    {
        ++currentScore;
        UpdateText();
    }

    public void SetScore(int value)
    {
        currentScore = value;
        UpdateText();
    }

    private void UpdateText()
    {
        scoreText.text = currentScore.ToString();
    }
}
