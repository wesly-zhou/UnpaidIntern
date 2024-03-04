using UnityEngine;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour
{
    public int score = 0; // score
    public Text scoreText; // score text

    // Define a delegate
    public delegate void ScoreEventHandler(float remainingTimes);
    public event ScoreEventHandler OnScoreIncreased;

    void Start()
    {
        // Set the initial score
        UpdateScoreText();
        
        // subscribe to the event
        OnScoreIncreased += IncreaseScore;
    }

    void IncreaseScore(float remainingTimes)
    {
        score += Mathf.CeilToInt(remainingTimes * 10);
        UpdateScoreText();
    }

    void UpdateScoreText()
    {
        scoreText.text = score.ToString();
    }

    // Trigger the event
    public void TriggerScoreIncrease(float remainingTimes)
    {
        OnScoreIncreased?.Invoke(remainingTimes);
    }

    private void OnDestroy()
    {
        // Cancel the subscription
        OnScoreIncreased -= IncreaseScore;
    }
}