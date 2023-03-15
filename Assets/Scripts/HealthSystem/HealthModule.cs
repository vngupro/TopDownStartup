using System;
using UnityEngine;

public class HealthModule : MonoBehaviour
{
    public event Action<float> Damaged;
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
}
