using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class SecondPlayer : MonoBehaviour
{

    public TextMeshProUGUI shieldText;
    private Vector2 dir = Vector2.up;
    private List<Transform> _segments = new List<Transform>();
    public Transform segmentPrefab;
    private bool upInput, DownInput, leftInput, rightInput;
    public int intialSize = 4;
    public TextMeshProUGUI scoreText;
    private float snakeFaceAngle;
    public Camera cam;
    public GameObject shield, shieldImage;
    public PanelController panelController;

    [HideInInspector]
    public bool p1Win, p2Win, faceTouched;

    [HideInInspector]
    public int scoreCount;





    void Start()
    {
        ResetState();
        scoreCount = 0;
        panelController = panelController.GetComponent<PanelController>();


        snakeFaceAngle = 0f;

        // InvokeRepeating("Movement", 0.3f, 0.3f);
    }


    void Update()
    {
        upInput = Input.GetKeyDown(KeyCode.W);
        DownInput = Input.GetKeyDown(KeyCode.S);
        leftInput = Input.GetKeyDown(KeyCode.A);
        rightInput = Input.GetKeyDown(KeyCode.D);

        changePos();
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
        transform.position = panelController.ScreenWrap(transform.position);

        for (int i = _segments.Count - 1; i > 0; i--)
        {
            _segments[i].position = _segments[i - 1].position;
        }

        this.transform.position = new Vector3(Mathf.Round(this.transform.position.x) + dir.x,
                                               Mathf.Round(this.transform.position.y) + dir.y,
                                               0f);
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
            panelController.GameOver();
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

                panelController.GameOver();
                scoreText.text = "Player_2 : 00";
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
            Physics2D.IgnoreLayerCollision(9, 10, true);
            Physics2D.IgnoreLayerCollision(8, 10, true);
            shield.gameObject.SetActive(true);
            shieldImage.gameObject.SetActive(true);

            shieldText.text = "Player 2 Got Shield";

            Destroy(other.gameObject);

            cam.GetComponent<PowerUpSpanner>().Start();
            StartCoroutine("StartLayerCollision");
        }
        else if (other.tag == "Body2")
        {
            p1Win = true;
            panelController.GameOver();
        }
        else if (other.tag == "Body1")
        {
            p2Win = true;
            panelController.GameOver();
        }
        else if (other.tag == "Player1")
        {
            Debug.Log("Player Face touched");
            faceTouched = true;
        }
        else if (!panelController.isScrennWrap && other.tag == "Wall")
        {
            p1Win = true;
            panelController.GameOver();
        }

    }



    void ScoreChanger()
    {
        scoreText.text = "Player_2 : " + scoreCount.ToString();
    }

    IEnumerator StartLayerCollision()
    {
        yield return new WaitForSeconds(20f);


        shield.gameObject.SetActive(false);
        shieldImage.gameObject.SetActive(false);
        panelController.isShield = false;
        Physics2D.IgnoreLayerCollision(9, 10, false);
        Physics2D.IgnoreLayerCollision(8, 10, false);
    }

}//class
