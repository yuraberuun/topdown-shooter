using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EnemySpawn
{
    public class EnemySpawner : MonoBehaviour
    {
        [SerializeField] protected Transform playerTransform;
        [SerializeField] protected Transform parent;

        [SerializeField] protected float cameraOffsetTospawn;

        protected float screenHeight, screenWidth;

        protected void Start()
        {
            screenHeight = Camera.main.pixelHeight;
            screenWidth = Camera.main.pixelWidth;
        }

        public void SetPlayer(Transform player) => playerTransform = player;

        protected void SpawnEnemy(GameObject enemyToSpawn)
        {
            var enemy = Instantiate(enemyToSpawn, GetSpawnPosition(), Quaternion.identity, parent);
            var ai = enemy.GetComponent<EnemyAI>();
            ai.SetTarget(playerTransform);
            ai.SetParentDamageText(parent);
        }

        protected void SpawnEnemy(GameObject enemyToSpawn, bool isSpecial)
        {
            var enemy = Instantiate(enemyToSpawn, GetSpawnPosition(), Quaternion.identity, parent);
            var ai = enemy.GetComponent<EnemyAI>();
            ai.SetTarget(playerTransform);
            ai.SetIsSpecial(isSpecial);
            ai.SetParentDamageText(parent);
        }

        protected Vector2 GetSpawnPosition()
        {
            switch (Random.Range(0, 4))
            {
                case 0:
                    return Camera.main.ScreenToWorldPoint(new Vector3(-cameraOffsetTospawn, screenHeight / 2 + Random.Range(-screenHeight / 2, screenHeight / 2)));
                case 1:
                    return Camera.main.ScreenToWorldPoint(new Vector3(screenWidth / 2 + Random.Range(-screenWidth / 2, screenWidth / 2), screenHeight + cameraOffsetTospawn));
                case 2:
                    return Camera.main.ScreenToWorldPoint(new Vector3(screenWidth + cameraOffsetTospawn, screenHeight / 2 + Random.Range(-screenHeight / 2, screenHeight / 2)));
                case 3:
                    return Camera.main.ScreenToWorldPoint(new Vector3(screenWidth / 2 + Random.Range(-screenWidth / 2, screenWidth / 2), -cameraOffsetTospawn));
                default:
                    return new Vector2();
            }
        }
    }
}