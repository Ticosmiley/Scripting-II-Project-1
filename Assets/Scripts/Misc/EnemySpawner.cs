using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] GameObject _enemy;

    [SerializeField] int _totalEnemies = 10;
    [SerializeField] float _spawnRate = 20;
    int _enemiesSpawned = 0;
    float _lastSpawn = 0;

    private void Start()
    {
        SpawnEnemy();
        _enemiesSpawned++;
        _lastSpawn = Time.fixedTime;
    }

    private void Update()
    {
        if (_totalEnemies == 0)
        {
            if (Time.fixedTime - _lastSpawn >= _spawnRate)
            {
                SpawnEnemy();
                _lastSpawn = Time.fixedTime;
            }
        }
        else
        {
            if (_enemiesSpawned < _totalEnemies && Time.fixedTime - _lastSpawn >= _spawnRate)
            {
                SpawnEnemy();
                _lastSpawn = Time.fixedTime;
                _enemiesSpawned++;
            }
        }
    }

    void SpawnEnemy()
    {
        Instantiate(_enemy, transform.position, Quaternion.identity);
    }
}
