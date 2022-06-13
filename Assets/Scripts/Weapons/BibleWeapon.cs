using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BibleWeapon : WeaponController
{
    [Header("Another")]
    [SerializeField] private float _radius;

    private List<Transform> _rotateObjects = new List<Transform>();
    private List<Vector3> _bulletPositions = new List<Vector3>();

    private float _currentDegree = 0;

    private bool _rotate = false;
    private bool _moveToOrbit = false;

    void Update()
    {
        if(_moveToOrbit)
            MoveToOrbit();

        if (_rotate)
            Rotate();
    }

    private void MoveToOrbit()
    {
        for (int i = 0; i < _rotateObjects.Count; i++)
            _rotateObjects[i].position = Vector2.MoveTowards(_rotateObjects[i].position, _bulletPositions[i], speed / 10 * Time.deltaTime);

        if(_rotateObjects[0].position == _bulletPositions[0])
        {
            _moveToOrbit = false;
            _rotate = true;
        } 
    }

    private void Rotate()
    {      
        transform.rotation = Quaternion.Euler(0, 0, -_currentDegree);

        foreach (Transform transform in _rotateObjects)
            transform.SetPositionAndRotation(transform.position, Quaternion.Euler(0, 0, transform.rotation.z));

        _currentDegree += speed * Time.deltaTime;
    }

    override protected void Shoot()
    {
        _rotateObjects.Clear();
        _bulletPositions.Clear();
        _rotate = false;
        for (int degree = 0; degree < 360; degree += 360 / bulletCount)
        {
            var bulletPosition = new Vector3(_radius * attackArea * Mathf.Cos(degree * Mathf.Deg2Rad), _radius * attackArea * Mathf.Sin(degree * Mathf.Deg2Rad), 0);

            var bulletInfo = CreateBulletInfo(false, false);
            bulletInfo.AttackArea = 1.0f;

            var bullet = objectsPool.GetObject();
            
            bullet.transform.SetPositionAndRotation(transform.position, Quaternion.identity);
            bullet.GetComponent<BulletController>().Shoot(bulletInfo);
            
            _rotateObjects.Add(bullet.transform);
            _bulletPositions.Add(transform.position + bulletPosition);
        }
        _moveToOrbit = true;
    }

    protected override IEnumerator ShootsWithDelayCorutine()
    {
        yield return new WaitForSeconds(0.0f);
        Shoot();
    }
}