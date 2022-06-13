using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;
using UnityEngine;

public class BulletInfo
{    
    private UnityAction<GameObject> _destroyAction;
    private UnityAction<GameObject> _collidedAction;

    private float _damage;
    private float _hitForce;
    private float _lifeTime;
    private float _attackArea;
    private float _speed;
    private float _delayBetweenAttacks;

    private int _passesThroughtEnemy;

    private bool _destroyWhenCollided;
    private bool _resizeCollider;

    public BulletInfo() {}

    public BulletInfo(UnityAction<GameObject> destroyAction, float damage, float hitForce, float lifeTime, float attackArea,
     float speed, float delayBetweenAttacks = 0, int passesThroughtEnemy = 0, bool destroyWhenCollided = true, bool resizeCollider = true)
    {
        _damage = damage;
        _hitForce = hitForce;
        _lifeTime = lifeTime;
        _attackArea = attackArea;
        _speed = speed;
        _delayBetweenAttacks = delayBetweenAttacks;
        _passesThroughtEnemy = passesThroughtEnemy;
        _destroyWhenCollided = destroyWhenCollided;
        _resizeCollider = resizeCollider;
        _destroyAction = destroyAction;
        _collidedAction = _destroyAction;
    }

    public UnityAction<GameObject> DestroyAction { get => _destroyAction; set => _destroyAction = value; }

    public UnityAction<GameObject> CollidedAction { get => _collidedAction; set => _collidedAction = value; }
    
    public float Damage { get => _damage; set => _damage = value; }

    public float HitForce { get => _hitForce; set => _hitForce = value; }

    public float LifeTime { get => _lifeTime; set => _lifeTime = value; }

    public float AttackArea { get => _attackArea; set => _attackArea = value; }

    public float Speed { get => _speed; set => _speed = value; }

    public int PassesThroughtEnemy { get => _passesThroughtEnemy; set => _passesThroughtEnemy = value; }

    public bool DestroyWhenCollided { get => _destroyWhenCollided; set => _destroyWhenCollided = value; }

    public bool ResizeCollider { get => _resizeCollider; set => _resizeCollider = value; }

    public float DelayBetweenAttacks { get => _delayBetweenAttacks; set => _delayBetweenAttacks = value; }
}
