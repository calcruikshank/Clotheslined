using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieSpawner : MonoBehaviour
{
    [SerializeField] GameObject[] zombs;
    float betweenTime = 1f;
    GameManager gameManager;
    int percentage;
    bool gameHasStarted = false;
    public Camera cam;
    float width;
    float height;
    ZombieSpawnPoint[] zombieSpawns;
    // Start is called before the first frame update
    void Start()
    {
        cam = Camera.main;
        gameManager = FindObjectOfType<GameManager>();
        height = cam.orthographicSize + 1;
        width = cam.orthographicSize * cam.aspect + 1;
        zombieSpawns = FindObjectsOfType<ZombieSpawnPoint>();
        Debug.Log(zombieSpawns.Length);
    }

    // Update is called once per frame
    void Update()
    {
        if (!gameHasStarted) return;
        betweenTime -= Time.deltaTime;
        if (betweenTime <= 0)
        {
            for (int i = 0; i <= (int)gameManager.numOfZombiesKilled / 10 + 1; i++)
            {

                SpawnRandomZombie();
            }

            betweenTime = 1f;
        }
    }

    void SpawnRandomZombie()
    {
        if (gameManager.numOfZombsOnScreen > gameManager.numOfZombiesKilled / 10 + 10)
        {
            return;
        }
        percentage = Random.Range(0, 100);
        Vector3 randomPosition = zombieSpawns[Random.Range(0, zombieSpawns.Length)].transform.position;
        if (percentage < 1)
        {

            float hugeZombiePercentage = Random.Range(0, 100);
            if (hugeZombiePercentage < 2)
            {
                Instantiate(zombs[3], randomPosition, Quaternion.identity);
                gameManager.numOfZombsOnScreen++;
                return;
            }
            else
            {
                Instantiate(zombs[2], randomPosition, Quaternion.identity);
                gameManager.numOfZombsOnScreen++;
                return;
            }
        }
        if (percentage < 5)
        {
            Instantiate(zombs[1], randomPosition, Quaternion.identity);
            gameManager.numOfZombsOnScreen++;
            return;
        }
        if(percentage <= 100)
        {
            Instantiate(zombs[0], randomPosition, Quaternion.identity);
            gameManager.numOfZombsOnScreen++;
        }
    }

    public void StartSpawningZombs()
    {
        gameHasStarted = true;
    }
}
