using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PanelController : MonoBehaviour
{
    public GameObject pauseUI, gameOverUI, screenWrappinText;
    public Animator wallAnim;
    public string currentScene, MenuScene;
    public Text WinText;
    [HideInInspector]
    public bool isShield, isScrennWrap;
    bool isPaused;
    public SnakePlayer snake1;
    public SecondPlayer snake2;

    [HideInInspector]
    public float maxX, maxY, minX, minY;
    public BoxCollider2D wallArea;
    void Start()
    {
        isScrennWrap = true;

        snake1 = snake1.GetComponent<SnakePlayer>();
        snake2 = snake2.GetComponent<SecondPlayer>();
        snake1.scoreCount = 0;
        snake2.scoreCount = 0;

        Bounds bounds = this.wallArea.bounds;
        maxX = bounds.max.x;
        maxY = bounds.max.y;
        minX = bounds.min.x;
        minY = bounds.min.y;
        StartCoroutine(DisableSrennWrapping());
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            isPaused = !isPaused;
            PauseButtonClick();
        }

    }

    public Vector3 ScreenWrap(Vector3 newPosition)
    {
        if (isScrennWrap)
        {
            if (newPosition.x > maxX)
            {
                newPosition.x = -newPosition.x + 1f;
            }
            else if (newPosition.x <= minX)
            {
                newPosition.x = -newPosition.x - 1f;
            }

            if (newPosition.y >= maxY)
            {
                newPosition.y = -newPosition.y + 1f;
            }
            else if (newPosition.y <= minY)
            {
                newPosition.y = -newPosition.y - 1f;
            }
            return newPosition;

        }
        return newPosition;


    }
    IEnumerator DisableSrennWrapping()
    {
        float wrapTimimg = Random.Range(10f, 30f);
        yield return new WaitForSeconds(wrapTimimg);

        isScrennWrap = !isScrennWrap;
        if (isScrennWrap)
        {
            wallAnim.SetBool("WallAnim", false);
            screenWrappinText.SetActive(false);
            yield return new WaitForSeconds(0.02f);
        }
        else
        {
            wallAnim.SetBool("WallAnim", true);
            screenWrappinText.SetActive(true);
        }

        StartCoroutine(DisableSrennWrapping());
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

        if (isShield || snake1.faceTouched || snake2.faceTouched)
        {

            if (snake1.scoreCount > snake2.scoreCount)
            {
                WinText.text = "Player 1 Win";
            }
            else if (snake1.scoreCount < snake2.scoreCount)
            {
                WinText.text = "Player 2 Win";
            }
            else
            {
                WinText.text = "Match Tie";
            }
        }
        else if (snake1.p1Win || snake2.p1Win)      //without shield
        {

            WinText.text = "Player 1 Win";

        }
        else if (snake1.p2Win || snake2.p2Win)      //without shield
        {
            WinText.text = "Player 2 Win";

        }

    }
}
