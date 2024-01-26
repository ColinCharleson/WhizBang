using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ScoreSystem : MonoBehaviour
{
    private int score;
    public static ScoreSystem instance;

    public TextMeshProUGUI scoreDisplay;
    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    public void UpdateScore(int value)
    {
        score += value;
        scoreDisplay.text = score.ToString();
    }
    public int CheckScore()
    {
        return score;
    }
}
