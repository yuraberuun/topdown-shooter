using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FreezeWeapon : WeaponController
{
    private float _freezeTime;

    private float _currentAngle;
    private float _step;

    protected override void Start()
    {
        base.Start();

        _step = 360f / 12f;
        _currentAngle = 0;
    }

    override protected void Shoot()
    {
        var rotation = Quaternion.Euler(0, 0, _currentAngle);

        GameObject freezeShot = objectsPool.GetObject();
        freezeShot.transform.localScale *= attackArea;
        freezeShot.transform.SetPositionAndRotation(transform.position, rotation);
        freezeShot.GetComponent<FreezeBulletController>().Shoot(_freezeTime, lifeTime, objectsPool.AddObject);

        _currentAngle -= _step;
    }

    protected override void ResetCharacteristic()
    {
        base.ResetCharacteristic();
        _freezeTime = currentLevelData.FreezeTime;
    }

    protected override IEnumerator ShootsWithDelayCorutine()
    {
        yield return new WaitForSeconds(0.0f);
        Shoot();
    }
}
