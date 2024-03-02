using UnityEngine;

public class NPC_get_angry : MonoBehaviour
{
    private Animator animator;
    private float waitTimer = 0f;

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        // Increment the timer by the time passed since the last frame
        waitTimer += Time.deltaTime;

        // Update the wait_time parameter in the Animator with the current timer value
        animator.SetFloat("wait_time", waitTimer);
    }
}