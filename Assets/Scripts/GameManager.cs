using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public GameObject titleOverlay;
    public GameObject gameOverOverlay;

    private bool isGameOver = false;
    private bool gameStarted = false;

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

        if ((isGameOver || !gameStarted) && Input.GetKeyDown(KeyCode.Escape))
        {
            ExitGame();
        }
    }

    public void StartGame()
    {
        titleOverlay.SetActive(false);
        gameStarted = true;
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

    public void ExitGame()
    {
        Application.Quit();
        print("Game Closed");
    }
}
