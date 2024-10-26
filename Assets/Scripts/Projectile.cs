using UnityEngine;

public class Projectile : MonoBehaviour
{
    private int damage; // ����, ������� ����� ����������
    private Vector3 target; // ����, �� ������� ����� ��������� ������
    [SerializeField] private float speed = 20f; // �������� �������

    // ����� ��� ��������� ����� � ���� �������
    public void Initialize(int damageAmount, Vector3 targetPosition)
    {
        damage = damageAmount;
        target = targetPosition;
        Destroy(gameObject, 8f); // ���������� ������ ����� 3 �������, ���� �� �� ����� � ������
    }

    private void Update()
    {
        // ���������� ������ � ������� ����
        Vector3 direction = (target - transform.position).normalized;
        transform.position += direction * speed * Time.deltaTime;

        // ���� ������ ������ � ���� (������), ���������� ���
        if (Vector3.Distance(transform.position, target) < 0.5f)
        {
            Destroy(gameObject); // ���������� ������, ���� �� ������ ����
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        // �������� �� ������������ � �������
        if (other.CompareTag("Player"))
        {
            // ����� �������� ��� ��� ��������� ����� ������
            // ��������:
            // PlayerHealth playerHealth = other.GetComponent<PlayerHealth>();
            // if (playerHealth != null) 
            // {
            //     playerHealth.TakeDamage(damage);
            // }

            Debug.Log($"Player hit! Damage: {damage}"); // ������-��� ��� ������������
        }

        // ���������� ������ ��� ������������ � ����� ��������
        Destroy(gameObject);
    }
}
