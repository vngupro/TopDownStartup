using NaughtyAttributes;
using System;
using UnityEngine;

public class HealthModule : MonoBehaviour
{
    private static IAudioService _audioService;

    private AudioClip _damageSound;
    private AudioClip _dieSound;
    private AudioClip _healthSound;

   
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
        _audioService ??= Services.Resolve<AudioService>();
        Health = _initHealth;
    }

    private void Start()
    {
        _damageSound = Resources.Load("AudioResources/SFX/DamageSFX.mp3") as AudioClip;
        _dieSound = Resources.Load("AudioResources/SFX/DieSFX.mp3") as AudioClip;
        _healthSound = Resources.Load("AudioResources/SFX/HealthSFX.mp3") as AudioClip;
    }

    public void ApplyDamage(float f)
    {
        Health -= f;
        Damaged?.Invoke(f);
        if (Health <= 0)
        {
            Died?.Invoke();
            _audioService.PlaySound(AUDIO_CHANNEL.SFX, _dieSound);
        }
        _audioService.PlaySound(AUDIO_CHANNEL.SFX, _damageSound);
    }

    public void ApplyHeal(float f)
    {
        Health = Mathf.Clamp(Health + f, Health, _initHealth);
        Healed?.Invoke(f);
        _audioService.PlaySound(AUDIO_CHANNEL.SFX, _healthSound);
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

    [Button]
    public void CheatApply1Damage()
    {
        ApplyDamage(1.0f);
    }
}
