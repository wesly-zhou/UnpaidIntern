using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ClearLevel : MonoBehaviour
{
    public int numNPC;

    void Start()
    {

    }

    void Update() {
        if (numNPC <= 0) {
            int currentLevel = SceneManager.GetActiveScene().buildIndex;
            SceneManager.LoadScene("NextLevelScene");
        }
    }
}