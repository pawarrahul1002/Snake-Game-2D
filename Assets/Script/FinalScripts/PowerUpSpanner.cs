using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpSpanner : MonoBehaviour
{
    public BoxCollider2D gridArea;
    public List<GameObject> PowerUps = new List<GameObject>();

    public void Start()
    {
        StartCoroutine("timeForSpawnning");
    }

    public void PowerSpawn()
    {
        Bounds bounds = this.gridArea.bounds;
        int x = (int)Random.Range(bounds.min.x, bounds.max.x);
        int y = (int)Random.Range(bounds.min.y, bounds.max.y);


        int num = (int)Random.Range(0, PowerUps.Count);
        Instantiate(PowerUps[1], new Vector2(x, y), Quaternion.identity);

    }

    IEnumerator timeForSpawnning()
    {

        float nextSpwanTime = Random.Range(5f, 10f);

        yield return new WaitForSeconds(0.02f);
        PowerSpawn();
    }
}