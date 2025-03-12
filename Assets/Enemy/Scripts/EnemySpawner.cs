using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private Enemy _enemyData;
    [SerializeField] private int _size;
    [SerializeField] private float _spawnTime = 10f;
    [SerializeField] private float _spawnDistance = 10f;

    private List<Spawner> _spawnerList = new List<Spawner>();

    private void OnValidate()
    {
        if (_enemyData != null)
        {
            gameObject.name = $"SPAWNER_{_enemyData.EnemyName.ToUpper()}";
        }
    }

    private void Start()
    {
        for (int i = 0; i < _size; i++)
        {
            EnemyObject enemy = _enemyData.GetEnemyObject();

            if (enemy != null)
            {
                enemy.Initialize(_enemyData);

                enemy.gameObject.SetActive(false);
                enemy.OnDeath += AddSpawner;

                EnemyRespawn(enemy);
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, _spawnDistance);
    }

    private void Update()
    {
        if (_spawnerList.Count > 0)
        {
            for (int i = 0; i < _spawnerList.Count; i++)
            {
                Spawner spawner = _spawnerList[i];

                bool isDone = spawner.Tick(Time.deltaTime);

                if (isDone)
                {
                    EnemyRespawn(spawner.Enemy);

                    _spawnerList.RemoveAt(i);
                }
            }
        }
    }

    private void EnemyRespawn(EnemyObject enemy)
    {
        float randomX = transform.position.x + Random.Range(_spawnDistance * -1, _spawnDistance * 1);
        float randomZ = transform.position.z + Random.Range(_spawnDistance * -1, _spawnDistance * 1);

        Vector3 spawnPoint = new Vector3(randomX, transform.position.y, randomZ);
        enemy.Spawn(spawnPoint);
    }

    private void AddSpawner(EnemyObject enemy)
    {
        enemy.gameObject.SetActive(false);

        _spawnerList.Add(new Spawner(enemy, _spawnTime));
    }

    private class Spawner
    {
        private EnemyObject _enemy;
        private float _spawnTimer;

        public EnemyObject Enemy => _enemy;

        public Spawner(EnemyObject enemy, float spawnTimer)
        {
            _enemy = enemy;
            _spawnTimer = spawnTimer;
        }

        public bool Tick(float deltaTime)
        {
            _spawnTimer -= deltaTime;

            if (_spawnTimer <= 0)
            {
                return true;
            }
            return false;
        }
    }
}
