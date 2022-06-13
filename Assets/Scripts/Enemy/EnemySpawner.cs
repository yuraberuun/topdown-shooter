using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] Transform playerTransform;
    [SerializeField] GameObject enemyPrefab;
    [SerializeField] List<GameObject> enemyPrefabs = new List<GameObject>();

    [SerializeField] float cameraOffsetTospawn;
    [SerializeField] float minDelay = 0.1f;
    [SerializeField] float maxDelay = 0.6f;
    float screenHeight, screenWidth;

    [SerializeField] GameObject batEnemy;
    [SerializeField] GameObject bossEnemy;
    [SerializeField] GameObject flowerEnemyPrefab;

    [SerializeField] int batCount = 15;
    [SerializeField] int countEnemiesInSquare = 100;

    void Start()
    {
        screenHeight = Camera.main.pixelHeight;
        screenWidth = Camera.main.pixelWidth;

        //SpawnBoss();
        StartCoroutine(Spawn());
        //StartCoroutine(SpawnEnemySquare());
    }

    void SpawnBoss()
    {
        var enemy = Instantiate(bossEnemy, (Vector2)playerTransform.position + Vector2.right, Quaternion.identity);

        var ai = enemy.GetComponent<EnemyAI>();

        ai.SetTarget(playerTransform);
        ai.Respawn();

        SpawnCircleOfEnemies();
    }

    void SpawnCircleOfEnemies()
    {
        float a = 25;
        float b = 20;

        float angleStep = 0.05f;

        var playerPos = playerTransform.position;

        for (float angle = 0; angle < Mathf.PI * 2; angle += angleStep)
        {
            Vector3 newPos = new Vector2(a * Mathf.Cos(angle), b * Mathf.Sin(angle));

            var enemy = Instantiate(flowerEnemyPrefab, newPos + playerPos, Quaternion.identity);
            enemy.GetComponent<EnemyAI>().SetTarget(playerTransform);
        }
    }

    void SpawnBatColony()
    {
        Vector2 pos = new Vector2();

        switch (Random.Range(0, 2))
        {
            case 0:
                pos = Camera.main.ScreenToWorldPoint(new Vector3(-cameraOffsetTospawn, screenHeight / 2 + Random.Range(-screenHeight / 2, screenHeight / 2)));
                pos -= Vector2.left * 0.5f;
                break;
            case 1:
                pos = Camera.main.ScreenToWorldPoint(new Vector3(screenWidth + cameraOffsetTospawn, screenHeight / 2 + Random.Range(-screenHeight / 2, screenHeight / 2)));
                break;
        }

        for (int i = 0; i < batCount; ++i)
        {
            pos = Random.Range(0, 2) == 1 ? pos + Vector2.down * i * 0.05f : pos + Vector2.right * i * 0.05f;

            var enemy = Instantiate(batEnemy, pos, Quaternion.identity);
            enemy.GetComponent<EnemyAI>().SetTarget(playerTransform);
        }
    }

    IEnumerator Spawn()
    {
        while (true)
        {
            if (GameStatus.Instance.status != GameState.Play)
                yield return new WaitForSeconds(Time.deltaTime);

            if (Random.Range(0, 10) == 0)
                SpawnBatColony();
            else
                SpawnEnemy();

            yield return new WaitForSeconds(Random.Range(minDelay, maxDelay));
        }
    }

    IEnumerator SpawnEnemySquare()
    {
        void spawnMuchEneimies()
        {
            for (int i = 0; i < countEnemiesInSquare; ++i)
                SpawnEnemy();
        }

        for (int i = 0; i < 5; ++i)
        {
            spawnMuchEneimies();

            yield return new WaitForSeconds(0.5f);
        }
    }

    void SpawnEnemy()
    {
        Vector2 pos = new Vector2();

        switch (Random.Range(0, 4))
        {
            case 0:
                pos = Camera.main.ScreenToWorldPoint(new Vector3(-cameraOffsetTospawn, screenHeight / 2 + Random.Range(-screenHeight / 2, screenHeight / 2)));
                break;
            case 1:
                pos = Camera.main.ScreenToWorldPoint(new Vector3(screenWidth / 2 + Random.Range(-screenWidth / 2, screenWidth / 2), screenHeight + cameraOffsetTospawn));
                break;
            case 2:
                pos = Camera.main.ScreenToWorldPoint(new Vector3(screenWidth + cameraOffsetTospawn, screenHeight / 2 + Random.Range(-screenHeight / 2, screenHeight / 2)));
                break;
            case 3:
                pos = Camera.main.ScreenToWorldPoint(new Vector3(screenWidth / 2 + Random.Range(-screenWidth / 2, screenWidth / 2), -cameraOffsetTospawn));
                break;
        }

        GameObject enemyRandomPrefab = enemyPrefabs[Random.Range(0, enemyPrefabs.Count)] ?? null;

        var enemy = Instantiate(enemyRandomPrefab ?? enemyPrefab, pos, Quaternion.identity);
        enemy.GetComponent<EnemyAI>().SetTarget(playerTransform);
    }
}