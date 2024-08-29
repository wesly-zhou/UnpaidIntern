using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public GameObject quad;
    public GameObject trash;
    float screenX, screenY;
    Vector2 pos;

    void Start() {
        if (quad.CompareTag("Left")) {
            InvokeRepeating("Spawn", 2f, Random.Range(3f, 5f));
        }
        else if (quad.CompareTag("Right")) {
            InvokeRepeating("Spawn", 0.0f, Random.Range(3f, 5f));
        }
        else if (quad.CompareTag("Top")) {
            InvokeRepeating("Spawn", 3f, Random.Range(3f, 5f));
        }
        else {
            InvokeRepeating("Spawn", 5f, Random.Range(3f, 5f));
        }
    }       

    public void Spawn() 
    {
        MeshCollider c = quad.GetComponent<MeshCollider>();
        screenX = Random.Range(c.bounds.min.x, c.bounds.max.x);
        screenY = Random.Range(c.bounds.min.y, c.bounds.max.y);
        pos = new Vector2(screenX, screenY);
        Instantiate (trash, pos, trash.transform.rotation);
    }
}
