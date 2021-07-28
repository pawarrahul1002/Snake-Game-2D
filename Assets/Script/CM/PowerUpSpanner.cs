using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpSpanner : MonoBehaviour
{
    public BoxCollider2D gridArea;
    public List<GameObject> PowerUps = new List<GameObject>();

    public void Start()
    {
        // Invoke("PowerSpawn", 2f);
        StartCoroutine("timeForSpawnning");
    }

    public void PowerSpawn()
    {
        Bounds bounds = this.gridArea.bounds;
        int x = (int)Random.Range(bounds.min.x, bounds.max.x);
        int y = (int)Random.Range(bounds.min.y, bounds.max.y);


        int num = (int)Random.Range(0, PowerUps.Count);
        Instantiate(PowerUps[num], new Vector2(x, y), Quaternion.identity);

    }

    IEnumerator timeForSpawnning()
    {

        float nextSpwanTime = Random.Range(5f, 20f);

        yield return new WaitForSeconds(nextSpwanTime);
        PowerSpawn();
    }
}