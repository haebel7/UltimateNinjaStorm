using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public GameObject titleOverlay;
    public GameObject gameOverOverlay;

    private bool isGameOver = false;

    // Start is called before the first frame update
    void Start()
    {
        titleOverlay.SetActive(true);
    }

    void Update()
    {
        if (isGameOver && Input.GetKeyDown(KeyCode.Space))
        {
            RestartGame();
        }
    }

    public void StartGame()
    {
        titleOverlay.SetActive(false);
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void TriggerGameOver()
    {
        gameOverOverlay.SetActive(true);
        isGameOver = true;
    }
}
