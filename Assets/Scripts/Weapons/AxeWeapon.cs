using Random = UnityEngine.Random;
using UnityEngine.Events;
using UnityEngine;

public class AxeWeapon : WeaponController
{
    private bool rightDirection = true;

    protected override void Shoot()
    {
        bulletDirection = new Vector2((rightDirection) ? Random.Range(0.5f, spawnOffset) : Random.Range(-spawnOffset, -0.5f), 1);

        var bulletInfo = CreateBulletInfo(true);
        
        GameObject bullet = objectsPool.GetObject();

        bullet.transform.position = transform.position;

        bullet.GetComponent<BulletController>().Shoot(bulletInfo);
        bullet.GetComponent<Rigidbody2D>().AddForce(bulletDirection * speed);

        rightDirection = !rightDirection;

        SoundManager.Instance.MakeSound(SoundType.WeaponAxeKnifeBoomerang);
    }
}
