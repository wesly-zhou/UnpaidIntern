using UnityEngine;

public class InfiniteScroll : MonoBehaviour
{
    public float speed = 0.5f; 
    private Vector2 startPosition;
    public float tileSizeX; 

    void Start()
    {
        startPosition = GetComponent<RectTransform>().anchoredPosition;
    }

    void Update()
    {
        float newPosition = Mathf.Repeat(Time.time * speed, tileSizeX);
        GetComponent<RectTransform>().anchoredPosition = startPosition + Vector2.left * newPosition;
    }
}