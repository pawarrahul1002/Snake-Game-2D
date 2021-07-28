using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class SecondPlayer : MonoBehaviour
{
    private Vector2 dir = Vector2.down;
    private List<Transform> _segments = new List<Transform>();
    public Transform segmentPrefab;
    private bool upInput, DownInput, leftInput, rightInput;
    public int intialSize = 4;

    private float maxX, maxY, minX, minY;
    public BoxCollider2D wallArea;
    private Rigidbody2D rgbd2D;
    private int scoreCount;
    public Text scoreText;
    private float snakeFaceAngle;
    public GameObject pauseUI, gameOverUI;
    public string currentScene, MenuScene;
    private bool isPaused;
    public float speed;
    public Camera cam;
    // public GameObject segmentGameObject;
    public GameObject shield;
    public Text WinText;





    void Start()
    {
        // speed = 2f;
        // rgbd2D = GetComponent<Rigidbody2D>();
        ResetState();
        scoreCount = 0;
        Bounds bounds = this.wallArea.bounds;
        maxX = bounds.max.x;
        maxY = bounds.max.y;
        minX = bounds.min.x;
        minY = bounds.min.y;


        snakeFaceAngle = 0f;

        // InvokeRepeating("Movement", 0.3f, 0.3f);
    }


    void Update()
    {


        upInput = Input.GetKeyDown(KeyCode.W);
        DownInput = Input.GetKeyDown(KeyCode.S);
        leftInput = Input.GetKeyDown(KeyCode.A);
        rightInput = Input.GetKeyDown(KeyCode.D);



        // upInput = Input.GetKeyDown(KeyCode.UpArrow);
        // DownInput = Input.GetKeyDown(KeyCode.DownArrow);
        // leftInput = Input.GetKeyDown(KeyCode.LeftArrow);
        // rightInput = Input.GetKeyDown(KeyCode.RightArrow);


        changePos();


        if (Input.GetKeyDown(KeyCode.Escape))
        {
            isPaused = !isPaused;
            PauseButtonClick();
        }


    }


    private void changePos()
    {
        if (upInput && (dir != Vector2.down))
        {
            dir = Vector2.up;
            snakeFaceAngle = 0f;
        }
        else if (DownInput && (dir != Vector2.up))
        {

            dir = Vector2.down;

            snakeFaceAngle = 180f;
        }
        else if (leftInput && (dir != Vector2.right))
        {
            dir = Vector2.left;

            snakeFaceAngle = 90f;
        }
        else if (rightInput && (dir != Vector2.left))
        {
            dir = Vector2.right;

            snakeFaceAngle = -90f;
        }

        this.transform.eulerAngles = new Vector3(0, 0, snakeFaceAngle);
    }

    private void FixedUpdate()
    {
        Movement();
    }


    void Movement()
    {
        ScreenWrap();


        for (int i = _segments.Count - 1; i > 0; i--)
        {
            _segments[i].position = _segments[i - 1].position;
        }



        this.transform.position = new Vector3(Mathf.Round(this.transform.position.x) + dir.x,
                                               Mathf.Round(this.transform.position.y) + dir.y,
                                               0f);





    }

    void ScreenWrap()
    {
        Vector3 newPosition = transform.position;

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


        transform.position = newPosition;
    }





    public void Grow()
    {
        Transform segment = Instantiate(this.segmentPrefab);
        segment.GetComponent<BoxCollider2D>().enabled = true;

        segment.position = _segments[_segments.Count - 1].position;
        _segments.Add(segment);

    }

    void Reduce()
    {
        Destroy(_segments[_segments.Count - 1].gameObject);
        if (_segments.Count <= intialSize)
        {
            GameOver();
        }
        _segments.RemoveAt(_segments.Count - 1);
        cam.GetComponent<SpawingFood>().gainerCount--;
    }


    private void ResetState()
    {
        scoreCount = 0;
        ScoreChanger();

        for (int i = 1; i < _segments.Count; i++)
        {
            Destroy(_segments[i].gameObject);
        }
        _segments.Clear();

        this.transform.position = new Vector3(-18f, 0f, 0f);
        _segments.Add(this.transform);

        for (int i = 1; i < this.intialSize; i++)
        {
            _segments.Add(Instantiate(this.segmentPrefab, new Vector3(-18f, 0f, 0f), Quaternion.identity));
        }


    }


    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Food")
        {

            Destroy(other.gameObject);
            cam.GetComponent<SpawingFood>().Spawn();
            Grow();
            scoreCount++;
            ScoreChanger();
        }
        else if (other.tag == "Burner")
        {
            scoreCount--;
            if (scoreCount <= 0)
            {
                GameOver();
            }
            else
            {
                Reduce();
                Destroy(other.gameObject);
                cam.GetComponent<SpawingFood>().Spawn();
                ScoreChanger();
            }

        }
        else if (other.tag == "ScoreBoost")
        {
            scoreCount += 10;
            Destroy(other.gameObject);

            cam.GetComponent<PowerUpSpanner>().Start();
            ScoreChanger();
        }
        else if (other.tag == "Obstacle2")
        {
            GameOver();
        }
        else if (other.tag == "Shield")
        {
            Physics2D.IgnoreLayerCollision(8, 8, true);
            shield.gameObject.SetActive(true);

            Destroy(other.gameObject);

            cam.GetComponent<PowerUpSpanner>().Start();
            StartCoroutine("StartLayerCollision");
        }
        else if (other.tag == "Obstacle")
        {
            GameOver();
        }
        // else if (other.tag == "Player")
        // {
        //     // GameOver();
        // }

    }

    void ScoreChanger()
    {
        scoreText.text = "Player_2 : " + scoreCount.ToString();
    }

    IEnumerator StartLayerCollision()
    {
        yield return new WaitForSeconds(20f);


        shield.gameObject.SetActive(false);
        Physics2D.IgnoreLayerCollision(8, 8, false);
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

    void GameOver()
    {
        // WinText.text = "Player 2 Win";
        Time.timeScale = 0;
        gameOverUI.gameObject.SetActive(true);
        WinText.gameObject.SetActive(true);

        WinText.text = "Player 2 Win";
    }

}//class
