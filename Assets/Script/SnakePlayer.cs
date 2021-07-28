using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SnakePlayer : MonoBehaviour
{
    private Vector2 dir = Vector2.right;
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
    public GameObject segmentGameObject;
    public GameObject shield;





    void Start()
    {
        // speed = 2f;
        rgbd2D = GetComponent<Rigidbody2D>();
        ResetState();
        scoreCount = 0;
        snakeFaceAngle = -90f;
        Bounds bounds = this.wallArea.bounds;
        maxX = bounds.max.x;
        maxY = bounds.max.y;
        minX = bounds.min.x;
        minY = bounds.min.y;

        // InvokeRepeating("Movement", 0.3f, 0.3f);
    }

    private void Update()
    {

        upInput = Input.GetKeyDown(KeyCode.UpArrow);
        DownInput = Input.GetKeyDown(KeyCode.DownArrow);
        leftInput = Input.GetKeyDown(KeyCode.LeftArrow);
        rightInput = Input.GetKeyDown(KeyCode.RightArrow);


        changePos();




        if (Input.GetKeyDown(KeyCode.Escape))
        {
            isPaused = !isPaused;
            PauseButtonClick();
        }

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

    // void changePos()
    // {
    //     if (Input.GetKey(KeyCode.RightArrow) && (dir != Vector2.left))
    //     {
    //         dir = Vector2.right * new Vector2(speed, dir.y);
    //     }
    //     else if (Input.GetKey(KeyCode.DownArrow) && (dir != Vector2.up))
    //     {
    //         dir = Vector2.down * new Vector2(dir.x, -speed);
    //     } // '-up' means 'down'
    //     else if (Input.GetKey(KeyCode.LeftArrow) && (dir != Vector2.right))
    //     {
    //         dir = Vector2.left * new Vector2(-speed, dir.y);
    //     }// '-right' means 'left'
    //     else if (Input.GetKey(KeyCode.UpArrow) && (dir != Vector2.down))
    //     {
    //         dir = Vector2.up * new Vector2(dir.x, speed);
    //     }
    // }

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

        // Save current position (gap will be here)
        // v = transform.position;

        // Move head into new direction (now there is a gap)

        // transform.Translate(dir * speed * Time.deltaTime);

        ScreenWrap();


        for (int i = _segments.Count - 1; i > 0; i--)
        {
            _segments[i].position = _segments[i - 1].position;// * speed;// * Time.deltaTime;
        }



        this.transform.position = new Vector3(Mathf.Round(this.transform.position.x) + dir.x,
                                               Mathf.Round(this.transform.position.y) + dir.y,
                                               0f);





    }


    public void Grow()
    {
        Transform segment = Instantiate(this.segmentPrefab);
        segment.GetComponent<BoxCollider2D>().enabled = true;
        // if (!shield)
        // {
        //     segment.gameObject.GetComponent<BoxCollider2D>().enabled = true;
        // }

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
        // noOfSegemnents = _segments.Count;

        cam.GetComponent<SpawingFood>().gainerCount--;
    }

    private void ResetState()
    {

        scoreCount = 0;
        scoreText.text = "Score : " + scoreCount.ToString();

        for (int i = 1; i < _segments.Count; i++)
        {
            Destroy(_segments[i].gameObject);
        }
        _segments.Clear();

        this.transform.position = Vector3.zero;
        _segments.Add(this.transform);

        for (int i = 1; i < this.intialSize; i++)
        {
            // Transform t = Instantiate(this.segmentPrefab, _segments[_segments.Count - 1].position + new Vector3(dir.x, dir.y, 0f), Quaternion.identity);
            // t.position = _segments[_segments.Count - 1].position + new Vector3(dir.x + 0.5f, dir.y + 0.5f, 0f);
            _segments.Add(Instantiate(this.segmentPrefab));

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
            scoreText.text = "Score : " + scoreCount.ToString();
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
                scoreText.text = "Score : " + scoreCount.ToString();
            }

        }
        else if (other.tag == "ScoreBoost")
        {
            scoreCount += 10;
            Destroy(other.gameObject);

            cam.GetComponent<PowerUpSpanner>().Start();
            scoreText.text = "Score : " + scoreCount.ToString();
        }
        // else if (other.tag == "Obstacle")
        else if (other.tag == "Obstacle")
        {
            // ResetState();
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
        // else if (other.tag == "Obsatcle")//== "Player2")
        // {
        //     Debug.Log("hhhhhhhhhhh");
        //     Debug.Log("collison found");
        //     GameOver();
        // }
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
        Time.timeScale = 0;
        gameOverUI.gameObject.SetActive(true);
    }

}//class


// private void changePos()
// {
//     if (upInput && (dir != Vector2.down))
//     {
//         dir = Vector2.up;

//         this.transform.eulerAngles = new Vector3(0f, 0f, 0f);
//     }
//     else if (DownInput && (dir != Vector2.up))
//     {
//         dir = Vector2.down;
//         this.transform.eulerAngles = new Vector3(0f, 0f, 180f);
//     }
//     else if (leftInput && (dir != Vector2.right))
//     {
//         dir = Vector2.left;
//         this.transform.eulerAngles = new Vector3(0, 0, 90f);
//     }
//     else if (rightInput && (dir != Vector2.left))
//     {
//         dir = Vector2.right;
//         this.transform.eulerAngles = new Vector3(0, 0, -90f);
//     }
// }




// private void changePos()
// {
//     // if ((transform.position.x != maxX) || (transform.position.x != minX) ||
//     //     (transform.position.y != maxY) || (transform.position.y != minY))
//     // {
//     if (upInput && (dir != Vector2.down) && ((transform.position.x != maxX) && (transform.position.x != minX)))
//     {
//         dir = Vector2.up;
//         this.transform.eulerAngles = new Vector3(0f, 0f, 0f);
//     }
//     else if (DownInput && (dir != Vector2.up) && ((transform.position.x != maxX) && (transform.position.x != minX)))
//     {
//         dir = Vector2.down;
//         this.transform.eulerAngles = new Vector3(0f, 0f, 180f);
//     }
//     else if (leftInput && (dir != Vector2.right) && ((transform.position.y != maxY) && (transform.position.y != minY)))
//     {
//         dir = Vector2.left;
//         this.transform.eulerAngles = new Vector3(0, 0, 90f);
//     }
//     else if (rightInput && (dir != Vector2.left) && ((transform.position.y != maxY) && (transform.position.y != minY)))
//     {
//         dir = Vector2.right;
//         this.transform.eulerAngles = new Vector3(0, 0, -90f);
//     }
//     // }

// }


// void ScreenWrap()
// {
//     Vector3 newPosition = transform.position;

//     if (newPosition.x > 23 || newPosition.x < -23)
//     {
//         newPosition.x = -newPosition.x;
//     }

//     if (newPosition.y > 11 || newPosition.y < -11)
//     {
//         newPosition.y = -newPosition.y;
//     }

//     transform.position = newPosition;
// }

