using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EnemySpawn
{
    public class BosSpawner : EnemySpawner
    {
        [SerializeField] List<GameObject> bossEnemyList;

        [SerializeField] GameObject flowerEnemyPrefab;

        [SerializeField] float firstCircleAliveTime = 30;
        [SerializeField] float otherCirclesAliveTime = 60;

        bool isFirstSpawn = true;

        private int currentBoss = 0;

        public void SpawnBoss()
        {
            if (currentBoss == bossEnemyList.Count)
                return;

            var enemy = Instantiate(bossEnemyList[currentBoss++], (Vector2)playerTransform.position + Vector2.right, Quaternion.identity);

            var ai = enemy.GetComponent<EnemyAI>();

            ai.SetTarget(playerTransform);
            ai.Respawn();

            SpawnCircleOfEnemies();

            isFirstSpawn = false;
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

                var enemy = Instantiate(flowerEnemyPrefab, newPos + playerPos, Quaternion.identity).GetComponent<EnemyAI>();

                enemy.SetTarget(playerTransform);
                enemy.SetAliveTime(isFirstSpawn ? firstCircleAliveTime : otherCirclesAliveTime);
            }
        }
    }
}
