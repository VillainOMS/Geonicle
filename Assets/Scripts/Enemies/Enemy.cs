using System;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public event Action onDie;

    [Header("Характеристики")]
    [SerializeField] protected int maxHealth = 50;
    protected int currentHealth;

    [SerializeField] private GameObject damageTextPrefab;

    [Header("Боевая сила")]
    [SerializeField] protected int baseDamage = 10;
    protected int currentDamage;

    [Header("Элитные настройки")]
    [SerializeField] private bool isElite = false;
    [SerializeField] private float eliteHealthMultiplier = 1.5f;
    [SerializeField] private float eliteDamageMultiplier = 1.3f;
    [SerializeField] private GameObject eliteMarker;

    public bool IsDead => currentHealth <= 0;

    protected virtual void Awake()
    {
        currentHealth = maxHealth;
        currentDamage = baseDamage;

        if (isElite)
        {
            ApplyEliteModifiers();
        }
    }

    public void MakeElite()
    {
        isElite = true;
        ApplyEliteModifiers();
    }

    private void ApplyEliteModifiers()
    {
        maxHealth = Mathf.RoundToInt(maxHealth * eliteHealthMultiplier);
        currentHealth = maxHealth;

        currentDamage = Mathf.RoundToInt(baseDamage * eliteDamageMultiplier);

        if (eliteMarker != null)
        {
            eliteMarker.SetActive(true);
        }
    }

    public int GetDamage()
    {
        return currentDamage;
    }

    public virtual void TakeDamage(int damage)
    {
        ShowDamageText(damage);
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
