using System;
using UnityEngine;

public class EnemyObject : MonoBehaviour
{
    private Enemy _enemyData;

    public event Action<EnemyObject> OnDeath;

    public int MaxHealth { get; private set; }
    public int CurrentHealth { get; private set; }

    public bool IsDie => CurrentHealth <= 0;

    //Test
    [ContextMenu("Hit")]
    public void Hit()
    {
        TakeDamage(999);
    }

    public void Initialize(Enemy enemyData)
    {
        _enemyData = enemyData;

        gameObject.name = enemyData.EnemyName;
        MaxHealth = enemyData.EnemyHealth;
        CurrentHealth = enemyData.EnemyHealth;
    }

    public void Spawn(Vector3 position)
    {
        CurrentHealth = MaxHealth;

        transform.position = position;

        gameObject.SetActive(true);
    }

    public void TakeDamage(int damage)
    {
        if (IsDie) return;

        CurrentHealth -= damage;

        if (CurrentHealth <= 0)
        {
            OnDeath?.Invoke(this);

            QuestEvents.OnEnemyKilled?.Invoke(_enemyData);
        }
    }
}
