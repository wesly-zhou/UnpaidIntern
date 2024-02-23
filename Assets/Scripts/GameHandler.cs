using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameHandler : MonoBehaviour
{

    public GameObject scoreText;
    private static int playerScore = 0;

    void Start()
    {
        UpdateScore();
    }

    public void StartGame()
    {
        playerScore = 0;
        SceneManager.LoadScene("LevelOne");
    }

    public void Resume()
    {
        playerScore = 0;
        SceneManager.LoadScene("LevelOne");
    }

    public void Credits()
    {
        SceneManager.LoadScene("Credits");
    }

    public void RestartGame()
    {
        // Time.timeScale = 1f;
        // GameHandler_PauseMenu.GameisPaused = false;
        MainMenu();
    }

    public void MainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void QuitGame()
    {
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #else
                Application.Quit(); 
        #endif
    }

    public void AddScore(int points)
    {
        playerScore += points;
        UpdateScore();
    }

    void UpdateScore()
    {
        Text scoreTextB = scoreText.GetComponent<Text>();
        if (SceneManager.GetActiveScene().name == "EndScene")
        {
            scoreTextB.text = "FINAL SCORE: " + playerScore;
        }
        else
        {
            scoreTextB.text = "SCORE: " + playerScore;
        }
    }

}