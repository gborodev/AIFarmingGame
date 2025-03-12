using System;
using UnityEngine;

[CreateAssetMenu(menuName = "Data/Enemy")]
public class Enemy : ScriptableObject
{
    private string _enemyID;

    [Header("Enemy Datas")]
    [SerializeField] private string _enemyName;
    [SerializeField] private int _enemyHealth;

    [Header("Enemy Objects")]
    [SerializeField] private EnemyObject _enemyPrefab;

    public string EnemyID => _enemyID;
    public string EnemyName => _enemyName;
    public int EnemyHealth => _enemyHealth;


    private void OnEnable()
    {
        _enemyID = Guid.NewGuid().ToString();
    }

    public EnemyObject GetEnemyObject()
    {
        return Instantiate(_enemyPrefab);
    }
}
