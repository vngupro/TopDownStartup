using NaughtyAttributes;
using System;
using UnityEngine;

public class HealthModule : MonoBehaviour
{
    private static IAudioService _audioService;

    private AudioClip _damageSound;
    private AudioClip _dieSound;
    private AudioClip _healthSound;

    [SerializeField] private ParticleSystem PS_Damage;
    [SerializeField] private ParticleSystem PS_DeathPlayer;
    [SerializeField] private ParticleSystem PS_Heal;


    /// <summary>
    /// Bind to add desired effects to OnDamaged, Call ApplyDamage(float) when you want to Apply Damage.
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
    /// Bind to add desired effects to OnDied
    /// Don't forget to unbind (-=) 
    /// </summary>
    public event Action OnDied;

    [SerializeField] private int _initHealth;
    public float Health { get; private set; }
    
    private void Awake()
    {
        _audioService ??= Services.Resolve<IAudioService>();
        Health = _initHealth;
    }

    private void Start()
    {
        _damageSound = Resources.Load("AudioResources/SFX/DamageSFX") as AudioClip;
        _dieSound = Resources.Load("AudioResources/SFX/DieSFX") as AudioClip;
        _healthSound = Resources.Load("AudioResources/SFX/HealthSFX") as AudioClip;
    }

    public void ApplyDamage(float damage)
    {
        Health -= damage;
        OnDamaged?.Invoke(damage);
        if (Health <= 0)
        {
            OnDied?.Invoke();
            _audioService.PlaySound(AUDIO_CHANNEL.SFX, _dieSound);
            ParticleSystem ParticleDeath = Instantiate<ParticleSystem>(PS_DeathPlayer, this.transform);
            Destroy(ParticleDeath.gameObject, ParticleDeath.main.duration);
            return;
        }

        ParticleSystem ParticleDamage = Instantiate<ParticleSystem>(PS_Damage, this.transform);
        Destroy(ParticleDamage.gameObject, ParticleDamage.main.duration);
        _audioService.PlaySound(AUDIO_CHANNEL.SFX, _damageSound);
    }

    public void ApplyHeal(float heal)
    {
        Health = Mathf.Clamp(Health + heal, Health, _initHealth);
        OnHealed?.Invoke(heal);

        ParticleSystem ParticleHeal = Instantiate<ParticleSystem>(PS_Heal, this.transform);
        Destroy(ParticleHeal.gameObject, ParticleHeal.main.duration);
        _audioService.PlaySound(AUDIO_CHANNEL.SFX, _healthSound);
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
        ApplyDamage(1.0f);
    }
}
