using UnityEngine;

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

    public void Initialize(Vector2 spawnPoint, GameObject player, OnEnemyDie onEnemyDie)
    {
        transform.position = spawnPoint;

        _player = player;
        _playerMask = player.layer;
        
        // reset health module
        _healthModule.Reset();
        
        _healthModule.OnDied += Death;
    }

    private void Update()
    {
        Transform enemyTransform = transform;
        Vector3 enemyPosition = enemyTransform.position;
        
        // Chasing player
        enemyPosition = Vector3.MoveTowards(enemyPosition, _player.transform.position, _moveSpeed * Time.deltaTime);
        enemyTransform.position = enemyPosition;

        Collider2D[] playersCollidedCollider = Physics2D.OverlapCircleAll(enemyPosition, _explosionRadius, _playerMask);
        
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
            playerCollider.gameObject.GetComponent<HealthModule>()?.ApplyDamage(_explosionDamages);
        }
        
        // End his own life
        _healthModule.ApplyDamage(_healthModule.Health);
    }

    private void Death()
    {
        _onEnemyDie?.Invoke();
    }
}
