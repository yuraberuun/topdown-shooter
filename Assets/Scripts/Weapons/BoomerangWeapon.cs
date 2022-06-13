using UnityEngine;

public class BoomerangWeapon : WeaponController
{
    [SerializeField] private float _changeDirectionTime = 0.5f;
    [SerializeField] private float _speedUpCoefficient = 4f;

    protected override void Shoot()
    {
        var offset = new Vector3(Random.Range(-spawnOffset, spawnOffset), Random.Range(-spawnOffset, spawnOffset), 0);

        bulletDirection = (EnemyContainer.Instance.GetNearestEnemy().transform.position - transform.position) + offset;

        if (bulletDirection.magnitude > 1)
            bulletDirection.Normalize();

        var bulletInfo = CreateBulletInfo(false, false);
        
        GameObject bullet = objectsPool.GetObject();

        bullet.transform.position = transform.position;

        bullet.GetComponent<BoomerangBulletController>().Prepare(bulletDirection, -speed * _speedUpCoefficient, _changeDirectionTime);
        bullet.GetComponent<BulletController>().Shoot(bulletInfo);
        bullet.GetComponent<Rigidbody2D>().AddForce(bulletDirection * speed);

        SoundManager.Instance.MakeSound(SoundType.WeaponAxeKnifeBoomerang);
    }
}
