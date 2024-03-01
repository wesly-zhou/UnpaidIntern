using System.Collections;
using System.Collections.Generic;
// using UnityEditor.Search;
using UnityEngine;
using UnityEngine.UI;

public class Interaction_Controller : MonoBehaviour
{
    private BoxCollider2D interactionAreaCollider;
    private int itemNum = 0;
    
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
    // -----------------------------------------------------

    
    public GameObject Entity;
    // Start is called before the first frame update
    void Start()
    {
        
        interactionAreaCollider = GetComponent<BoxCollider2D>();
        if (interactionAreaCollider == null || !interactionAreaCollider.gameObject.CompareTag("Detect_Area"))
        {
            Debug.LogWarning("Interaction area with 'Detect_Area' tag not found in children.");
        }

    }
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        // When object is static, notify user to use it
        if (state == 0)
        {
            use_bubble.SetActive(true);
            interactable = true;
        }
        if (state == 2 || state == 1)
        {
            interactable = true;
        }
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        // When object is static, notify user to use it
        if (state == 0)
        {
            use_bubble.SetActive(true);
            interactable = true;
        }
        if (state == 2 || state == 1)
        {
            interactable = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
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

    void FixedUpdate()
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
            }
        }
        // Interact with object
        if(Input.GetKeyDown(KeyCode.F)){
            if (interactable == true && state == 0 && isProcessing == false){
                use_bubble.SetActive(false);
                Entity.GetComponent<Animator>().SetInteger("State", 1);
                state = 1;
                ShowProgressBar();
            }
        

        // Finish interacting with object
            else if(interactable == true && state == 2 && isProcessing == false)
            {
                
                    finish_bubble.SetActive(false);
                    itemNum++;
                    inventoryText.text = itemNum.ToString();
                    use_bubble.SetActive(true);
                    Entity.GetComponent<Animator>().SetInteger("State", 0);
                    state = 0;
                    end_state.SetActive(false);
                    
                
            }
    }
    }
}
