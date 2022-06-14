 using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class StarWeapon : WeaponController
{
    override protected void Shoot()
    {
        bulletDirection = new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f));
        bulletDirection.Normalize();

        var bulletInfo = CreateBulletInfo(false, false);

        GameObject bullet = objectsPool.GetObject();
        
        bullet.transform.position = transform.position;

        bullet.GetComponent<BulletController>().Shoot(bulletInfo);
        bullet.GetComponent<Rigidbody2D>().AddForce(bulletDirection * speed);
        bullet.GetComponent<StarBulletController>().Shoot(bulletInfo.LifeTime, bulletDirection, speed);
    }
}
