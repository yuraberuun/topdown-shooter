using UnityEngine;
using System.Collections;

public class HolyWaterController : MonoBehaviour
{
    [SerializeField] private float _rotatingSpeed;

    private Vector2 movingPosition;
    private float _speed;
    private bool moving = false;

    private Transform _transform;
    private SpriteRenderer _spriteRenderer;

    private BulletInfo bulletInfo;
    private ObjectsPool holyWaterPool;

    void Start()
    {
        _transform = transform;
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        if (moving)
            Move();

        if(_spriteRenderer.enabled)
            _transform.Rotate(Time.deltaTime * _rotatingSpeed * Vector3.forward);
    }
    
    private void Move()
    {
        _transform.position = Vector2.MoveTowards(_transform.position, movingPosition, _speed * Time.deltaTime);

        if (Vector2.Distance(movingPosition, _transform.position) < 0.1f)
        {
            moving = false;
            Activate();
        }
    }

    public void SetMovingPositionAndSpeed(Vector2 pos, float speed)
    {
        movingPosition = pos;
        _speed = speed;
        moving = true;
    }

    public void SetBulletInfo(BulletInfo bl, ObjectsPool pool)
    {
        bulletInfo = bl;
        holyWaterPool = pool;
    }

    private IEnumerator StopEmission(ParticleSystem ps, GameObject attackZone)
    {
        ps.Stop();
        attackZone.GetComponent<Collider2D>().enabled = false;
        yield return new WaitForSeconds(ps.main.duration);
        attackZone.GetComponent<Collider2D>().enabled = true;
        holyWaterPool.AddObject(gameObject);
        attackZone.SetActive(false);
    }

    private void Activate()
    {
        _spriteRenderer.enabled = false;

        var attackZone = _transform.GetChild(0).gameObject;

        attackZone.SetActive(true);
        var ps = attackZone.GetComponent<ParticleSystem>();
        var emission = ps.emission;
        emission.rateOverTime = emission.rateOverTime.constant; 
        var shape = ps.shape;
        shape.radius = bulletInfo.AttackArea;
        bulletInfo.DestroyAction = delegate { StartCoroutine(StopEmission(ps, attackZone)); };

        attackZone.GetComponent<BulletController>().Shoot(bulletInfo);
        SoundManager.Instance.MakeSound(SoundType.WeaponWater);
    }
}
