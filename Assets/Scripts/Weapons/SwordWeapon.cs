using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class SwordWeapon : WeaponController
{
    [Header("Another")]
    private bool _useRightPart = true;
    private bool _useTwoParts = false;

    void Update() => _useRightPart = (Input.GetAxisRaw("Horizontal") > 0) ? true : (Input.GetAxisRaw("Horizontal") < 0) ? false : _useRightPart;

    override protected void Shoot()
    {
        var bulletInfo = CreateBulletInfo(false, false);

        void spawnBullet(bool right = false)
        {
            var bullet = objectsPool.GetObject();

            bullet.transform.position = right ? transform.position + new Vector3(spawnOffset, 0, 0) : transform.position + new Vector3(-spawnOffset, 0, 0);
            bullet.transform.rotation = right ? new Quaternion(0, 0, 0, 0) : new Quaternion(0, 0, 180, 0);
            
            bullet.GetComponent<BulletController>().Shoot(bulletInfo);
        };

        if (_useTwoParts)
        {
            spawnBullet(true);
            spawnBullet();
        }

        else
        {
            if (_useRightPart)
                spawnBullet(true);

            else
                spawnBullet();
        }
    }

    override public void Upgrade(int upgradeBy = 0)
    {
        base.Upgrade(upgradeBy);
        
        if (!_useTwoParts & currentLevelData.BulletCount == 2)  
            _useTwoParts = true;
    }

    protected override void BulletDestroyAction(GameObject obj)
    {
        base.BulletDestroyAction(obj);
        gameObject.transform.rotation = new Quaternion(0, 0, 0, 0);

        var scale = gameObject.transform.localScale;

        if (scale.x < 0)
            gameObject.transform.localScale = new Vector3(-scale.x, scale.y, 1);
    }

    protected override IEnumerator ShootsWithDelayCorutine()
    {
        SoundManager.Instance.MakeSound(SoundType.WeaponSword);

        yield return new WaitForSeconds(0.0f);
        Shoot();
    }
}
