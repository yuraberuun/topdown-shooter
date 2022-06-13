using UnityEngine.Events;
using UnityEngine;

public class GarlicWeapon : WeaponController
{
    private CircleCollider2D _circleCollider;

    override protected void Start()
    {
        base.Start();
        _circleCollider = bullet.GetComponent<CircleCollider2D>();
    }

    override protected void Shoot()
    {
        UnityAction<GameObject> destroyAction = delegate(GameObject gameObject) {};

        var bulletInfo = CreateBulletInfo(false, false);

        _circleCollider.enabled = true;

        bullet.GetComponent<BulletController>().Shoot(bulletInfo);
    }
    
    override protected void BulletDestroyAction(GameObject obj) => obj.GetComponent<CircleCollider2D>().enabled = false;
}
