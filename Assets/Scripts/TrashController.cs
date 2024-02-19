using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrashController : MonoBehaviour
{
    private bool pickUpAllowed;
    public GameHandler gameHandlerObj;
    public AudioClip cleanSound;//Add this line to reference the sound effect when cleaning

    void Start()
    {
        if (GameObject.FindWithTag("GameHandler") != null) {
            gameHandlerObj = GameObject.FindWithTag("GameHandler").GetComponent<GameHandler>();
        }
    }
    // Update is called once per frame
    void Update()
    {
        if (pickUpAllowed && Input.GetKeyDown(KeyCode.Space)) {
            PickUp();
            gameHandlerObj.AddScore(1);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.name.Equals("Player")) {
            pickUpAllowed = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.name.Equals("Player")) {
            pickUpAllowed = false;
        }
    }

    private void PickUp()
    {
        //Create a temporary game object to play sound effects
        GameObject tempAudioPlayer = new GameObject("TempAudioPlayer");
        AudioSource audioSource = tempAudioPlayer.AddComponent<AudioSource>();
        //Configure audio source
        audioSource.clip = cleanSound;
        audioSource.playOnAwake = false;
        //play sound
        audioSource.Play();
        //Destroy the temporary audio player object,
        //the delay time is equal to the length of the sound effect
        Destroy(tempAudioPlayer, cleanSound.length);

        Destroy(gameObject);
    }
}
