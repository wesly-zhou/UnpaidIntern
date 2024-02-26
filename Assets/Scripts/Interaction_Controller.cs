using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interaction_Controller : MonoBehaviour
{
    private Collider interactionAreaCollider;
    // Start is called before the first frame update
    void Start()
    {
        interactionAreaCollider = GetComponentInChildren<Collider>(true);
        if (interactionAreaCollider == null || !interactionAreaCollider.gameObject.CompareTag("Interaction_Area"))
        {
            Debug.LogWarning("Interaction area with 'Interaction_Area' tag not found in children.");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other == interactionAreaCollider && other.gameObject.CompareTag("Player"))
        {
            GetComponentwithTag("Bubble").SetActive(true);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
