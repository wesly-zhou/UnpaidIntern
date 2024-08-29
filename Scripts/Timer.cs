using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Timer : MonoBehaviour
{
    public GameObject timeText;
    public int gameTime = 60;
    private float gameTimer = 0f;

    void Start() {
        UpdateTime();
    }

    void FixedUpdate()
    {
        gameTimer += Time.fixedDeltaTime;
        if (gameTimer >= 1f) {
            gameTime -= 1;
            gameTimer = 0;
            UpdateTime();
        } 
        if (gameTime <= 0) {
            gameTime = 0;
            SceneManager.LoadScene ("EndScene"); 
        }
    }

    public void UpdateTime(){
        Text timeTextB = timeText.GetComponent<Text>();
        timeTextB.text = "" + gameTime;
    }
}
