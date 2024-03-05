using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameHandler : MonoBehaviour
{
    public static int currentLevel = 1;
    public Animator transition;


    public void StartGame()
    {
        StartCoroutine(LoadLevel());
        // SceneManager.LoadScene("Level 1");
    }

    public void Credits()
    {
        SceneManager.LoadScene("Credits");
    }

    public void RestartGame()
    {
        Time.timeScale = 1f;
        GameHandler_PauseMenu.GameisPaused = false;
        MainMenu();
    }

    public void NextLevel()
    {
        SceneManager.LoadScene("Level " + (currentLevel + 1));
        currentLevel++;
    }

    public void MainMenu()
    {
        currentLevel = 1;
        SceneManager.LoadScene("MainMenu");
    }

    public void QuitGame()
    {
        currentLevel = 1;
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #else
                Application.Quit(); 
        #endif
    }
    IEnumerator LoadLevel()
    {
        transition.Play("OutScene");
        yield return new WaitForSeconds(1f);
        SceneManager.LoadScene("Level 1");
    }
}