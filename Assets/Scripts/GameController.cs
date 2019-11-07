using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public GameObject enemy;
    public GameObject player;

    public float spawnTime;
    public float spawnTimer;
    public int spawnRange;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        spawnTimer -= Time.deltaTime;
        if(spawnTimer <= 0)
        {
            Instantiate(
                enemy, 
                new Vector3(
                    player.transform.position.x + Random.Range(-spawnRange, spawnRange),
                    player.transform.position.y, 
                    player.transform.position.y + Random.Range(-spawnRange, spawnRange)
                ),
                enemy.transform.rotation);
            spawnTimer = spawnTime;
        }
    }
}
