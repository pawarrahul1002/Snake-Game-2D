using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SnakePlayer : MonoBehaviour
{
    public TextMeshProUGUI shieldText;
    private Vector2 dir = Vector2.right;
    private List<Transform> _segments = new List<Transform>();
    public Transform segmentPrefab;
    private bool upInput, DownInput, leftInput, rightInput;
    public int intialSize = 4;
    // private float maxX, maxY, minX, minY;
    // public BoxCollider2D wallArea;
    // private Rigidbody2D rgbd2D;

    public TextMeshProUGUI scoreText;
    private float snakeFaceAngle;
    public Camera cam;
    public GameObject shield, shieldImage;
    public PanelController panelController;

    [HideInInspector]
    public bool p1Win, p2Win, faceTouched;

    [HideInInspector]
    public int scoreCount;
    Vector3 pos;



    void Start()
    {
        ResetState();
        scoreCount = 0;
        snakeFaceAngle = -90f;
        // Bounds bounds = this.wallArea.bounds;
        // maxX = bounds.max.x;
        // maxY = bounds.max.y;
        // minX = bounds.min.x;
        // minY = bounds.min.y;

        panelController = panelController.GetComponent<PanelController>();
    }

    private void Update()
    {

        upInput = Input.GetKeyDown(KeyCode.UpArrow);
        DownInput = Input.GetKeyDown(KeyCode.DownArrow);
        leftInput = Input.GetKeyDown(KeyCode.LeftArrow);
        rightInput = Input.GetKeyDown(KeyCode.RightArrow);


        changePos();
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
        // pos = transform.position;

        transform.position = panelController.ScreenWrap(transform.position);


        for (int i = _segments.Count - 1; i > 0; i--)
        {
            _segments[i].position = _segments[i - 1].position;// * speed;// * Time.deltaTime;
        }

        this.transform.position = new Vector3(Mathf.Round(this.transform.position.x) + dir.x,
                                               Mathf.Round(this.transform.position.y) + dir.y,
                                               0f);

    }

    // Vector3 ScreenWrap(Vector3 newPosition)
    // {
    //     if (newPosition.x > maxX)
    //     {
    //         newPosition.x = -newPosition.x + 1f;
    //     }
    //     else if (newPosition.x <= minX)
    //     {
    //         newPosition.x = -newPosition.x - 1f;
    //     }

    //     if (newPosition.y >= maxY)
    //     {
    //         newPosition.y = -newPosition.y + 1f;
    //     }
    //     else if (newPosition.y <= minY)
    //     {
    //         newPosition.y = -newPosition.y - 1f;
    //     }
    //     return newPosition;
    // }

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
            panelController.GameOver();
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

        this.transform.position = Vector3.zero;
        _segments.Add(this.transform);

        for (int i = 1; i < this.intialSize; i++)
        {
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
            ScoreChanger();
        }
        else if (other.tag == "Burner")
        {
            scoreCount--;
            if (scoreCount <= 0)
            {
                panelController.GameOver();
                scoreText.text = "Player_1 : 00";
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
        else if (other.tag == "Shield")
        {
            panelController.isShield = true;
            Physics2D.IgnoreLayerCollision(8, 11, true);    //player1 and body1
            Physics2D.IgnoreLayerCollision(9, 11, true);    //player2 and body1
            shield.gameObject.SetActive(true);
            shieldImage.gameObject.SetActive(true);
            shieldText.text = "Player 1 Got Shield";

            Destroy(other.gameObject);

            cam.GetComponent<PowerUpSpanner>().Start();
            StartCoroutine("StartLayerCollision");
        }

        else if (other.tag == "Body1")
        {
            p2Win = true;
            panelController.GameOver();
        }
        else if (other.tag == "Body2")
        {
            p1Win = true;
            panelController.GameOver();
        }
        else if (!panelController.isScrennWrap && other.tag == "Wall")
        {
            p2Win = true;
            panelController.GameOver();
        }


    }

    void ScoreChanger()
    {
        scoreText.text = "Player_1: " + scoreCount.ToString();
    }

    IEnumerator StartLayerCollision()
    {
        yield return new WaitForSeconds(20f);



        shield.gameObject.SetActive(false);
        shieldImage.gameObject.SetActive(false);

        panelController.isShield = false;
        Physics2D.IgnoreLayerCollision(8, 11, false);
        Physics2D.IgnoreLayerCollision(9, 11, false);
    }

}//class
