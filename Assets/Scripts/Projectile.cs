using UnityEngine;

public class Projectile : MonoBehaviour
{
    private int damage; // ����, ������� ����� ����������
    private PlayerHealth playerHealth;
    private Vector3 target; // ����, �� ������� ����� ��������� ������
    private Vector3 direction;
    [SerializeField] private float speed = 20f; // �������� �������

    // ����� ��� ��������� ����� � ���� �������
    public void Initialize(int damageAmount, Vector3 targetPosition, PlayerHealth playerHealthTarget)
    {
        damage = damageAmount;
        target = targetPosition;
        playerHealth = playerHealthTarget;
        direction = (target - transform.position).normalized;
        Destroy(gameObject, 8f); // ���������� ������ ����� 8 �������, ���� �� �� ����� � ������
    }

    private void Update()
    {
        // ���������� ������ � ������� ����
        transform.position += direction * speed * Time.deltaTime;
    }

    private void OnTriggerEnter(Collider other)
    {
        // �������� �� ������������ � �������
        if (other.CompareTag("Player") && playerHealth != null)
        {
            playerHealth.TakeDamage(damage);
        }
        Debug.Log(other.gameObject);
        // ���������� ������ ��� ������������ � ����� ��������
        Destroy(gameObject);
    }
}
