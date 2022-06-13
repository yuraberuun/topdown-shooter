using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightWeapon : WeaponController
{
    override protected void Shoot()
    {
        List<GameObject> enemies = EnemyContainer.Instance.GetEnemyOnCamera();

        if (enemies.Count == 0)
            return;

        for (int i = 0; i < bulletCount; i++)
        {
            Transform enemyTransform = enemies[Random.Range(0, enemies.Count)].transform;
            GameObject lighting = objectsPool.GetObject();

            var bulletInfo = CreateBulletInfo();

            lighting.transform.position = enemyTransform.position;
            lighting.GetComponent<BulletController>().Shoot(bulletInfo);
        }
    }

    protected override IEnumerator ShootsWithDelayCorutine()
    {
        SoundManager.Instance.MakeSound(SoundType.WeaponLightning);

        yield return new WaitForSeconds(0.0f);
        Shoot();
    }
}
