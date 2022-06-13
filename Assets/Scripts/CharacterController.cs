using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;
using UnityEngine;

public class CharacterController : MonoBehaviour
{
    [SerializeField] protected float initialSpeed = 4f;
    [SerializeField] protected float initialMaxHealth = 100;

    protected HealthManager _healthManager;

    protected float speed;
    protected float maxHealth;

    protected Transform objTransform;

    protected UnityEvent<float> onDamageTaken = new UnityEvent<float>();

    virtual public void Start()
    {
        
    }

    virtual public float HP
    {
        get => _healthManager.CurrentHealth;

        set => _healthManager.CurrentHealth = value;
    }

    virtual public void TakeDamage(float damage)
    {
        if (HP <= 0f)
            return;

        onDamageTaken.Invoke(damage);

        HP -= damage;
    }
}
