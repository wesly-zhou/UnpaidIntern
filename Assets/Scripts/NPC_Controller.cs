// using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class NPC_Controller : MonoBehaviour
{
    private Camera MainCamera;
    // public WinConHandler manager;
    // --------------------------Sound Effect--------------------------
    public AudioClip FinishSound;//sound effect
    public AudioClip AngrySound;//sound effect
    public AudioClip GetScoreSound;//sound effect
    private AudioSource audioSource;
    // ----------------------------------------------------------------

    public int NPC_State; // -2: OutScene, -1: Enter, 0: InScene, 1: Wait, 2: Leave, 3: Angry
    // -----------------------------------------------------------
    public int FaceDirection = 1;
    public float moveSpeed = 1;
    private float randomDistance;
    private Rigidbody2D rb; 
    // private float FixedDisdance = 5f;
    private Vector2 TargetPosition_Fixed;
    private Vector2 TargetPosition_Random;
    // -----------------------------------------------------------
    public static bool isTriggered = false;
    private BoxCollider2D interactionAreaCollider;
    public int Max_task_num = 3;
    public int Machine_num = 4;
    // 1 = paper, 2 = soda, 3 = water
    // private int itemNum1 = 0, itemNum2 = 0, itemNum3 = 0, itemNum4 = 0, itemNum5 = 0;
    private int inventoryNum1, inventoryNum2, inventoryNum3, inventoryNum4, inventoryNum5;
    private int[] inventoryInfo;
    public Text inventoryText1, inventoryText2, inventoryText3, inventoryText4, inventoryText5;
    public bool interactable = false;
    public GameObject request_bubble;
    public GameObject give_bubble;
    public GameObject Single_task;
    public GameObject Double_task;
    public GameObject Triple_task;
     // Create a set to store the requirement of each item
    private int[] NPC_Requirement;
    private int[] cur_task;

    // Waiting Bar
    public Slider ProgressBar;
    public float waitTime = 5f;
    private Image fillImage;
    private float currentTime;

    // 
    public int cur_task_num;
    private void SetupProgressBar()
    {
        // ProgressBar.gameObject.SetActive(true);
        // isProcessing = true;
        fillImage = ProgressBar.fillRect.GetComponent<Image>();
        ProgressBar.maxValue = waitTime;
        // ProgressBar.minValue = 0;
        ProgressBar.value = waitTime;
        currentTime = waitTime;
    }

    void Start()
    {
        NPC_Requirement = new int[Machine_num];
        inventoryInfo = new int[Machine_num];
        // Get the sound component
        audioSource = GetComponent<AudioSource>();
        // Get the score manager
        // scoreManager = transform.parent.GetComponent<ScoreManager>();
        MainCamera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
        // transform.parent.localScale = new Vector3(transform.parent.localScale.x * FaceDirection, transform.parent.localScale.y, 1);
        // give_bubble.transform.localScale = new Vector3(give_bubble.transform.localScale.x * FaceDirection, give_bubble.transform.localScale.y, 1);
        // give_bubble.transform.position = new Vector3(give_bubble.transform.position.x * FaceDirection, give_bubble.transform.position.y, give_bubble.transform.position.z);
        // request_bubble.transform.localScale = new Vector3(request_bubble.transform.localScale.x * FaceDirection, request_bubble.transform.localScale.y, 1);
        // request_bubble.transform.position = new Vector3(request_bubble.transform.position.x * FaceDirection, request_bubble.transform.position.y, give_bubble.transform.position.z);
        // Debug.Log(request_bubble.transform.position);
        // ProgressBar.transform.localScale = new Vector3(give_bubble.transform.localScale.x * FaceDirection, give_bubble.transform.localScale.y, 1);

        // Set the initial state of the NPC, -1 is because the NPC must move for a short distance when it is created
        NPC_State = -2;
        // Generate a random distance for the NPC continue to move
        randomDistance = Random.Range(5f, 10f);
        // Debug.Log("Random distance: " + randomDistance);
        // When the NPC is created, moving forward for a certain distance
        rb = transform.parent.GetComponent<Rigidbody2D>();
        
        TargetPosition_Random = TargetPosition_Fixed + new Vector2(randomDistance * FaceDirection, 0) ;
        // Debug.Log("TargetPosition_Random: " + TargetPosition_Random);

        // Randomly generate the number of tasks
        cur_task_num = Random.Range(1, Max_task_num + 1);
        cur_task = new int[cur_task_num];
        interactionAreaCollider = GetComponent<BoxCollider2D>();
        if (interactionAreaCollider == null || !interactionAreaCollider.gameObject.CompareTag("Detect_Area"))
        {
            Debug.LogWarning("Interaction area with 'Detect_Area' tag not found in children.");
        }

        for(int i = 0; i < cur_task_num; i++)
        {
            // Randomly generate the requirement
            int task_item = Random.Range(0, Machine_num);
            // Count the number of requirement of each item
            NPC_Requirement[task_item] += 1;
            // Store the item info for current task to show
            cur_task[i] = task_item;
        }
        
        // Judge the number of tasks and show the corresponding task
        if (cur_task_num == 1)
        {
            // Debug.Log(cur_task);
            Debug.Log("inventory_sprites_" + cur_task[0].ToString());
            // Single_task.GetComponentInChildren<SpriteRenderer>().sprite = Resources.Load<Sprite>("inventory_sprites_" + cur_task[0].ToString());
            Single_task.GetComponentInChildren<SpriteRenderer>().sprite = Resources.LoadAll<Sprite>("inventory_sprites")[cur_task[0]];



            Single_task.SetActive(true);
        }
        else if (cur_task_num == 2)
        {   
            // Debug.Log(cur_task);
            Debug.Log("inventory_sprites_" + cur_task[0].ToString());
            Double_task.transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = Resources.LoadAll<Sprite>("inventory_sprites")[cur_task[0]];
            Double_task.transform.GetChild(1).GetComponent<SpriteRenderer>().sprite = Resources.LoadAll<Sprite>("inventory_sprites")[cur_task[1]];
            Double_task.SetActive(true);
        }
        else if (cur_task_num == 3)
        {
            // Debug.Log(cur_task);
            Debug.Log("inventory_sprites_" + cur_task[0].ToString());
            Triple_task.transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = Resources.LoadAll<Sprite>("inventory_sprites")[cur_task[0]];
            Triple_task.transform.GetChild(1).GetComponent<SpriteRenderer>().sprite = Resources.LoadAll<Sprite>("inventory_sprites")[cur_task[1]];
            Triple_task.transform.GetChild(2).GetComponent<SpriteRenderer>().sprite = Resources.LoadAll<Sprite>("inventory_sprites")[cur_task[2]];
            Triple_task.SetActive(true);
        }

        SetupProgressBar();
        Debug.Log(string.Join(", ", inventoryInfo));
        

        
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && other.transform.position.y > transform.parent.position.y)
        {   
            // Debug.Log("Player is above the object");
            transform.parent.gameObject.GetComponent<SpriteRenderer>().sortingOrder = other.transform.parent.GetComponent<SpriteRenderer>().sortingOrder + 5;
        }
        else{
            // Debug.Log("Player is below the object");
        }
        if (isTriggered && other.gameObject.CompareTag("Player")) return;
    
        if (other.gameObject.CompareTag("Player") && NPC_State == 1)
        {
            isTriggered = true;
            give_bubble.SetActive(true);
            interactable = true;
        }

        if (other.gameObject.CompareTag("Destroy") && NPC_State == 2)
        {
            Destroy(transform.parent.gameObject);
        }

        if (NPC_State == 0)
        {
           // Meet collider and stop
           Debug.Log("Meet collider and stop! The collider name is: " + other.gameObject.name);
           NPC_State = 1;
           transform.parent.GetComponent<Animator>().SetInteger("State", 1);
        }
        
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("Player") && other.transform.position.y > transform.parent.position.y)
        {   
            // Debug.Log("Player is above the object");
            transform.parent.gameObject.GetComponent<SpriteRenderer>().sortingOrder = other.transform.parent.GetComponent<SpriteRenderer>().sortingOrder + 5;
        }
        else{
            // Debug.Log("Player is below the object");
        }
        if (isTriggered && other.gameObject.CompareTag("Player")) return;
    
        if (other.gameObject.CompareTag("Player") && NPC_State == 1)
        {
            isTriggered = true;
            give_bubble.SetActive(true);
            interactable = true;
        }

        if (other.gameObject.CompareTag("Destroy") && NPC_State == 2)
        {
            Destroy(transform.parent.gameObject);
        }

        if (NPC_State == 0 && other.gameObject.CompareTag("PlayerView"))
        {
           // Meet collider and stop
           Debug.Log("Meet collider and stop! The collider name is: " + other.gameObject.name);
           NPC_State = 1;
           transform.parent.GetComponent<Animator>().SetInteger("State", 1);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        
       
        if (other.gameObject.CompareTag("Player"))
        {
            transform.parent.gameObject.GetComponent<SpriteRenderer>().sortingOrder = other.transform.parent.GetComponent<SpriteRenderer>().sortingOrder - 5;
            isTriggered = false;
            give_bubble.SetActive(false);
            interactable = false;
        }

        if (other.gameObject.CompareTag("Boundary"))
        {
            NPC_State = -1;
            TargetPosition_Fixed = rb.position + new Vector2(1f * FaceDirection, 0) ;
        }
        
    }

    void Update()
    {
        switch(Machine_num){
            case 3:
                inventoryNum1 = int.Parse(inventoryText1.text);
                inventoryNum2 = int.Parse(inventoryText2.text);
                inventoryNum3 = int.Parse(inventoryText3.text);
                inventoryInfo = new int[] { inventoryNum1, inventoryNum2, inventoryNum3 };
                break; 
            case 4: 
                inventoryNum1 = int.Parse(inventoryText1.text);
                inventoryNum2 = int.Parse(inventoryText2.text);
                inventoryNum3 = int.Parse(inventoryText3.text);
                inventoryNum4 = int.Parse(inventoryText4.text);
                inventoryInfo = new int[] { inventoryNum1, inventoryNum2, inventoryNum3, inventoryNum4};
                break;
            case 5:
                inventoryNum1 = int.Parse(inventoryText1.text);
                inventoryNum2 = int.Parse(inventoryText2.text);
                inventoryNum3 = int.Parse(inventoryText3.text);
                inventoryNum4 = int.Parse(inventoryText4.text);
                inventoryNum5 = int.Parse(inventoryText5.text);
                inventoryInfo = new int[] { inventoryNum1, inventoryNum2, inventoryNum3, inventoryNum4, inventoryNum5};
                break;
        }

        if (NPC_State == 1){
            transform.parent.GetComponent<BoxCollider2D>().enabled = true;
            ProgressBar.gameObject.SetActive(true);
            request_bubble.SetActive(true);
            if (currentTime > 0)
            {
                currentTime -= Time.deltaTime;
                ProgressBar.value = currentTime;
                float progress = ProgressBar.value;
                fillImage.color = Color.Lerp(Color.red, new Color32(66, 204, 30, 255), currentTime / waitTime);
                

                if (NPC_State == 2){
                    // Interaction is interrupted
                    transform.parent.GetComponent<BoxCollider2D>().enabled = false;
                    return;
                }
            }
            else
            {
                NPC_State = 3;
                // Play angry sound
                audioSource.clip = AngrySound;
                audioSource.Play();
                ProgressBar.gameObject.SetActive(false);
                request_bubble.SetActive(false);
                // TODO: Show angry animation
                transform.parent.GetComponent<Animator>().SetInteger("State", 3);
                Debug.Log("Did not get anything and get angry!");
                // StartCoroutine(WaitAndPrint());
                CameraFocus();
                Invoke("EndGame", 3f);
                
                // Debug.Log("already done?");
                // transform.parent.rotation = Quaternion.Euler(0, 180, 0);
                
                
            }
        }
        
        if(interactable == true && Input.GetKeyDown(KeyCode.F)){
            

            // Check if the player has enough items to give
            for(int i = 0; i < cur_task.Length; i++)
            {
                // The player does not have enough items to give
                if (inventoryInfo[cur_task[i]] < NPC_Requirement[cur_task[i]])
                {
                    Debug.Log("You don't have enough items to give");
                    // Show angry animation and leave the scene
                    NPC_State = 3;
                    // Play angry sound
                    audioSource.clip = AngrySound;
                    audioSource.Play();
                    transform.parent.GetComponent<Animator>().SetInteger("State", 3);
                    give_bubble.SetActive(false);
                    request_bubble.SetActive(false);
                    ProgressBar.gameObject.SetActive(false);
                    // StartCoroutine(WaitAndPrint());
                    CameraFocus();
                    Invoke("EndGame", 3f);
                    // transform.parent.rotation = Quaternion.Euler(0, 180, 0);
                    return;
                }
            }

            // If fit the requirement, reduce the number of items in the inventory and upate the inventory text
            Debug.Log("Success! You have given the items to the NPC!");
            Debug.Log(string.Join(", ", inventoryInfo));
            give_bubble.SetActive(false);
            request_bubble.SetActive(false);
            // manager.numNPC -= 1;
            for(int i = 0; i < cur_task.Length; i++)
            {
                inventoryInfo[i] -= NPC_Requirement[i];
            }
            NPC_State = 2;
            // Play finish sound
            audioSource.clip = FinishSound;
            audioSource.Play();
            transform.parent.GetComponent<Animator>().SetInteger("State", 2);
            transform.parent.GetComponent<BoxCollider2D>().enabled = false;
            ProgressBar.gameObject.SetActive(false);
            transform.parent.rotation = Quaternion.Euler(0, 180, 0);
            Debug.Log("After giving the items to the NPC, the inventory is: ");
            Debug.Log(string.Join(", ", inventoryInfo));
            switch(Machine_num){
                case 3:
                    inventoryText1.text = inventoryInfo[0].ToString();
                    inventoryText2.text = inventoryInfo[1].ToString();
                    inventoryText3.text = inventoryInfo[2].ToString();
                    break;
                case 4: 
                    inventoryText1.text = inventoryInfo[0].ToString();
                    inventoryText2.text = inventoryInfo[1].ToString();
                    inventoryText3.text = inventoryInfo[2].ToString();
                    inventoryText4.text = inventoryInfo[3].ToString();
                    break;
                case 5:
                    inventoryText1.text = inventoryInfo[0].ToString();
                    inventoryText2.text = inventoryInfo[1].ToString();
                    inventoryText3.text = inventoryInfo[2].ToString();
                    inventoryText4.text = inventoryInfo[3].ToString();
                    inventoryText5.text = inventoryInfo[4].ToString();
                    break;
            }
        }
    }
    void DelayLeave(){
        transform.parent.GetComponent<BoxCollider2D>().enabled = false;
        NPC_State = 2;
        transform.parent.GetComponent<Animator>().SetInteger("State", 2);
        transform.parent.rotation = Quaternion.Euler(0, 180, 0);
    }

    void EndGame() {
        
        SceneManager.LoadScene("LoseScene");
    }

    void FixedUpdate()
    {
        // Setup: Move the fixed distance and enter the scene
        if (NPC_State == -2){
            // ---------------------------------------------------Teporary solution-------------------------------------------------
            rb.position = Vector2.MoveTowards(rb.position, new Vector2(100f * FaceDirection, 0) , moveSpeed * Time.deltaTime);
            // if (Vector2.Distance(rb.position, TargetPosition_Fixed) < 0.01f){
            //     NPC_State = 0;
            // }
        }

        if (NPC_State == -1){
            // ---------------------------------------------------Teporary solution-------------------------------------------------
            rb.position = Vector2.MoveTowards(rb.position, TargetPosition_Fixed , moveSpeed * Time.deltaTime);
            if (Vector2.Distance(rb.position, TargetPosition_Fixed) < 0.01f){
                NPC_State = 0;
            }
        }

        if (NPC_State == 0){
            
            rb.position = Vector2.MoveTowards(rb.position, new Vector2(100f * FaceDirection, 0), moveSpeed * Time.deltaTime);
            // if (Vector2.Distance(rb.position, TargetPosition_Random) < 0.01f){
            //     NPC_State = 1;
            //     transform.parent.GetComponent<Animator>().SetInteger("State", 1);
            // }
        }
        
        if (NPC_State == 2){
            // transform.parent.rotation = Quaternion.Euler(0, 180, 0);
            // FaceDirection = -FaceDirection;
            rb.position = Vector2.MoveTowards(rb.position, rb.position + new Vector2(100f * -FaceDirection, 0), moveSpeed * Time.deltaTime);
        }
    }

    private void CameraFocus(){
        
        // Vector3 newPosition = Vector3.Lerp(MainCamera.transform.position, transform.position, 10f * Time.deltaTime);
        // newPosition.z = MainCamera.transform.position.z; 
        // MainCamera.transform.position = newPosition;
        
        MainCamera.GetComponent<CameraMoveBounded>().target = transform;
        if (MainCamera.orthographic)
            {
                Debug.Log("Camera Focus");
                // MainCamera.orthographicSize = Mathf.Lerp(MainCamera.orthographicSize, 2f, 1f * Time.deltaTime);
                MainCamera.orthographicSize = 2f;
            }
    }

}
