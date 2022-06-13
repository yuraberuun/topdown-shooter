using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

public class WeaponController : MonoBehaviour
{
    [SerializeField] protected GameObject bullet;

    [Header("Levels data")]
    [SerializeField] protected int level = 1;
    [SerializeField] protected WeaponLevelsData weaponLevelsAsset;
    [SerializeField] protected WeaponBuffsData weaponBuffsData;

    [Header("Pull objects")]
    [SerializeField] protected bool pull = true;
    [SerializeField] protected GameObject parent;

    protected WeaponLevelData currentLevelData;

    protected int minDamage = 0;
    protected int maxDamage = 0;
    protected float hitForce = 0;
    protected float cooldown = 10;
    protected float speed = 500;
    protected float lifeTime = 10;
    protected float attackArea = 1.0f;
    protected float spawnOffset = 0.0f;
    protected float delayBetweenAttacks = 0.0f;
    protected int bulletCount = 1;
    protected int passesThroughtEnemy = 0;
    protected Vector2 bulletDirection = new Vector2(1, 0);

    protected ObjectsPool objectsPool;

    protected int shootTimes;

    virtual protected void Start()
    {
        if (pull)
            objectsPool = new ObjectsPool(5, bullet, parent);
    }

    virtual protected void OnEnable()
    {       
        Upgrade();
        RestartShooting(cooldown);
    }

    //Shoot

    virtual protected void RestartShooting(float startDelay)
    {
        CancelInvoke();
        InvokeRepeating("ShootsWithDelay", startDelay, cooldown);
    }

    virtual protected void ShootsWithDelay() => StartCoroutine(ShootsWithDelayCorutine());
    
    virtual protected IEnumerator ShootsWithDelayCorutine()
    {
        shootTimes = bulletCount;

        for (int i = 0; i < shootTimes; i++)
        {
            yield return new WaitForSeconds(delayBetweenAttacks);
            Shoot();
        }
    }

    virtual protected void Shoot(){}

    //BulletInfo

    virtual protected int GetRamdomDamage() => Random.Range(minDamage, maxDamage);

    virtual protected BulletInfo CreateBulletInfo(bool destroyWhenCollided = false, bool resizeCollider = true)
    {
        return new BulletInfo(BulletDestroyAction, GetRamdomDamage(), hitForce, lifeTime, attackArea, speed, delayBetweenAttacks,
        passesThroughtEnemy, destroyWhenCollided, resizeCollider);
    }

    virtual protected void BulletDestroyAction(GameObject obj) => objectsPool.AddObject(obj);

    //Upgrade

    virtual public void Upgrade(int upgradeBy = 0)
    {
        if (level > 8 && upgradeBy != 0)
            return;

        level += upgradeBy;

        ResetCharacteristic();
        ResetBuffs();
        RestartShooting(0);
    }

    virtual protected void ResetCharacteristic()
    {
        currentLevelData = weaponLevelsAsset.GetLevel(level);

        currentLevelData.GetDefaultCharacteristic(ref minDamage, ref maxDamage, ref hitForce, ref cooldown, ref speed, ref lifeTime,
         ref attackArea, ref bulletCount, ref spawnOffset, ref delayBetweenAttacks, ref passesThroughtEnemy);
    }

    virtual protected void ResetBuffs()
    {
        minDamage *= (int)weaponBuffsData.GetDamageBuff();
        maxDamage *= (int)weaponBuffsData.GetDamageBuff();
        cooldown *= weaponBuffsData.GetCooldownBuff();
        lifeTime *= weaponBuffsData.GetLifeTimeBuff();
        attackArea *= weaponBuffsData.GetAttackAreaBuff();
        speed *= weaponBuffsData.GetSpeedBuff();
        bulletCount += weaponBuffsData.GetBulletCountBuff();
    }
}
