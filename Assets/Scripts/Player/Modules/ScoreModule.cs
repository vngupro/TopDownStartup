using UnityEngine;
using System;

public class ScoreModule : MonoBehaviour
{
    public int Score { get; private set; }

    public event Action<int> ScoreChanged;
    public void AddToScore(int score)
    {
        Score += score;
        ScoreChanged?.Invoke(Score);
    }
    public void RemoveToScore(int score)
    {
        Score = Mathf.Clamp(Score - score, 0 , Score);
        ScoreChanged?.Invoke(Score);
    }
}
