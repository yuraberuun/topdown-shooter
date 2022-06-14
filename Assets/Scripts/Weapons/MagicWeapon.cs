using UnityEngine;

public class MagicWeapon : WeaponController
{
    override protected void Shoot()
    {
        var offset = new Vector3(Random.Range(-spawnOffset, spawnOffset), Random.Range(-spawnOffset, spawnOffset), 0);

        bulletDirection = ((EnemyContainer.Instance.GetNearestEnemy().transform.position - transform.position) + offset);

        if (bulletDirection.magnitude > 1)
            bulletDirection.Normalize();

        var bulletInfo = CreateBulletInfo(true);

        GameObject bullet = objectsPool.GetObject();
        
        bullet.transform.position = transform.position + offset;

        bullet.GetComponent<BulletController>().Shoot(bulletInfo);
        bullet.GetComponent<Rigidbody2D>().AddForce(bulletDirection * speed);

        SoundManager.Instance.MakeSound(SoundType.WeaponMagic);
    }
}
