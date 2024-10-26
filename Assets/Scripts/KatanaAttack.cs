using DG.Tweening;
using UnityEngine;

public class KatanaAttack : MonoBehaviour
{
    [SerializeField] private float attackRange = 8f; // ������ �����
    [SerializeField] private float attackAngle = 90f; // ���� �����
    [SerializeField] private int damage = 25; // ����
    private Tween tween;

    public void Attack()
    {
        if (tween != null)
        {
            tween.Complete();
        }
        tween = transform.DOLocalRotate(new Vector3(0, -120, -90), 0.25f).SetLoops(2, LoopType.Yoyo);
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, attackRange);

        foreach (var hitCollider in hitColliders)
        {
            Vector3 directionToTarget = hitCollider.transform.position - transform.position;
            float angleToTarget = Vector3.Angle(transform.forward, directionToTarget);

            if (angleToTarget < attackAngle / 2)
            {
                if (hitCollider.CompareTag("Enemy"))
                {
                    Enemy enemy = hitCollider.GetComponent<Enemy>(); // �������� ��������� Enemy
                    if (enemy != null)
                    {
                        enemy.TakeDamage(damage); // ������� ���� �����
                        Debug.Log("Katana hit: " + hitCollider.name);
                    }
                }
            }
        }
    }
}
