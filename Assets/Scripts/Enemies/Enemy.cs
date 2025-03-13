using System;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public event Action onDie;

    [SerializeField] protected int maxHealth = 50;
    protected int currentHealth;

    [SerializeField] private GameObject damageTextPrefab; // Префаб текста урона

    protected virtual void Awake()
    {
        currentHealth = maxHealth;
    }

    public void TakeDamage(int damage)
    {
        ShowDamageText(damage); // Показываем урон
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

    private void ShowDamageText(int damage)
    {
        if (damageTextPrefab != null)
        {
            GameObject damageText = Instantiate(damageTextPrefab, transform.position + Vector3.up * 1.5f, Quaternion.identity);
            damageText.GetComponent<DamageText>().SetText(damage);
        }
    }
}
