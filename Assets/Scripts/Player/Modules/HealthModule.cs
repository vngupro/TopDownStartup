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
    public event Action<float> OnDamaged;
    /// <summary>
    /// Bind to add desired effects to Heal, Call ApplyHeal(float) when you want to Apply Heal.
    /// Don't forget to unbind (-=) 
    /// </summary>
    public event Action<float> OnHealed;
    /// <summary>
    /// Bind (+=) to add desired effects to Died, Call ApplyHeal(float) when you want to Apply Heal.
    /// Don't forget to unbind (-=) 
    /// </summary>
    public event Action OnDied;

    [SerializeField] private int _initHealth;
    public float Health { get; private set; }
    
    private void Awake()
    {
        Health = _initHealth;
    }
    public void ApplyDamage(float damage, GameObject damageGiver)
    {
        Health -= damage;
        OnDamaged?.Invoke(damage);
        if (Health <= 0)
        {
            if(damageGiver.TryGetComponent<Player>(out Player p))
            {
                damageGiver.GetComponent<ScoreModule>().AddToScore(1);
            }
            OnDied?.Invoke();
        }
    }

    public void ApplyHeal(float heal)
    {
        Health = Mathf.Clamp(Health + heal, Health, _initHealth);
        OnHealed?.Invoke(heal);
    }

    public void Reset()
    {
        _initHealth = 100;
        Health = _initHealth;
        OnDied = null;
        OnDamaged = null;
        OnHealed = null;
    }

    [Button]
    public void CheatApply1Damage()
    {
        ApplyDamage(1.0f, gameObject);
    }
}
