using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PanelController : MonoBehaviour
{
    public GameObject pauseUI, gameOverUI;

    public string currentScene, MenuScene;
    public Text scoreText;
    public Text WinText;
    bool isPaused;

    void Update()
    {

    }



    public void PauseButtonClick()
    {
        Time.timeScale = 0f;
        pauseUI.gameObject.SetActive(true);

    }

    public void ResumeButtonClick()
    {
        Time.timeScale = 1f;
        isPaused = false;
        pauseUI.gameObject.SetActive(false);
    }
    public void RestartButtonClick()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(currentScene);
    }
    public void MenuButtonClick()
    {
        SceneManager.LoadScene(MenuScene);
    }

    public void GameOver()
    {
        // WinText.text = "Player 1 Win";
        Time.timeScale = 0;

        gameOverUI.gameObject.SetActive(true);
        WinText.gameObject.SetActive(true);

        WinText.text = "Player 1 Win";
    }
}
