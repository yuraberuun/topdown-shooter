using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class HeroController : CharacterController
{
    [SerializeField] private float initialMagnet = 1;

    [SerializeField] private GameObject shieldGameObject;

    [SerializeField] Slider healthBar;

    [SerializeField] private HeroBuffsData _heroBuffsData;

    [SerializeField] private GameObject _magnet;
    private CircleCollider2D _magnetCollider;

    protected float magnet = 1;
    private float armor = 1;  

    private int _shieldsCount;

    [SerializeField] private Color damageColor;
    private Color startColor;
    [SerializeField] private SpriteRenderer sprite;
    [SerializeField] private Animator animator;
    [SerializeField] private float _deathAnimationLength = 2f;
    [SerializeField] private ParticleSystem damagePS;

    private bool _isDeathless = false;
    [SerializeField] private float _deathlessTime = 1f;
    [SerializeField] private Color _deathlessColor;
    Coroutine deathlessCoroutine;

    private bool _isDead = false;

    private Vector2 _movementDirection;

    private Coroutine takingDamageCoroutine;

    override public void Start()
    {
        _magnetCollider = _magnet.GetComponent<CircleCollider2D>();

        objTransform = transform;
        _healthManager = new HealthManager();

        ResetCharacteristic();

        _healthManager.CurrentHealth = _healthManager.MaxHealth;

        _healthManager.SubscribeOnDeath(delegate { StartCoroutine(MakeDeathAnimation()); });

        UpdateHealthBar();

        startColor = sprite.color;
    }

    private IEnumerator MakeDeathAnimation()
    {
        _isDead = true;
        sprite.color = damageColor;
        animator.SetTrigger("Death");
        GetComponent<Collider2D>().isTrigger = true;

        damagePS.Play();

        yield return new WaitForSeconds(_deathAnimationLength);

        GUIController.instance.EndGame();
    }

    public void ResetCharacteristic()
    {
        maxHealth = initialMaxHealth * _heroBuffsData.GetMaxHPBuff();
        speed = initialSpeed * _heroBuffsData.GetSpeedBuff();
        magnet = initialMagnet * _heroBuffsData.GetMagnetBuff();
        armor = _heroBuffsData.GetArmorBuff();

        _healthManager.MaxHealth = maxHealth;

        UpdateMagnetCollider();
    }

    private void UpdateMagnetCollider() => _magnetCollider.transform.localScale.Set(magnet, magnet, 1);

    void Update()
    {
        Move();
    }

    private void Move()
    {
        if (_isDead)
            return;

        if (GameStatus.Instance.status == GameState.Play)
        {
            var horizontal = Input.GetAxisRaw("Horizontal");
            var vertical = Input.GetAxisRaw("Vertical");

            animator.SetBool("Move", !(horizontal == 0f && vertical == 0f));

            _movementDirection = new Vector2(horizontal, vertical).normalized * speed * Time.deltaTime;
            objTransform.Translate(_movementDirection);
            
            if(horizontal != 0)
                sprite.flipX = horizontal > 0;
        }  
    }

    private void RemoveShield()
    {
        _shieldsCount--;
        shieldGameObject.GetComponent<ShieldController>().RemoveShield();
    }

    public void AddShield() => _shieldsCount++;

    override public void TakeDamage(float damage)
    {
        if (_shieldsCount > 0)
            RemoveShield();
        else
        {
            SoundManager.Instance.MakeSound(SoundType.TakeDamage);
            base.TakeDamage(damage);
        }
    }

    void UpdateHealthBar() => healthBar.value = HP / _healthManager.MaxHealth;

    public void StartTakingDamage(float damage, float attackCooldown)
    {
        StopTakingDamage();

        takingDamageCoroutine = StartCoroutine(TakingDamage(damage, attackCooldown));
    }

    public void StopTakingDamage()
    {
        if (takingDamageCoroutine != null)
            StopCoroutine(takingDamageCoroutine);

        ResetColor();
    }

    private IEnumerator TakingDamage(float damage, float attackCooldown)
    {
        ResetColor();

        while (true)
        {
            if (_isDeathless)
            {
                yield return null;
                continue;
            }

            if (_shieldsCount == 0 && !_isDeathless)
            {
                ResetColor();

                sprite.color = damageColor;
                damagePS.Play();
            }

            TakeDamage(damage - armor);

            UpdateHealthBar();

            yield return new WaitForSeconds(attackCooldown);

            ResetColor();
        }
    }

    public void ResetColor()
    {
        if (_isDeathless)
            return;

        sprite.color = startColor;
        damagePS.Stop();
    }

    public void SetSprite(Sprite sp) => sprite.sprite = sp;

    public void AddHP(float value)
    {
        var newHP = _healthManager.CurrentHealth + value;

        _healthManager.CurrentHealth = newHP <= _healthManager.MaxHealth ? newHP : _healthManager.MaxHealth;

        UpdateHealthBar();
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
            StopTakingDamage();
    }

    private IEnumerator MakeDeathLess()
    {
        _isDeathless = true;

        sprite.color = _deathlessColor;

        yield return new WaitForSeconds(_deathlessTime);

        sprite.color = startColor;

        _isDeathless = false;
    }

    public void MakeDeathless()
    {
        StopTakingDamage();

        if (deathlessCoroutine != null)
            StopCoroutine(deathlessCoroutine);

        deathlessCoroutine = StartCoroutine(MakeDeathLess());
    }

    public void SetAnimatorController(AnimatorOverrideController controller) => animator.runtimeAnimatorController = controller;
}