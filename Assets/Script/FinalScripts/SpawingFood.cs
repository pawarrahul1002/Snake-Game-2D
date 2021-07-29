using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawingFood : MonoBehaviour
{
    public BoxCollider2D gridArea;

    [HideInInspector]
    public int gainerCount;
    public List<GameObject> foods = new List<GameObject>();

    void Start()
    {
        gainerCount = 0;
        Invoke("Spawn", 4f);
    }

    public void Spawn()
    {
        Bounds bounds = this.gridArea.bounds;
        int x = (int)Random.Range(bounds.min.x, bounds.max.x);
        int y = (int)Random.Range(bounds.min.y, bounds.max.y);

        if (gainerCount < 4)
        {
            Instantiate(foods[0], new Vector2(x, y), Quaternion.identity);
            gainerCount++;
        }
        else
        {
            int num = (int)Random.Range(0, foods.Count);
            Instantiate(foods[num], new Vector2(x, y), Quaternion.identity);
        }
    }
}