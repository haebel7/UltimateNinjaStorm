using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public GameObject titleOverlay;
    public GameObject gameOverOverlay;
    public GameObject winOverlay;
    public NinjaCounter ninjaCounter;

    private bool isGameOver = false;
    private bool gameStarted = false;

    // Start is called before the first frame update
    void Start()
    {
        Time.timeScale = 1;
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

    private void FixedUpdate()
    {
        if (isGameOver)
        {
            Time.timeScale = Mathf.MoveTowards(Time.timeScale, 0, 0.1f);
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

    public void TriggerGameWon()
    {
        winOverlay.transform.Find("NinjaTotal").GetComponent<TMPro.TextMeshProUGUI>().text = "Total Ninjas: " + ninjaCounter.ninjaCount;
        winOverlay.SetActive(true);
        isGameOver = true;
    }

    public void ExitGame()
    {
        Application.Quit();
        print("Game Closed");
    }

    public bool GetIsGameOver()
    {
        return isGameOver;
    }
}
