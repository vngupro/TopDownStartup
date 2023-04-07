using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EnemyPooling : MonoBehaviour
{
    private List<Enemy> _enemiesPool;
    
    private Enemy _enemyPrefab;

    public void Initialize(Enemy enemyPrefab)
    {
        _enemiesPool = new List<Enemy>();
        _enemyPrefab = enemyPrefab;
    }
    
    public Enemy SpawnEnemy()
    {
        Enemy pooledEnemy = GetFirstPooledEnemy();

        if (pooledEnemy == null)
        {
            pooledEnemy = Instantiate(_enemyPrefab);
            _enemiesPool.Add(pooledEnemy);
        }
        else
        {
            pooledEnemy.gameObject.SetActive(true);
        }

        return pooledEnemy;
    }

    public bool InitializeEnemy(Enemy pooledEnemy, Vector3 position, GameObject player, Enemy.OnEnemyDie onEnemyDie)
    {
        Enemy.OnEnemyDie onEnemyDieAction = () => PoolEnemy(pooledEnemy);
        if (_enemiesPool.Exists((x) => x == pooledEnemy))
        {
            pooledEnemy.Initialize(position, player, onEnemyDieAction);
            return false;
        }
        
        if (onEnemyDie != null) onEnemyDieAction += onEnemyDie;
        
        pooledEnemy.Initialize(position, player, onEnemyDieAction);

        return true;
    }

    public Enemy GetFirstPooledEnemy() => _enemiesPool.FirstOrDefault(enemy => !enemy.gameObject.activeInHierarchy);

    public Enemy[] GetPooledEnemies() => _enemiesPool.Where(enemy => !enemy.gameObject.activeInHierarchy) as Enemy[];

    private void PoolEnemy(Enemy enemy)
    {
        enemy.gameObject.SetActive(false);
    }
}
