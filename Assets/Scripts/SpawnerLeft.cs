using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnerLeft : MonoBehaviour
{
    public GameObject quad;
    public GameObject trash;
    float screenX, screenY;
    Vector2 pos;

    void Start() {
        InvokeRepeating("Spawn", 2f, Random.Range(3f, 5f));
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
