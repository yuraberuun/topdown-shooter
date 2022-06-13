using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class KnifeWeapon : WeaponController
{
    private Vector2 moveInput;

    void Update() 
    {
        moveInput = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        bulletDirection = ((moveInput != Vector2.zero) ? moveInput : bulletDirection).normalized;
    } 

    private void SetPosAndRot(GameObject gameObject)
    {
        var offset = new Vector3((bulletDirection.y == 0) ? 0 : Random.Range(-spawnOffset, spawnOffset) / 10f, (bulletDirection.x == 0) ? 0 : Random.Range(-spawnOffset, spawnOffset) / 10f, 1);

        var rotation = Quaternion.Euler(0, 0, -(float)Math.Atan2(bulletDirection.x, bulletDirection.y) * Mathf.Rad2Deg - 90f);

        gameObject.transform.SetPositionAndRotation(transform.position + offset, rotation);
    }

    override protected void Shoot()
    {
        var bulletInfo = CreateBulletInfo(true);
        
        GameObject bullet = objectsPool.GetObject();

        SetPosAndRot(bullet);

        bullet.GetComponent<BulletController>().Shoot(bulletInfo);
        bullet.GetComponent<Rigidbody2D>().AddForce(bulletDirection * speed);

        SoundManager.Instance.MakeSound(SoundType.WeaponAxeKnifeBoomerang);
    }
}
