using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RandomGeneratorWithWeight;

namespace EnemySpawn
{
    public class AdvancedSpawner : EnemySpawner
    {
        public static AdvancedSpawner Instance { get; private set; }

        [SerializeField] GameObject batSpecial;
        [SerializeField] BosSpawner bossSpawner;
        [SerializeField] GameObject lantern;
        [SerializeField] float bosStepSpawn = 300;
        [SerializeField] SpawnerSettingsPerMinute[] spawnerSettings;

        SpawnerSettingsPerMinute currentSettings;

        public int minutsCounter;

        new void Start()
        {
            minutsCounter = 0;

            if (Instance != null && Instance != this)
                Destroy(gameObject);

            Instance = this;

            base.Start();

            currentSettings = spawnerSettings[0];
            RunLanternSpawners();

            StartCoroutine(Timer());
            StartCoroutine(Spawn());
            StartCoroutine(BossSpawnTimer());
        }

        void RunLanternSpawners()
        {
            for (int i = 0; i < currentSettings.lanternsCount; ++i)
                Invoke("SpawnLantern", Random.Range(minutsCounter * 60f, (minutsCounter + 1) * 60f));
        }

        void SpawnLantern() => Instantiate(lantern, GetSpawnPosition(), Quaternion.identity, parent);

        IEnumerator Timer()
        {
            while (minutsCounter < spawnerSettings.Length)
            {
                if (GameStatus.Instance.status != GameState.Play)
                {
                    yield return null;
                    continue;
                }

                yield return new WaitForSeconds(60);

                ++minutsCounter;
                currentSettings = spawnerSettings[minutsCounter];
                RunLanternSpawners();
                SpawnSpecialEnemy();
            }
        }

        IEnumerator BossSpawnTimer()
        {
            while (minutsCounter < spawnerSettings.Length)
            {
                yield return new WaitForSeconds(bosStepSpawn);

                bossSpawner.SetPlayer(playerTransform);
                bossSpawner.SpawnBoss();
            }
        }

        IEnumerator Spawn()
        {
            while (true)
            {
                if (GameStatus.Instance.status != GameState.Play)
                {
                    yield return null;
                    continue;
                }

                var enemyToSpawn = GetItemWithWeight.GetItem<GameObject>(currentSettings.enemiesPrefabs);

                switch (enemyToSpawn.tag)
                {
                    case "Enemy":
                        SpawnEnemy(enemyToSpawn);
                        break;
                    case "Spawner":
                        Instantiate(enemyToSpawn).GetComponent<EnemySpawner>().SetPlayer(playerTransform);
                        break;
                }

                yield return new WaitForSeconds(Random.Range(currentSettings.minDelay, currentSettings.maxDelay));
            }
        }

        public void MakeFreezeSpawn(float delay)
        {
            StopAllCoroutines();
            StartCoroutine(FreezeSpawn(delay));
        }

        private IEnumerator FreezeSpawn(float delay)
        {
            yield return new WaitForSeconds(delay);

            StartCoroutine(Timer());
            StartCoroutine(Spawn());
            StartCoroutine(BossSpawnTimer());
        }

        private void SpawnSpecialEnemy() => SpawnEnemy(batSpecial);
    }
}