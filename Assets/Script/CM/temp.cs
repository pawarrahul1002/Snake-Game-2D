using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class temp : MonoBehaviour
{
    public Transform currrentPos;
    private bool upInput, DownInput, leftInput, rightInput;

    Vector2 dir = Vector3.down;
    bool leftright;


    public float speed = 0.01f;

    void Start()
    {

        currrentPos = GetComponent<Transform>();
    }

    void Update()
    {

        upInput = Input.GetKeyDown(KeyCode.UpArrow);
        DownInput = Input.GetKeyDown(KeyCode.DownArrow);
        leftInput = Input.GetKeyDown(KeyCode.LeftArrow);
        rightInput = Input.GetKeyDown(KeyCode.RightArrow);



        // changePos();
    }

    void FixedUpdate()
    {
        // Movement();
        changePos();
    }

    private void changePos()
    {

        Vector2 pos = this.transform.position;

        if (upInput && (dir != Vector2.up))
        {
            dir = Vector2.up;

            pos.y = Mathf.Round(pos.y) * speed * Time.deltaTime + dir.y;
            this.transform.eulerAngles = new Vector3(0f, 0f, 0f);
        }
        else if (DownInput && (dir != Vector2.up))
        {
            dir = Vector2.down;

            pos.y = Mathf.Round(pos.y) * speed * Time.deltaTime + dir.y;
            this.transform.eulerAngles = new Vector3(0f, 0f, 180f);
        }
        else if (leftInput && (dir != Vector2.right))
        {

            dir = Vector2.left;

            pos.x = Mathf.Round(pos.x) * speed * Time.deltaTime + dir.x;
            this.transform.eulerAngles = new Vector3(0, 0, 90f);
        }
        else if (rightInput && (dir != Vector2.left))
        {
            dir = Vector2.right;

            pos.x = Mathf.Round(pos.x) * speed * Time.deltaTime + dir.x;
            this.transform.eulerAngles = new Vector3(0, 0, -90f);
        }
        transform.position = pos;
    }


    void Movement()
    {
        Vector2 pos = this.transform.position;
        pos.x = Mathf.Round(pos.x) * speed + dir.x;
        pos.y = Mathf.Round(pos.y) * speed + dir.y;

        // this.transform.position = new Vector3(Mathf.Round(this.transform.position.x) + dir.x,
        //                             Mathf.Round(this.transform.position.y) + dir.y,
        //                             0f);
        transform.position = pos;
    }
}
