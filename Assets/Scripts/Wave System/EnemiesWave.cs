using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EnemiesWave : MonoBehaviour
{
    private GameObject _player;
    
    private List<Enemy> _enemiesPool;

    [SerializeField] private Enemy _enemyPrefab;

    private void Start()
    {
        try
        {
            _player = FindObjectOfType<Player>().gameObject;
        }
        catch (NullReferenceException e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
    
    public Enemy SpawnEnemy()
    {
        Enemy pooledEnemy = GetFirstPooledEnemy();

        if (pooledEnemy == null)
        {
            print("Create new pooled enemy");
            
            pooledEnemy = Instantiate(_enemyPrefab);
            _enemiesPool.Add(pooledEnemy);
        }
        
        pooledEnemy.gameObject.SetActive(true);
        
        pooledEnemy.Initialize(Vector2.zero, _player, () => PoolEnemy(pooledEnemy));

        return pooledEnemy;
    }

    private Enemy GetFirstPooledEnemy()
    {
        return _enemiesPool.FirstOrDefault(enemy => !enemy.gameObject.activeInHierarchy);
    }

    private Enemy[] GetPooledEnemies()
    {
        return _enemiesPool.Where(enemy => !enemy.gameObject.activeInHierarchy).ToArray();
    }

    private void PoolEnemy(Enemy enemy)
    {
        enemy.gameObject.SetActive(false);
    }
}
