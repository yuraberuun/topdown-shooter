using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EnemySpawn
{
    internal class BatColonySpawner : EnemySpawner
    {
        [SerializeField] int batCount = 15;
        [SerializeField] GameObject batEnemy;

        new void Start()
        {
            base.Start();
            SpawnBatColony();
            Destroy(gameObject);
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
    }
}