using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelGrid
{
    private Vector2Int foodGridPos;
    private int height;
    private int width;
    public LevelGrid(int width, int height)
    {
        this.width = width;
        this.height = height;
        SpawnFood();
    }

    private void SpawnFood()
    {
        foodGridPos = new Vector2Int(Random.Range(0, width), Random.Range(0, height));

        GameObject foodGameObject = new GameObject("Food", typeof(SpriteRenderer));
        foodGameObject.GetComponent<SpriteRenderer>().sprite = GameAssets.instance.foodSprite;
        foodGameObject.transform.position = new Vector3(foodGridPos.x, foodGridPos.y);
    }

}
