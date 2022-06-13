using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireballWeapon : WeaponController
{
    override protected void Shoot()
    {
        Transform enemyTransform = EnemyContainer.Instance.GetNearestEnemy().transform;

        Vector3 lookAt = enemyTransform.position;

        var bulletDirection = (lookAt - transform.position).normalized;

        float AngleRad = Mathf.Atan2(lookAt.y - transform.position.y, lookAt.x - transform.position.x);

        float AngleDeg = (180 / Mathf.PI) * AngleRad;

        var bulletInfo = CreateBulletInfo(true, false);

        bool isPair = bulletCount % 2 == 0;
        int count = isPair ? bulletCount + 1 : bulletCount;

        int middle = count / 2;

        int offset = count - middle;
        float step = 5f;
        float startStep = step;
        for (int i = 0, j = 1; i < count; i++, j++)
        {
            GameObject fireball = objectsPool.GetObject();

            if (isPair)
            {
                if (i == middle - 1 || i == middle + 1)
                    step = step / bulletCount;
                else
                    step = startStep;
            }

            fireball.transform.rotation = Quaternion.Euler(0, 0, AngleDeg + (step * (offset - j)));

            if (i == middle)
            {
                if(isPair)
                {
                    j = 0;
                    step = -step;
                    Destroy(fireball);
                    continue;
                }
                
                fireball.transform.rotation = Quaternion.Euler(0, 0, AngleDeg);
                j = 0;
                step = -step;
            }

            fireball.transform.position = transform.position;
            fireball.GetComponent<BulletController>().Shoot(bulletInfo);
            fireball.GetComponent<Rigidbody2D>().AddForce(fireball.transform.up * speed);
        }
    }
    
    protected override IEnumerator ShootsWithDelayCorutine()
    {
        SoundManager.Instance.MakeSound(SoundType.WeaponMagic);

        yield return new WaitForSeconds(0.0f);
        Shoot();
    }
}
