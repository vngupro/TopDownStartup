using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleModule : MonoBehaviour
{
    [SerializeField] private ParticleSystem _deathParticles;
    [SerializeField] private ParticleSystem _damageParticles;
    [SerializeField] private ParticleSystem _healParticles;


    public void SpawnDeathParticles()
    {
        ParticleSystem temp = Instantiate<ParticleSystem>(_deathParticles);
        Destroy(temp, _deathParticles.main.duration);
    }
    public void SpawnDamageParticles()
    {
        ParticleSystem temp = Instantiate<ParticleSystem>(_damageParticles);
        Destroy(temp, _damageParticles.main.duration);
    }
    public void SpawnHealParticles()
    {
        ParticleSystem temp = Instantiate<ParticleSystem>(_healParticles);
        Destroy(temp, _healParticles.main.duration);
    }
}
