// using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NPC_Controller : MonoBehaviour
{
    public static bool isTriggered = false;
    private BoxCollider2D interactionAreaCollider;
    public static int Max_task_num = 3;
    public static int Machine_num = 3;
    // 1 = paper, 2 = soda, 3 = water
    private int itemNum1 = 0, itemNum2 = 0, itemNum3 = 0, itemNum4 = 0, itemNum5 = 0;
    private int inventoryNum1, inventoryNum2, inventoryNum3, inventoryNum4, inventoryNum5;
    private int[] inventoryInfo = new int[Machine_num];
    //itemNum4 = 0, itemNum5 = 0;
    public Text inventoryText1, inventoryText2, inventoryText3, inventoryText4, inventoryText5;
    //inventoryText4, inventoryText5;
    public bool interactable = false;
    public GameObject request_bubble;
    public GameObject give_bubble;
    public GameObject Single_task;
    public GameObject Double_task;
    public GameObject Triple_task;
     // Create a set to store the requirement of each item
    private int[] NPC_Requirement = new int[Machine_num];
    private int[] cur_task;
    void Start()
    {
        
        // Randomly generate the number of tasks
        int cur_task_num = Random.Range(1, Max_task_num + 1);
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
            Debug.Log(cur_task);
            Debug.Log("inventory_sprites_" + cur_task[0].ToString());
            // Single_task.GetComponentInChildren<SpriteRenderer>().sprite = Resources.Load<Sprite>("inventory_sprites_" + cur_task[0].ToString());
            Single_task.GetComponentInChildren<SpriteRenderer>().sprite = Resources.LoadAll<Sprite>("inventory_sprites")[cur_task[0]];



            Single_task.SetActive(true);
        }
        else if (cur_task_num == 2)
        {   
            Debug.Log(cur_task);
            Debug.Log("inventory_sprites_" + cur_task[0].ToString());
            Double_task.transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = Resources.LoadAll<Sprite>("inventory_sprites")[cur_task[0]];
            Double_task.transform.GetChild(1).GetComponent<SpriteRenderer>().sprite = Resources.LoadAll<Sprite>("inventory_sprites")[cur_task[1]];
            Double_task.SetActive(true);
        }
        else if (cur_task_num == 3)
        {
            Debug.Log(cur_task);
            Debug.Log("inventory_sprites_" + cur_task[0].ToString());
            Triple_task.transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = Resources.LoadAll<Sprite>("inventory_sprites")[cur_task[0]];
            Triple_task.transform.GetChild(1).GetComponent<SpriteRenderer>().sprite = Resources.LoadAll<Sprite>("inventory_sprites")[cur_task[1]];
            Triple_task.transform.GetChild(2).GetComponent<SpriteRenderer>().sprite = Resources.LoadAll<Sprite>("inventory_sprites")[cur_task[2]];
            Triple_task.SetActive(true);
        }

        
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && other.transform.position.y > transform.parent.position.y)
        {   
            Debug.Log("Player is above the object");
            transform.parent.gameObject.GetComponent<SpriteRenderer>().sortingOrder = other.GetComponent<SpriteRenderer>().sortingOrder + 5;
        }
        else{
            Debug.Log("Player is below the object");
        }
        if (isTriggered && other.gameObject.CompareTag("Player")) return;
    
        if (other.gameObject.CompareTag("Player"))
        {
            isTriggered = true;
            give_bubble.SetActive(true);
            interactable = true;
        }
        
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        
       
        if (other.gameObject.CompareTag("Player"))
        {
            transform.parent.gameObject.GetComponent<SpriteRenderer>().sortingOrder = other.GetComponent<SpriteRenderer>().sortingOrder - 5;
            isTriggered = false;
            give_bubble.SetActive(false);
            interactable = false;
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
        
        if(interactable == true && Input.GetKeyDown(KeyCode.F)){
            

            // Check if the player has enough items to give
            for(int i = 0; i < cur_task.Length; i++)
            {
                // The player does not have enough items to give
                if (inventoryInfo[cur_task[i]] < NPC_Requirement[cur_task[i]])
                {
                    Debug.Log("You don't have enough items to give");
                    // TODO: Show angry animation and leave the scene
                    give_bubble.SetActive(false);
                    return;
                }
            }

            // If fit the requirement, reduce the number of items in the inventory and upate the inventory text
            Debug.Log("Success! You have given the items to the NPC!");
            Debug.Log(string.Join(", ", inventoryInfo));
            give_bubble.SetActive(false);
            request_bubble.SetActive(false);
            for(int i = 0; i < cur_task.Length; i++)
            {
                inventoryInfo[i] -= NPC_Requirement[i];
            }
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


}
