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
    public GameObject give_bubble;
    public GameObject talk_bubble;
    public GameObject finish_bubble;
    public bool interactable = false;
    public bool Talkabe = false;
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
    public GameObject InventorySystem;
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
        if (other.CompareTag("Player") && state == 0 && transform.parent.gameObject.CompareTag("Entity"))
        {
            use_bubble.SetActive(true);
            interactable = true;
        }

        if (other.CompareTag("Player") && transform.parent.gameObject.CompareTag("NPC"))
        {
            give_bubble.SetActive(true);
            Talkabe = true;
        }

        if (other.CompareTag("Player") && state == 2 && transform.parent.gameObject.CompareTag("Entity"))
        {
            interactable = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player") && transform.parent.gameObject.CompareTag("Entity"))
        {
            use_bubble.SetActive(false);
            interactable = false;
        }

        if (other.CompareTag("Player") && transform.parent.gameObject.CompareTag("NPC"))
        {
            give_bubble.SetActive(false);
            Talkabe = false;
        }
    }

    private void ShowProgressBar()
    {
        ProgressBar.gameObject.SetActive(true);
        isProcessing = true;
        currentTime = 0f;
        ProgressBar.maxValue = totalTime;
        ProgressBar.value = currentTime;
    }

    // Update is called once per frame
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
                interactable = true;
            }
        }
        // Interact with object
        if(interactable == true && state == 0 && isProcessing == false){
            if (Input.GetKeyDown(KeyCode.F)){
                Debug.Log("Get enter key down");
                use_bubble.SetActive(false);
                Entity.GetComponent<Animator>().SetInteger("State", 1);
                state = 1;
                ShowProgressBar();
            }
        }

        // Interact with NPC
        if(Talkabe == true){
            if (Input.GetKeyDown(KeyCode.F)){
                Debug.Log("Get enter key down");
                give_bubble.SetActive(false);
            // TODO: Detect if fit the requirement to give and remove items from inventory
            }
        }

        // Finish interacting with object
        if(interactable == true && state == 2 && isProcessing == false){
            if (Input.GetKeyDown(KeyCode.F)){
                Debug.Log("Get exit key down");
                finish_bubble.SetActive(false);
                itemNum++;
                inventoryText.text = itemNum.ToString();
                use_bubble.SetActive(true);
                // ProgressBar.gameObject.SetActive(false);
                Entity.GetComponent<Animator>().SetInteger("State", 0);
                state = 0;
                // TODO: Add item to the inventory
                
            }
        }


    }
}
