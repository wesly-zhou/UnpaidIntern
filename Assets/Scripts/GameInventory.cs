using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class GameInventory : MonoBehaviour {
      public GameObject InventoryMenu;
      public bool InvIsOpen = true;

      //4 Inventory Items:
      public static bool item1bool = true;
      public static bool item2bool = true;
      public static bool item3bool = true;
      public static bool item4bool = true;

      public static int item1num = 0;
      public static int item2num = 0;
      public static int item3num = 0;
      public static int item4num = 0;

      public GameObject item1image;
      public GameObject item2image;
      public GameObject item3image;
      public GameObject item4image;

      public Text item1Text;
      public Text item2Text;
      public Text item3Text;
      public Text item4Text;

      void Start(){
            InventoryMenu.SetActive(true);
            InventoryDisplay();
      }

      void InventoryDisplay(){
            if (item1bool == true) {item1image.SetActive(true);}
            if (item2bool == true) {item2image.SetActive(true);}
            if (item3bool == true) {item3image.SetActive(true);}
            if (item3bool == true) {item4image.SetActive(true);}


            // Item number updates. Comment out if each item is unique (2/2).
            Text item1TextB = item1Text.GetComponent<Text>();
            item1TextB.text = ("" + item1num);

            Text item2TextB = item2Text.GetComponent<Text>();
            item2TextB.text = ("" + item2num);

            Text item3TextB = item3Text.GetComponent<Text>();
            item3TextB.text = ("" + item3num);

            Text item4TextB = item4Text.GetComponent<Text>();
            item4TextB.text = ("" + item4num);
      }

      public void InventoryAdd(string item){
            string foundItemName = item;
            if (foundItemName == "item1") {item1num ++;}
            else if (foundItemName == "item2") {item2num ++;}
            else if (foundItemName == "item3") {item3num ++;}
            else if (foundItemName == "item4") {item4num ++;}
            else { Debug.Log("This item does not exist to be added"); }
            InventoryDisplay();
      }

      public void InventoryRemove(string item, int num){
            string itemRemove = item;
            if (itemRemove == "item1") {
                  item1num -= num;
                  if (item1num <= 0) { item1bool =false; }
                  // Add any other intended effects: new item crafted, speed boost, slow time, etc
             }
            else if (itemRemove == "item2") {
                  item2num -= num;
                  if (item2num <= 0) { item2bool =false; }
                  // Add any other intended effects
             }
            else if (itemRemove == "item3") {
                  item3num -= num;
                  if (item3num <= 0) { item3bool =false; }
                    // Add any other intended effects
            }
            else if (itemRemove == "item4") {
                  item4num -= num;
                  if (item4num <= 0) { item4bool =false; }
                    // Add any other intended effects
            }
            else { Debug.Log("This item does not exist to be removed"); }
            InventoryDisplay();
      }

      // Reset all static inventory values on game restart.
      public void ResetAllInventory(){
            item1num = 0; // object name
            item2num = 0; // object name
            item3num = 0; // object name
            item4num = 0; // object name
      }

}