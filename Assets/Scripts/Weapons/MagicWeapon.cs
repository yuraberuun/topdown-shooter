using UnityEngine;

public class MagicWeapon : WeaponController
{
    override protected void Shoot()
    {
        //похибка напряму та розміщення пулі
        var offset = new Vector3(Random.Range(-spawnOffset, spawnOffset), Random.Range(-spawnOffset, spawnOffset), 0);

        //напрям пулі
        bulletDirection = ((EnemyContainer.Instance.GetNearestEnemy().transform.position - transform.position) + offset);

        if (bulletDirection.magnitude > 1)  //при необхідності, нормалізуємо вектор
            bulletDirection.Normalize();

        var bulletInfo = CreateBulletInfo(true); //створюємо змінну, яка містить всю інформацію, яка потрібна пулі

        GameObject bullet = objectsPool.GetObject(); //отримуємо пулю
        
        bullet.transform.position = transform.position + offset; //розміщуємо її на сцені

        bullet.GetComponent<BulletController>().Shoot(bulletInfo); //передаємо для пулі інформацію
        bullet.GetComponent<Rigidbody2D>().AddForce(bulletDirection * speed); //стріляємо
    }
}
