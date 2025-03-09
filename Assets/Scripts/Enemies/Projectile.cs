using UnityEngine;

public class Projectile : MonoBehaviour
{
    private int damage; // ����, ������� ����� ����������
    private PlayerStats playerStats; // ������� �������� ������
    private Vector3 direction; // ����������� ������
    [SerializeField] private float speed = 20f; // �������� �������
    [SerializeField] private float lifetime = 8f; // ����� ����� �������

    // ����� ��� ��������� ����� � ���� �������
    public void Initialize(int damageAmount, Vector3 targetPosition, PlayerStats targetStats)
    {
        damage = damageAmount;
        playerStats = targetStats;
        direction = (targetPosition - transform.position).normalized;

        Destroy(gameObject, lifetime); // ���������� ������ ����� lifetime ������, ���� �� �� ����� � ������
    }

    private void Update()
    {
        // ���������� ������ � ������� ����
        transform.position += direction * speed * Time.deltaTime;
    }

    private void OnTriggerEnter(Collider other)
    {
        // �������� �� ������������ � �������
        if (other.CompareTag("Player") && playerStats != null)
        {
            playerStats.TakeDamage(damage);
            Destroy(gameObject);
        }

        // ���������, �� �������� �� ������ ������� (��������, ����)
        if (!other.CompareTag("Enemy") && !other.CompareTag("Projectile"))
        {
            Destroy(gameObject);
        }
    }
}
