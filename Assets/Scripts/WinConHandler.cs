using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class WinConHandler : MonoBehaviour
{
    public int numNPC;
    public int currentLevel;

    void Start()
    {
        currentLevel = SceneManager.GetActiveScene().buildIndex;
    }

    void Update() {
        if (numNPC <= 0 && currentLevel == 9) {
            SceneManager.LoadScene("WinScene");
        }
        else if (numNPC <= 0) {
            SceneManager.LoadScene("NextLevelScene");
        }
    }
}