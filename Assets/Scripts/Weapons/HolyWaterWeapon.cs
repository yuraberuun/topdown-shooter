using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HolyWaterWeapon : WeaponController
{
    [Header("Another")]
    [SerializeField] private Transform _shootPosition;

    override protected void Shoot()
    {
        var enemiesOnScreen = EnemyContainer.Instance.GetEnemyOnCamera();

        if (enemiesOnScreen.Count == 0)
            return;

        GameObject bottle = objectsPool.GetObject();

        bottle.transform.position = _shootPosition.position;

        Transform enemyTransform = enemiesOnScreen[Random.Range(0, enemiesOnScreen.Count)]?.transform;

        var bulletInfo = CreateBulletInfo();

        var bottleController = bottle.GetComponent<HolyWaterController>();
        bottleController.SetBulletInfo(bulletInfo, objectsPool);
        bottleController.SetMovingPositionAndSpeed(enemyTransform.position, speed);
    }
}
