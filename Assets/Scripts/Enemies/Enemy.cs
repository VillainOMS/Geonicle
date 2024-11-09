using System;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public event Action onDie;
    [SerializeField] protected int maxHealth = 50;
    protected int currentHealth;

    protected virtual void Awake()
    {
        currentHealth = maxHealth;
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    protected virtual void Die()
    {
        onDie?.Invoke();
        Destroy(gameObject);
    }
}
