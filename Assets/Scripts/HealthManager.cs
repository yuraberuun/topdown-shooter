using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;

[Serializable]
public class HealthManager
{
    private float _maxHealth;
    private float _currentHealth;

    UnityEvent onDeath = new UnityEvent();

    public void SubscribeOnDeath(UnityAction action) => onDeath.AddListener(action);

    public void UnSubscribeAllFromDeath() => onDeath?.RemoveAllListeners();

    public float MaxHealth
    {
        get => _maxHealth;
        set => _maxHealth = value;
    }

    public float CurrentHealth
    {
        get => _currentHealth;

        set
        {
            _currentHealth = Mathf.Max(0, value);

            if (_currentHealth <= 0)
            {
                onDeath?.Invoke();
            }
        }
    }
}
