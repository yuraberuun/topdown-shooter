 using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class StarWeapon : WeaponController
{
    override protected void Shoot()
    {
        bulletDirection = new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f)); //створюємо випадковий напрям
        bulletDirection.Normalize(); //за необхідності, нормалізуємо вектор

        var bulletInfo = CreateBulletInfo(false, false); //створюємо змінну, яка містить всю інформацію, яка потрібна пулі

        GameObject bullet = objectsPool.GetObject(); //отримуємо пулю
        
        bullet.transform.position = transform.position; //розміщуємо її на сцені

        bullet.GetComponent<BulletController>().Shoot(bulletInfo); //передаємо для пулі інформацію
        bullet.GetComponent<Rigidbody2D>().AddForce(bulletDirection * speed); //стріляємо

        //передаємо інформацію для додаткового контролера пулі
        bullet.GetComponent<StarBulletController>().Shoot(bulletInfo.LifeTime, bulletDirection, speed);
    }
}
