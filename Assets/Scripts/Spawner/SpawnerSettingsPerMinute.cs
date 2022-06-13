using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RandomGeneratorWithWeight;

namespace EnemySpawn
{
    [System.Serializable]
    internal class SpawnerSettingsPerMinute
    {
        public float minDelay = 0.1f;
        public float maxDelay = 0.6f;
        public int lanternsCount = 10;
        public List<ItemForRandom<GameObject>> enemiesPrefabs = new List<ItemForRandom<GameObject>>();
    }
}
