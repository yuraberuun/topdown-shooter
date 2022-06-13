using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class WeaponLevelData
{
    [SerializeField] private int _minDamage = 0;
    [SerializeField] private int _maxDamage = 0;
    [SerializeField] private float _hitForce = 0.0f;
    [SerializeField] private float _cooldown = 0.0f;
    [SerializeField] private float _speed = 0.0f;
    [SerializeField] private float _lifeTime = 0.0f;
    [SerializeField] private float _attackArea = 1.0f;
    [SerializeField] private int _bulletCount = 0;
    [SerializeField] private float _spawnOffset = 0.0f;
    [SerializeField] private int _passesThroughtEnemy = 0;
    [SerializeField] private float _delayBetweenShots = 0.0f;
    [SerializeField] private float _freezeTime = 0.0f;

    public void GetDefaultCharacteristic(ref int minDamage, ref int maxDamage, ref float hitForce, ref float cooldown, ref float speed,
     ref float lifeTime, ref float attackArea, ref int bulletCount, ref float spawnOffset, ref float delayBetweenShots, ref int passesThroughtEnemy)
    {
        minDamage = _minDamage;
        maxDamage = _maxDamage;
        hitForce = _hitForce;
        cooldown = _cooldown;
        speed = _speed;
        lifeTime = _lifeTime;
        attackArea = _attackArea;
        bulletCount = _bulletCount;
        spawnOffset = _spawnOffset;
        delayBetweenShots = _delayBetweenShots;
        passesThroughtEnemy = _passesThroughtEnemy;
    }

    public float MinDamage { get => _minDamage; }

    public float MaxDamage { get => _maxDamage; }

    public float HitForce { get => _hitForce; }

    public float Cooldown { get => _cooldown; }

    public float Speed { get => _speed; }

    public float LifeTime { get => _lifeTime; }

    public float AttackArea { get => _attackArea; }

    public int BulletCount { get => _bulletCount; }

    public float SpawnOffset { get => _spawnOffset; }

    public int PassesThroughtEnemy { get => _passesThroughtEnemy; }

    public float DelayBetweenShots { get => _delayBetweenShots; }

    public float FreezeTime { get => _freezeTime; }
}

[CreateAssetMenu(fileName = "New WeaponLevelsData", menuName = "Weapon levels data", order = 51)]
public class WeaponLevelsData : ScriptableObject
{
    [SerializeField] private List<WeaponLevelData> _levelsData;

    public List<WeaponLevelData> LevelsData { get { return _levelsData; } }

    public WeaponLevelData GetLevel(int level) { return _levelsData[level - 1]; }
}
