using UnityEngine;

public class BoneWeapon : WeaponController
{
    override protected void Shoot()
    {
        bulletDirection = (EnemyContainer.Instance.GetRandomEnemy().transform.position - transform.position);

        if (bulletDirection.magnitude > 1)
            bulletDirection.Normalize();

        var bulletInfo = CreateBulletInfo(true, false);
        bulletInfo.CollidedAction = BulletCollidedAction;


        var bullet = objectsPool.GetObject();

        bullet.transform.position = transform.position;

        bullet.GetComponent<BoneBulletController>().Prepare(bulletDirection, speed);
        bullet.GetComponent<BulletController>().Shoot(bulletInfo);
        bullet.GetComponent<Rigidbody2D>().AddForce(bulletDirection * speed);

        SoundManager.Instance.MakeSound(SoundType.WeaponBone);
    }

    private void BulletCollidedAction(GameObject obj) => obj.GetComponent<BoneBulletController>().ChangeDirection();
}
