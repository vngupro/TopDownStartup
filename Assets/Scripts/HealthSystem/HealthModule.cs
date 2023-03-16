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
    private float _health;

    public void ApplyDamage(float f)
    {
        _health -= f;
        Damaged?.Invoke(f);
        if (_health <= 0)
        {
            Died?.Invoke();
        }
    }

    public void ApplyHeal(float f)
    {
        _health = Mathf.Clamp(_health + f, _health, _initHealth);
        Healed?.Invoke(f);
    }

    [Button]
    public void CheatApply1Damage()
    {
        ApplyDamage(1);
    }
}
