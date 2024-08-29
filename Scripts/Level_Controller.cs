using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Level_Controller : MonoBehaviour
{
    // Start is called before the first frame update
    public Animator transition;
    void Start()
    {
        if (SceneManager.GetActiveScene().name != "MainMenu")
        transition.Play("IntoScene");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
