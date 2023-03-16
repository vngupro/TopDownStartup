using NaughtyAttributes;
using System;
using UnityEngine;

public class HealthModule : MonoBehaviour
{
    /// <summary>
    /// Bind to add desired effects to Damage, Call ApplyDamage(float) when you want to Apply Damage.
    /// Died is invoked when _health <= 0 automatically when applying damage.
    /// Don't forget to unbind (-=) 
    /// </summary>
    public event Action<float> Damaged;
    /// <summary>
    /// Bind to add desired effects to Heal, Call ApplyHeal(float) when you want to Apply Heal.
    /// Don't forget to unbind (-=) 
    /// </summary>
    public event Action<float> Healed;
    /// <summary>
    /// Bind (+=) to add desired effects to Died, Call ApplyHeal(float) when you want to Apply Heal.
    /// Don't forget to unbind (-=) 
    /// </summary>
    public event Action Died;

    [SerializeField] private int _initHealth;
    public float Health { get; private set; }
    
    private void Awake()
    {
        Health = _initHealth;
    }
    public void ApplyDamage(float damage)
    {
        Health -= damage;
        Damaged?.Invoke(damage);
        if (Health <= 0)
        {
            Died?.Invoke();
        }
    }

    public void ApplyHeal(float heal)
    {
        Health = Mathf.Clamp(Health + heal, Health, _initHealth);
        Healed?.Invoke(heal);
    }

    public void Reset()
    {
        _initHealth = 100;
        Health = _initHealth;
        Died = null;
        Damaged = null;
        Healed = null;
    }

    [Button]
    public void CheatApply1Damage()
    {
        ApplyDamage(1.0f);
    }
}
