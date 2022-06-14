using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using TMPro;
using EnemySpawn;

public class EnemyAI : CharacterController
{
    [SerializeField] private GameObject _takenDamageText;
    [SerializeField] private Transform _playerTransform;

    [SerializeField] private float _attackCooldown = 0.1f;
    [SerializeField] private float _timeToChangeDirection = 1f;
    [SerializeField] private float _damage = 1;
    [SerializeField] private float hitForceCoeficient = 1;
    [SerializeField] float spawnBossOffset = 1;
    [SerializeField] float maxHPCoefficient = 1.1f;
    Vector2 moveDir;

    [SerializeField] private Collider2D _collider;

    [SerializeField] private bool _isSpecial;

    private bool _isFreezed = false;

    Coroutine startedGivingDamageCoroutine;
    Coroutine startedHolyWaterCoroutine;
    Coroutine startedDamageEffectCoroutine;
    Coroutine startedFreezeEffectCoroutine;

    UnityEvent<GameObject> destroyEnemyEvent;

    [SerializeField] UnityEvent onExitFromScreen;

    [SerializeField] private Color damageColor;
    private Color startColor;
    private SpriteRenderer spriteRenderer;
    
    Rigidbody2D rb;

    private Transform parent;

    bool processBulletCollision = true;

    private Animator _animator;

    public override void Start()
    {
        objTransform = transform;
        _healthManager = new HealthManager();

        ResetCharacteristic();

        _healthManager.CurrentHealth = _healthManager.MaxHealth;

        SetMaxHP(GetMaxHP() + GetMaxHP() * 2 * AdvancedSpawner.Instance.minutsCounter / 10);
        destroyEnemyEvent = new UnityEvent<GameObject>();

        _animator = GetComponent<Animator>();

        if (!CompareTag("Lantern"))
        {
            _healthManager.SubscribeOnDeath(EnemyContainer.Instance.CountDeadEnemies);
            destroyEnemyEvent.AddListener(EnemyContainer.Instance.RemoveEnemy);
            EnemyContainer.Instance.AddEnemy(gameObject);
            _healthManager.SubscribeOnDeath(StopAttackPlayerCoroutine);
            onDamageTaken.AddListener(delegate { SoundManager.Instance.MakeSound(SoundType.GiveDamage); });
            _healthManager.SubscribeOnDeath(delegate { StartCoroutine(DestroyWithAnimation()); });
        }

        _healthManager.SubscribeOnDeath(MakeDrop);

        if (CompareTag("Lantern"))
            _healthManager.SubscribeOnDeath(delegate { Destroy(gameObject); });

        onDamageTaken.AddListener(ShowTakenDamage);
        onDamageTaken.AddListener(delegate
        {
            if (startedDamageEffectCoroutine != null)
                StopCoroutine(startedDamageEffectCoroutine);

            startedDamageEffectCoroutine = StartCoroutine(TakeDamageEffect());
        });

        InvokeRepeating("UpdateDirection", 0, _timeToChangeDirection);

        rb = GetComponent<Rigidbody2D>();

        spriteRenderer = GetComponent<SpriteRenderer>();
        startColor = spriteRenderer.color;
    }

    private IEnumerator DestroyWithAnimation()
    {
        _animator.speed = 1;

        spriteRenderer.color = startColor;
        _animator.SetTrigger("Death");
        _collider.isTrigger = true;

        if (_animator.GetCurrentAnimatorClipInfo(0).Length > 0)
            yield return new WaitForSeconds(_animator.GetCurrentAnimatorClipInfo(0)[0].clip.length);

        Destroy(gameObject);
    }

    private void StopAttackPlayerCoroutine()
    {
        _playerTransform.gameObject.GetComponent<HeroController>()?.StopTakingDamage();
    }

    private void MakeDrop()
    {
        var pos = objTransform.position;
        if (CompareTag("Lantern"))
            DropController.Instance.SpawnDropToLantern(pos);
        // else if(_isSpecial)
        //     DropController.Instance.SpawnDropSpecialEnemy(pos);
        else
            DropController.Instance.SpawnXP(pos);
    }
    
    public void ResetCharacteristic()
    {
        speed = initialSpeed;

        maxHealth = initialMaxHealth;

        _healthManager.MaxHealth = maxHealth;
    }

    void ShowTakenDamage(float damage)
    {
        if (!SettingsLoader.Instance.ShowEnemyDamageText || !_takenDamageText)
            return;

        var canvas = Instantiate(_takenDamageText, objTransform.position, Quaternion.identity, parent).GetComponent<Canvas>();
        canvas.GetComponentInChildren<TextMeshProUGUI>().text = damage.ToString();

        Destroy(canvas.gameObject, 1f);
    }

    void Update()
    {
    }

