using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class ScoreUI : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI _scoreText;

    public void UpdateScoreText(int score)
    {
        _scoreText.text = score.ToString();
    }
}
