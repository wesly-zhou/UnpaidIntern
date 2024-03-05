using System.Collections;
using System.Collections.Generic;
// using UnityEditor.Search;
using UnityEngine;
using UnityEngine.UI;

public class Interaction_Controller : MonoBehaviour
{
    
    private BoxCollider2D interactionAreaCollider;
    private int itemNum = 0;
    // public static bool isTriggered = false;
    // ------------------- Bubble ------------------------
    public GameObject use_bubble;
    public GameObject finish_bubble;
    public GameObject start_state;
    public GameObject end_state;
    public bool interactable = false;
    public int state = 0; // state 0: static, 1: interacting, 2: finished
    // ----------------------------------------------------

    // ------------------ Processing Bar ------------------
    public Slider ProgressBar;
    public bool isProcessing = false;
    public float totalTime;
    private float currentTime;
    // -----------------------------------------------------

    // ------------------ Inventory sys --------------------
    public Text inventoryText;
    public Animator GetItem_Animation;
    public AnimationClip GetItem;
    // -----------------------------------------------------

    // ------------------ Sound ----------------------------
    private AudioSource audioSource;
    public AudioClip getSound;
    public AudioClip workSound;
    public GameObject Entity;
    // Start is called before the first frame update
    void Start()
    {

        audioSource = GetComponent<AudioSource>();//get sound
        audioSource.Stop();
        interactionAreaCollider = GetComponent<BoxCollider2D>();
        if (interactionAreaCollider == null || !interactionAreaCollider.gameObject.CompareTag("Detect_Area"))
        {
            Debug.LogWarning("Interaction area with 'Detect_Area' tag not found in children.");
        }

    }
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("Player" + other.transform.position.y + transform.name + transform.position.y);
        if (other.CompareTag("Player") && other.transform.position.y > transform.position.y)
        {   
            Debug.Log("Player is above the object");
            // transform.parent.GetComponentInChildren<SpriteRenderer>().sortingOrder = other.GetComponent<SpriteRenderer>().sortingOrder + 5;
        }
        else{
            Debug.Log("Player is below the object");
        }
        if (NPC_Controller.isTriggered && other.gameObject.CompareTag("Player")) return;
        // When object is static, notify user to use it
        if (other.CompareTag("Player") && state == 0)
        {
            NPC_Controller.isTriggered = true;
            use_bubble.SetActive(true);
            interactable = true;
        }
        if (other.CompareTag("Player") && (state == 2 || state == 1))
        {
            NPC_Controller.isTriggered = true;
            interactable = true;
        }
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (NPC_Controller.isTriggered && other.gameObject.CompareTag("Player")) return;
        // When object is static, notify user to use it
        if (other.CompareTag("Player") && state == 0)
        {
            NPC_Controller.isTriggered = true;
            use_bubble.SetActive(true);
            interactable = true;
        }
        if (other.CompareTag("Player") && (state == 2 || state == 1))
        {
            NPC_Controller.isTriggered = true;
            interactable = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player")){
            // transform.parent.GetComponentInChildren<SpriteRenderer>().sortingOrder = other.transform.parent.GetComponent<SpriteRenderer>().sortingOrder - 5;
        }
        
        NPC_Controller.isTriggered = false;
        use_bubble.SetActive(false);
        interactable = false;
    }

    private void ShowProgressBar()
    {
        ProgressBar.gameObject.SetActive(true);
        isProcessing = true;
        currentTime = 0f;
        ProgressBar.maxValue = totalTime;
        ProgressBar.value = currentTime;
    }

    void Update()
    {
        if (isProcessing == true){
            if (currentTime < totalTime)
            {
                currentTime += Time.deltaTime;
                ProgressBar.value = currentTime;
            }
            else
            {
                isProcessing = false;
                ProgressBar.gameObject.SetActive(false);
                finish_bubble.SetActive(true);
                state = 2;
                Entity.GetComponent<Animator>().SetInteger("State", 2);
                // interactable = true;
                end_state.SetActive(true);
                audioSource.Stop();
            }
        }
        // Interact with object
        if(Input.GetKeyDown(KeyCode.F)){
            Debug.Log(NPC_Controller.isTriggered);
            if (interactable == true && state == 0 && isProcessing == false && NPC_Controller.isTriggered == true){
                use_bubble.SetActive(false);
                Entity.GetComponent<Animator>().SetInteger("State", 1);
                
                state = 1;
                audioSource.clip = workSound;
                audioSource.volume = 1f;
                audioSource.Play();
                    
                
                ShowProgressBar();
            }
        

        // Finish interacting with object
            else if(interactable == true && state == 2 && isProcessing == false)
            {
                    if (!audioSource.isPlaying)
                    {
                        audioSource.clip = getSound;
                        audioSource.volume = 0.3f;
                        audioSource.Play();
                    }
                    finish_bubble.SetActive(false);
                    itemNum = int.Parse(inventoryText.text);
                    itemNum++;
                    inventoryText.text = itemNum.ToString();
                    use_bubble.SetActive(true);
                    Entity.GetComponent<Animator>().SetInteger("State", 0);
                    state = 0;
                    end_state.SetActive(false);
                    GetItem_Animation.Play(GetItem.name);
                    
                
            }
    }
    }
}
