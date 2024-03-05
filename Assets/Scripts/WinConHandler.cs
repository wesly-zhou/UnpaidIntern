using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;
using System.Collections;

public class WinConHandler : MonoBehaviour
{
    public int numNPC;
    public int currentLevel;
    private GameObject Player;
    private GameObject mainCamera;
    private AudioSource audioSource;
    public AudioClip winSound;
    void Start()
    {
        currentLevel = SceneManager.GetActiveScene().buildIndex;
        Player = GameObject.FindGameObjectWithTag("PlayerCollider");
        mainCamera = GameObject.FindGameObjectWithTag("MainCamera");
        audioSource = GetComponent<AudioSource>();
        // numNPC = GameObject.FindGameObjectsWithTag("NPC").Length;
    }

    void Update() {

        if (numNPC <= 0 && currentLevel == 9) {
            SceneManager.LoadScene("WinScene");
        }
        else if (numNPC <= 0) {
            // When win, show some text and disable the player movement
            Player.GetComponent<PlayerMovement>().enabled = false;
            mainCamera.GetComponentInChildren<Canvas>().transform.Find("WinText").gameObject.SetActive(true);
            
            // SceneManager.LoadScene("NextLevelScene");
            // ("LoadNextScene", 3f, "NextLevelScene");
            StartCoroutine(LoadNextScene("NextLevelScene"));
        }
    }

    IEnumerator LoadNextScene(String sceneName) {
        audioSource.clip = winSound;
        if(!audioSource.isPlaying) audioSource.Play();
        yield return new WaitForSeconds(3f);
        mainCamera.GetComponentInChildren<Canvas>().transform.Find("WinText").gameObject.SetActive(false);
        Player.GetComponent<PlayerMovement>().enabled = true;

        SceneManager.LoadScene(sceneName);
        
    }

    void FixedUpdate() {
        numNPC = GameObject.FindGameObjectsWithTag("NPC").Length;
    }
}