using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Enemy : MonoBehaviour
{
    private HealthModule _healthModule;

    private GameObject _player;
    private LayerMask _playerMask;

    public delegate void OnEnemyDie();
    private OnEnemyDie _onEnemyDie;

    [Header("Stats")]
    [SerializeField] private float _moveSpeed;

    [Header("Explosion")]
    [SerializeField] private float _explosionRadius;
    [SerializeField] private float _explosionDamages;
    
    private void Awake()
    {
        // Get player
        //_playerMask = _player.layer;
    }

    public void Initialize(Vector2 spawnPoint, OnEnemyDie onEnemyDie)
    {
        transform.position = spawnPoint;
        
        // reset health module
        // health, events
        
        _healthModule.Died += Death;
    }

    private void Update()
    {
        // Chasing player
        transform.position = Vector3.MoveTowards(transform.position, _player.transform.position, _moveSpeed * Time.deltaTime);

        Collider2D[] playersCollidedCollider = Physics2D.OverlapCircleAll(transform.position, _explosionRadius, _playerMask);
        
        if (playersCollidedCollider.Length > 0)
        {
            Explode(playersCollidedCollider);
        }
    }

    private void Explode(Collider2D[] playersCollider)
    {
        // Deal damages to players
        foreach (Collider2D playerCollider in playersCollider)
        {
            playerCollider.gameObject.GetComponent<HealthModule>().ApplyDamage(_explosionDamages);
        }
        
        // End his own life
        //_healthModule.ApplyDamage(_healthModule.CurrentLife);
    }

    private void Death()
    {
        _onEnemyDie?.Invoke();
    }
}
