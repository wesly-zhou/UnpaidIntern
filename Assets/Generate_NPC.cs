using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Generate_NPC : MonoBehaviour
{
    public GameObject prefabToSpawn; 
    public BoxCollider2D spawnArea; 
    private int direction = 0;

    void Start()
    {
        
        
        // SpawnPrefab();
        InvokeRepeating("SpawnPrefab", 0, 10f);
    }

    void SpawnPrefab()
    {
        // Get the bounds of the spawn area
        Bounds bounds = spawnArea.bounds;
        Quaternion spawnRotation;
        // Randomly pick a point within the bounds
        float x = transform.position.x;
        float y = Random.Range(bounds.min.y, bounds.max.y);
        if (transform.gameObject.name == "NPC_Generate_Area_Right"){
            spawnRotation = Quaternion.Euler(0, 180, 0);
            Debug.Log("This is right");
            direction = -1;
        }
        else{
            spawnRotation = Quaternion.Euler(0, 0, 0);
            direction = 1;
        }
        // Debug.Log("After"+prefabToSpawn.GetComponentInChildren<NPC_Controller>().FaceDirection);
        GameObject instance = Instantiate(prefabToSpawn, new Vector3(x, y, 0), Quaternion.identity);

        spawnArea = GetComponent<BoxCollider2D>();
        NPC_Controller scriptInstance = instance.GetComponentInChildren<NPC_Controller>();
        if (direction == -1){
            Debug.Log("Right");
            scriptInstance.FaceDirection = -1;
            // Debug.Log("Before"+prefabToSpawn.GetComponentInChildren<NPC_Controller>().FaceDirection);
            
        }
        else{
           scriptInstance.FaceDirection = 1;
            // direction = 1;
        }
        
    }
}
