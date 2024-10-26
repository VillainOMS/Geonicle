using System;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public event Action onDie;
    [SerializeField] private int maxHealth = 50;
    private int currentHealth;

    private void Awake()
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

    private void Die()
    {
        onDie?.Invoke();
        Destroy(gameObject);
    }
}
