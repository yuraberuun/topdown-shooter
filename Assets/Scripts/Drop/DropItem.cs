using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropItem : MonoBehaviour
{
    public enum DropType { None, Money, XP, LuckUp, XPMagnet, Chest, HP, Freeze, DestroyAll };
    [SerializeField]
    private DropType type;
    [SerializeField]
    private int amount;

    private bool isAnimated = false;
    private bool moving = false;
    private bool movingBack = false;

    private float speed = 10f;
    private float distance = 3f;
    private float moveBackMultiplier = 1.5f;

    private Vector2 movingPoint;

    private Transform _transform;
    private Transform _playerTransform;

    [SerializeField]
    private GameObject pickUpParticles;

    private void Start()
    {
        _transform = transform;
    }

    private void Update()
    {
        if (moving || movingBack)
            Move();
    }

    private void Move()
    {
        if (moving)
        {
            _transform.position = Vector2.MoveTowards(_transform.position, movingPoint, Time.deltaTime * speed);

            if ((Vector2)_transform.position == movingPoint)
            {
                movingBack = true;
                moving = false;
            }
        }

        if (movingBack)
            _transform.position = Vector2.MoveTowards(_transform.position, _playerTransform.position, Time.deltaTime * speed * moveBackMultiplier);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Magnet"))
        {
            if (isAnimated || type == DropType.Chest)
            {
                UseItem(collision);
                return;
            }

            isAnimated = true;
            MakeAnimation(collision);

            MakeSound();

            if (pickUpParticles)
            {
                var particles = Instantiate(pickUpParticles, transform.position, Quaternion.identity);
                Destroy(particles, 1f);
            }
        }
    }

    private void MakeAnimation(Collider2D collision)
    {
        _playerTransform = collision.transform.parent;

        movingPoint = _transform.position - ((collision.transform.position - _transform.position).normalized * distance);

        moving = true;
    }

    private void UseItem(Collider2D collision)
    {
        switch (type)
        {
            case DropType.XP:
                {
                    XPSystem.Instance.TakeXP(amount);
                    DropController.Instance.TakeXPItem(gameObject);
                    break;
                }
            case DropType.Money:
                {
                    Inventory.Instance.TakeMoney(amount);
                    break;
                }
            case DropType.DestroyAll:
                {
                    var enemiesOnScreen = EnemyContainer.Instance.GetEnemyOnCamera();

                    foreach (var enemy in enemiesOnScreen)
                        enemy.GetComponent<EnemyAI>().TakeDamage(amount);

                    Inventory.Instance.AddBonus("Death");

                    break;
                }
            case DropType.HP:
                {
                    var player = collision.transform.parent.gameObject;

                    player.GetComponent<HeroController>().AddHP(amount);

                    Inventory.Instance.AddBonus("HP");
                    break;
                }
            case DropType.LuckUp:
                {
                    PlayerPrefs.SetInt("BattleHeroLucky", PlayerPrefs.GetInt("BattleHeroLucky") + 1);
                    Inventory.Instance.AddBonus("Luck_Up");
                    break;
                }
            case DropType.XPMagnet:
                {
                    DropController.Instance.MakeXPMagnet();
                    Inventory.Instance.AddBonus("XP_Magnet");
                    break;
                }
            case DropType.Freeze:
                {
                    EnemyContainer.Instance.FreezeEnemies(amount);
                    EnemySpawn.AdvancedSpawner.Instance.MakeFreezeSpawn(amount);
                    Inventory.Instance.AddBonus("Freeze");
                    break;
                }
            case DropType.Chest:
                {
                    GUIController.instance.ShowChestPanel();
                    Inventory.Instance.AddBonus("Chest");
                    break;
                }
            case DropType.None:
            default:
                break;
        }

        Destroy(gameObject);
    }

    private void MakeSound()
    {
        switch (type)
        {
            case DropType.XP:
                {
                    SoundManager.Instance.MakeSound(SoundType.CollectXP);
                    break;
                }
            case DropType.Money:
                {
                    SoundManager.Instance.MakeSound(SoundType.CollectMoney);
                    break;
                }
            case DropType.HP:
                {
                    SoundManager.Instance.MakeSound(SoundType.CollectHP);
                    break;
                }
            case DropType.XPMagnet:
                {
                    SoundManager.Instance.MakeSound(SoundType.CollectMagnet);
                    break;
                }
            case DropType.Chest:
                {
                    SoundManager.Instance.MakeSound(SoundType.ChestOpen);
                    break;
                }
            case DropType.None:
            default:
                break;
        }
    }

    public void SetAnimated(bool value) => isAnimated = value;
}
