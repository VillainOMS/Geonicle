using UnityEngine;
using System.Collections;

public class EnergyStrike : MonoBehaviour
{
    [Header("Параметры удара")]
    [SerializeField] private float damageDelay = 2f;
    [SerializeField] private int damageAmount = 20;
    [SerializeField] private float activeTimeAfterDamage = 1f;

    [Header("Визуальные материалы")]
    [SerializeField] private Material warningMaterial;
    [SerializeField] private Material strikeMaterial;

    private bool hasDamaged = false;
    private bool hasAppliedDamage = false;

    private Renderer rend;

    private void Start()
    {
        rend = GetComponent<Renderer>();
        if (warningMaterial != null && rend != null)
            rend.material = warningMaterial;

        StartCoroutine(StrikeRoutine());
    }

    private IEnumerator StrikeRoutine()
    {
        yield return new WaitForSeconds(damageDelay);

        hasDamaged = true;

        if (strikeMaterial != null && rend != null)
            rend.material = strikeMaterial;

        yield return new WaitForSeconds(activeTimeAfterDamage);
        Destroy(gameObject);
    }

    private void OnTriggerStay(Collider other)
    {
        if (!hasDamaged || hasAppliedDamage) return;

        if (other.CompareTag("Player"))
        {
            PlayerStats stats = other.GetComponent<PlayerStats>();
            if (stats != null)
            {
                stats.TakeDamage(damageAmount);
                hasAppliedDamage = true;
                Debug.Log("Игрок получил урон от молнии!");
            }
        }
    }
}
