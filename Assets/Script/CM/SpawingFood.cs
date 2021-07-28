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



// private void OnTriggerEnter2D(Collider2D other)
// {
//     if (other.tag == "Player")
//     {
//         Invoke("Spawn", 4f);
//         // Spawn();
//         // gameObject.SetActive(false);
//     }
// }

// Spawn one piece of food
// void Spawn()
// {
//     // x position between left & right border
//     int x = (int)Random.Range(borderLeft.position.x,
//                               borderRight.position.x);

//     // y position between top & bottom border
//     int y = (int)Random.Range(borderBottom.position.y,
//                               borderTop.position.y);

//     // Instantiate the food at (x, y)
//     Instantiate(foodPrefab,
//                 new Vector2(x, y),
//                 Quaternion.identity); // default rotation
// }
