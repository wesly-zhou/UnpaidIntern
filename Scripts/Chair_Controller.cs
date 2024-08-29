using System.Text.RegularExpressions;
using UnityEditor;
using UnityEngine;

public class Chair_Controller : MonoBehaviour
{

    // public AnimationCurve speedCurve;
    public Animator animator; 
    public float Speed;
    private string animationName = "Chair_loop"; 
    // public int startFrame; 
    // public int endFrame; 
    private int frameRate = 12; 
    private bool isPlaying = false;
    void OnTriggerEnter2D(Collider2D other)
    {
        if ((other.gameObject.CompareTag("PlayerCollider") || (other.gameObject.CompareTag("NPC")) && isPlaying == false))
        {
            string pattern = @"\d+$";
            Match match = Regex.Match(GetComponent<SpriteRenderer>().sprite.name, pattern);
            Debug.Log(match.Value);
            int startFrame = int.Parse(match.Value);
            float normalizedStart = startFrame / (float)(frameRate * 3f);
            // // Claculate the start and end time of the animation
            // float startTime = startFrame / (float)frameRate;
            // float endTime = endFrame / (float)frameRate;

            // Play the animation from the start time
            animator.Play(animationName, 0, normalizedStart);
            animator.speed = Speed;
            // Calculate the duration of the animation
            float duration = Random.Range(2f, 3.5f);


            StartCoroutine(StopAnimationAfterTime(duration));
            
        }
    }
    void Start()
    {
        
    }

    System.Collections.IEnumerator StopAnimationAfterTime(float time)
    {
        isPlaying = true;
        Debug.Log("Waiting for " + time + " seconds");
        // Wait for the specified time
        yield return new WaitForSeconds(time);
        Debug.Log("Stopping the animation");
        // Stop the animation
        animator.speed = 0;
        isPlaying = false;
    }
}