    private void UpdateDirection()
    {
        if (_isFreezed || !_playerTransform)
            return;

        var enemyPos = objTransform.position;
        var playerPos = _playerTransform.position;

        moveDir = (playerPos - enemyPos).normalized;

        int scaleVar = enemyPos.x > playerPos.x ? -1 : 1;

        objTransform.localScale = new Vector3(scaleVar, 1, 1);

        rb.velocity = moveDir * speed;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (HP <= 0f)
            return;

        if (collision.CompareTag("Bullet"))
            ProcessBullet(collision.GetComponent<BulletController>());

        if (collision.CompareTag("FreezeBullet") && !CompareTag("Lantern"))
        {
            var bullet = collision.transform.parent.parent.parent.GetComponent<FreezeBulletController>();

            if (startedFreezeEffectCoroutine != null)
                StopCoroutine(startedFreezeEffectCoroutine);

            startedFreezeEffectCoroutine = StartCoroutine(Freeze(bullet.GetFreezeTime()));
        }

        if (collision.CompareTag("HolyWater"))
        {
            var bullet = collision.GetComponent<BulletController>();

            startedHolyWaterCoroutine = StartCoroutine(TakingDamage(bullet.GetDamage(), bullet.GetDelayBetweenAttacks()));
        }
    }

    void ProcessBullet(BulletController bullet)
    {
        TakeDamage(bullet.GetDamage());

        Vector2 forceVector = -(bullet.transform.position - objTransform.position).normalized * bullet.GetHitForce() * hitForceCoeficient;

        if ((bullet.name.Contains("Bible") || bullet.name.Contains("Sword")) && _playerTransform)
            forceVector = -(_playerTransform.position - objTransform.position).normalized * bullet.GetHitForce() * hitForceCoeficient;

        rb.AddForce(forceVector, ForceMode2D.Impulse);

        CancelInvoke();
        InvokeRepeating("UpdateDirection", bullet.GetHitForce() * hitForceCoeficient / 5, _timeToChangeDirection);

        bullet.CollidedWithEnemy();
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("EnemyAliveZone"))
            onExitFromScreen.Invoke();

        if (collision.CompareTag("HolyWater") && startedHolyWaterCoroutine != null)
            StopCoroutine(startedHolyWaterCoroutine);
    }

    private IEnumerator Freeze(float delay)
    {
        var color = spriteRenderer.color;
        rb.velocity = Vector2.zero;
        _collider.isTrigger = true;
        _isFreezed = true;
        hitForceCoeficient = 0f;
        spriteRenderer.color = new Color(0, 255, 255);

        _animator.speed = 0;

        yield return new WaitForSeconds(delay);
        _isFreezed = false;
        spriteRenderer.color = color;
        _collider.isTrigger = false;
        hitForceCoeficient = 1f;

        _animator.speed = 1;

        UpdateDirection();
    }

    private IEnumerator TakingDamage(float damage, float attackCooldown)
    {
        while (true)
        {
            if (HP <= 0f)
                yield break;

            TakeDamage(damage);

            yield return new WaitForSeconds(attackCooldown);
        }
    }

    private IEnumerator TakeDamageEffect()
    {
        spriteRenderer.color = damageColor;

        yield return new WaitForSeconds(.3f);

        if(_isFreezed)
            spriteRenderer.color = new Color(0, 255, 255);

        spriteRenderer.color = startColor;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && !_isFreezed)
        {
            StopAttackPlayerCoroutine();
            collision.gameObject.GetComponent<HeroController>().StartTakingDamage(_damage, _attackCooldown);
        }  
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && startedGivingDamageCoroutine != null)
            collision.gameObject.GetComponent<HeroController>().StopTakingDamage();
    }

    public void Respawn()
    {
        objTransform = transform;

        Vector2 pos = _playerTransform.position;

        var angleRadians = Random.Range(0, Mathf.PI * 2);

        var _x = Mathf.Cos(angleRadians);
        var _y = Mathf.Sin(angleRadians);

        Vector2 spawnOffset = new Vector2(_x, _y) * spawnBossOffset;

        objTransform.position = pos + spawnOffset;
    }

    public void FreezeEnemy(float time)
    {
        if (startedFreezeEffectCoroutine != null)
            StopCoroutine(startedFreezeEffectCoroutine);

        startedFreezeEffectCoroutine = StartCoroutine(Freeze(time));
    }

    public void DestroyEnemy() => Destroy(gameObject);

    public float GetDamage() => _damage;

    public void SetTarget(Transform target) => _playerTransform = target;

    public void SetIsSpecial(bool value)
    {
        _isSpecial = value;

        if (_isSpecial)
        {
            _damage *= 2;
            _healthManager.MaxHealth *= 3;
            _healthManager.CurrentHealth = _healthManager.MaxHealth;
        }
    }

    private void OnDestroy() => destroyEnemyEvent?.Invoke(gameObject);

    public void SetAliveTime(float time) => Destroy(gameObject, time);

    public void SetMaxHP(float value)
    {
        _healthManager.MaxHealth = value * maxHPCoefficient;
        _healthManager.CurrentHealth = _healthManager.MaxHealth;
    }

    public float GetMaxHP() => _healthManager.MaxHealth;

    public void SetParentDamageText(Transform _parent) => parent = _parent;
}