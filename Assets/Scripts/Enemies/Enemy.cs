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
        _healthModule = gameObject.GetComponent<HealthModule>();
        _healthModule.Reset();
        _onEnemyDie = onEnemyDie;
        _healthModule.OnDied += Death;
    }

    private void Update()
    {
        Transform enemyTransform = transform;
        Vector3 enemyPosition = enemyTransform.position;
        
        // Chasing player
        enemyPosition = Vector3.MoveTowards(enemyPosition, _player.transform.position, _moveSpeed * Time.deltaTime);
        enemyTransform.position = enemyPosition;
    }


    private void OnTriggerEnter2D(Collider2D c)
    {
        if (c.CompareTag("Player"))
        {
            Explode(c.gameObject.GetComponent<Player>());
        }
    }

    private void Explode(Player p)
    {
        p.gameObject.GetComponent<HealthModule>()?.ApplyDamage(_explosionDamages);
        // End his own life
        _healthModule.ApplyDamage(_healthModule.Health);
        
    }

    private void Death()
    {
        _onEnemyDie?.Invoke();
    }

    private void OnDrawGizmos()
    {
        // Draw a yellow sphere at the transform's position
        Gizmos.color = Color.yellow;
        Gizmos.DrawSphere(transform.position, _explosionRadius);
    }
}
