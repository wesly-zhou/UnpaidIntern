using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NPC_Controller : MonoBehaviour
{
    private BoxCollider2D interactionAreaCollider;
    public int itemNum1 = 0, itemNum2 = 0, itemNum3 = 0;
    //itemNum4 = 0, itemNum5 = 0;
    public Text inventoryText1, inventoryText2, inventoryText3;
    //inventoryText4, inventoryText5;
    public bool interactable = false;
    public GameObject request_bubble;
    public GameObject give_bubble;

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
        give_bubble.SetActive(true);
        interactable = true;
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        give_bubble.SetActive(false);
        interactable = false;
    }

    void FixedUpdate()
    {
        int inventoryNum1 = int.Parse(inventoryText1.text);
        int inventoryNum2 = int.Parse(inventoryText2.text);
        int inventoryNum3 = int.Parse(inventoryText3.text);
        // int inventoryNum4 = int.Parse(inventoryText4.text);
        // int inventoryNum5 = int.Parse(inventoryText5.text);
        if(interactable == true && Input.GetKeyDown(KeyCode.F)){
            give_bubble.SetActive(false);
            request_bubble.SetActive(false);
            if (inventoryNum1 >= itemNum1 && inventoryNum2 >= itemNum2 &&
            inventoryNum3 >= itemNum3)
            // && inventoryNum4 >= itemNum4 && inventoryNum5 >= itemNum5)
            {
                inventoryNum1 -= itemNum1;
                inventoryNum2 -= itemNum2;
                inventoryNum3 -= itemNum3;
                // inventoryNum4 -= itemNum4;
                // inventoryNum5 -= itemNum5;
                inventoryText1.text = inventoryNum1.ToString();
                inventoryText2.text = inventoryNum2.ToString();
                inventoryText3.text = inventoryNum3.ToString();
                // inventoryText4.text = inventoryNum4.ToString();
                // inventoryText5.text = inventoryNum5.ToString();
            }
        }
    }

}
