using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EnemySpawn
{
    public class SquareSpawner : EnemySpawner
    {
        [SerializeField] int countEnemiesInSquare = 100;
        [SerializeField] GameObject enemyPrefab;

        new void Start()
        {
            base.Start();
            StartCoroutine(SpawnEnemySquare());
        }

        IEnumerator SpawnEnemySquare()
        {
            void spawnMuchEnemies()
            {
                for (int i = 0; i < countEnemiesInSquare; ++i)
                {
                    SpawnEnemy(enemyPrefab);
                }
            }

            for (int i = 0; i < 5; ++i)
            {
                spawnMuchEnemies();

                yield return new WaitForSeconds(0.5f);
            }

            Destroy(gameObject);
        }
    }
}