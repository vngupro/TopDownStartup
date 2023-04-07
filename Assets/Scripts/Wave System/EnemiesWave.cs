using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public class EnemiesWave : MonoBehaviour
{
    private const string _DEFAULT_NAME = "DEFAULT ENEMY TYPE NAME";
    
    private GameObject _player;

    private Dictionary<string, EnemyPooling> _enemyPoolingObjects;

    private int _currentWaveNumber;
    private int _currentEnemiesAliveNumber;

    [Header("Waves")]
    [SerializeField] private Enemy[] _enemiesType;
    [SerializeField] private int _waveNumber;
    [SerializeField] private int _enemiesNumberPerWave;

    private void Awake()
    {
        foreach (var enemyType in _enemiesType.Distinct())
        {
            string enemyTypeName = enemyType.GetType().FullName ?? _DEFAULT_NAME;

            GameObject nPooling = new GameObject($"Enemies pooling: {enemyTypeName}");
            nPooling.transform.SetParent(transform);
            EnemyPooling enemyPooling = nPooling.AddComponent<EnemyPooling>();
            enemyPooling.Initialize(enemyType);

            _enemyPoolingObjects.Add(enemyTypeName, enemyPooling);
        }
    }

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
    
    private void SpawnEnemyWave()
    {
        int enemyType = Random.Range(0, _enemiesType.Length);
        
        EnemyPooling enemyPooling = _enemyPoolingObjects[_enemiesType[enemyType].GetType().FullName ?? _DEFAULT_NAME];

        for (int i = 0; i < _enemiesNumberPerWave - 1; i++)
        {
            Enemy nPooledEnemy = enemyPooling.SpawnEnemy();
            enemyPooling.InitializeEnemy(nPooledEnemy, _player, () => OneEnemyDied(nPooledEnemy));
        }
    }

    private void OneEnemyDied(Enemy enemy)
    {
        print("Enemy died");
        _currentEnemiesAliveNumber--;

        if (_currentEnemiesAliveNumber > 0) return;

        _currentWaveNumber++;
        
        if (_currentWaveNumber >= _waveNumber) // gg
            return;
        
        SpawnEnemyWave();
    }
}
