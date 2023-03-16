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
    public void ApplyDamage(float f)
    {
        Health -= f;
        Damaged?.Invoke(f);
        if (Health <= 0)
        {
            Died?.Invoke();
        }
    }

    public void ApplyHeal(float f)
    {
        Health = Mathf.Clamp(Health + f, Health, _initHealth);
        Healed?.Invoke(f);
    }

    private void Reset()
    {
        _initHealth = 100;
    }
    
    public void ResetModule()
    {
        Health = _initHealth;
        Died = null;
        Damaged = null;
        Healed = null;
    }
}
