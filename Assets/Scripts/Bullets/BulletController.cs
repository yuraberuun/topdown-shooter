using UnityEngine;
using System.Collections;

public class BulletController : MonoBehaviour
{
    [SerializeField] private Collider2D _collider;
    [SerializeField] private bool _isRotating;
    [SerializeField] private float _rotatingSpeed;

    private Transform _transform;

    private Vector2 _initialColliderScale;
    private Vector2 _initialScale;

    private BulletInfo _bulletInfo;

    private void Awake()
    {
        _initialColliderScale = (_collider.GetType() == typeof(BoxCollider2D)) ? _collider.GetComponent<BoxCollider2D>().size : new Vector2(_collider.GetComponent<CircleCollider2D>().radius, 0);

        _initialScale = transform.localScale;

        _bulletInfo = new BulletInfo();

        _transform = transform;
    }

    private void Update()
    {
        if (_isRotating)
            _transform.Rotate(Time.deltaTime * _rotatingSpeed * Vector3.forward);
    }

    private void ResizeBullet()
    {
        if (_bulletInfo.ResizeCollider)
        {
            if (_collider.GetType() == typeof(CircleCollider2D))
                _collider.GetComponent<CircleCollider2D>().radius = _initialColliderScale.x * _bulletInfo.AttackArea;

            else if (_collider.GetType() == typeof(BoxCollider2D))
                _collider.GetComponent<BoxCollider2D>().size = _initialColliderScale * new Vector2(_bulletInfo.AttackArea, _bulletInfo.AttackArea);

            return;
        }

        transform.localScale = _initialScale * _bulletInfo.AttackArea;
    }

    public void Shoot(BulletInfo bulletInfo)
    {
        _bulletInfo = bulletInfo;

        if (_bulletInfo.AttackArea > 1)
            ResizeBullet();

        StopAllCoroutines();
        StartCoroutine(WaitingToDestroy());
    }

    private IEnumerator WaitingToDestroy()
    {
        yield return new WaitForSeconds(_bulletInfo.LifeTime);
        _bulletInfo.DestroyAction.Invoke(gameObject);
    }

    public float GetDamage() => _bulletInfo.Damage;

    public void CollidedWithEnemy()
    {
        if (_bulletInfo.DestroyWhenCollided && _bulletInfo.PassesThroughtEnemy > 0)
           _bulletInfo.PassesThroughtEnemy--;

        else if (_bulletInfo.DestroyWhenCollided && _bulletInfo.PassesThroughtEnemy <= 0)
            _bulletInfo.CollidedAction.Invoke(gameObject);
    }

    public float GetHitForce() => _bulletInfo.HitForce;

    public float GetDelayBetweenAttacks() => _bulletInfo.DelayBetweenAttacks;

    public void Magic()
    {
        _bulletInfo.CollidedAction.Invoke(gameObject);
    }
}
