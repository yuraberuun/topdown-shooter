using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class EnemyContainer : SingletonComponent<EnemyContainer>
{
    List<GameObject> enemies = new List<GameObject>();

    private int killedEnemies = 0;

    [SerializeField] Transform playerTransform;

    public GameObject GetRandomEnemy() => enemies[Random.Range(0, enemies.Count)];

    public GameObject GetNearestEnemy() => enemies.OrderBy(x => Vector3.Distance(x.transform.position, playerTransform.position)).ToList()[0];

    public List<GameObject> GetEnemyOnCamera() => enemies.FindAll(x => GeometryUtility.TestPlanesAABB(GeometryUtility.CalculateFrustumPlanes(Camera.main), x.GetComponent<Collider2D>().bounds));

    public void FreezeEnemies(float time)
    {
        foreach (var enemy in enemies)
            enemy.GetComponent<EnemyAI>().FreezeEnemy(time);
    }

    public void AddEnemy(GameObject enemy) => enemies.Add(enemy);

    public void RemoveEnemy(GameObject enemy)
    {
        if (GameStatus.Instance.status == GameState.GameOver)
            return;

        enemies.Remove(enemy);
    }

    public void CountDeadEnemies()
    {
        if (GameStatus.Instance.status == GameState.GameOver)
            return;

        HUDController.Instance.UpdateKilledEnemies(++killedEnemies);
    }
}
