using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Snake : MonoBehaviour
{
    private Vector2Int gridpos;
    private Vector2Int gridMoveDir;
    private float gridMoveTimer;
    private float gridMoveTimerMax;

    private void Awake()
    {
        gridpos = new Vector2Int(10, 10);
        gridMoveTimerMax = 1f;
        gridMoveTimer = gridMoveTimerMax;
        gridMoveDir = new Vector2Int(1, 0);
    }

    private void Update()
    {
        HandleInput();
        HandleGridMovement();

    }

    private void HandleInput()
    {
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            if (gridMoveDir.y != -1)
            {
                gridMoveDir.x = 0;
                gridMoveDir.y = 1;
            }
        }

        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            if (gridMoveDir.y != +1)
            {
                gridMoveDir.x = 0;
                gridMoveDir.y = -1;
            }
        }

        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            if (gridMoveDir.x != +1)
            {
                gridMoveDir.x = -1;
                gridMoveDir.y = 0;
            }
        }

        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            if (gridMoveDir.x != -1)
            {
                gridMoveDir.x = 1;
                gridMoveDir.y = 0;

            }
        }


    }

    private void HandleGridMovement()
    {
        gridMoveTimer += Time.deltaTime;

        if (gridMoveTimer >= gridMoveTimerMax)
        {
            gridMoveTimer -= gridMoveTimerMax;
            gridpos += gridMoveDir;


            transform.position = new Vector3(gridpos.x, gridpos.y);
            transform.eulerAngles = new Vector3(0, 0, GetAngleFromVector(gridMoveDir) - 90);
        }

    }

    private float GetAngleFromVector(Vector2Int dir)
    {
        float n = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        if (n < 0)
        {
            n += 360;
        }
        return n;
    }


}
