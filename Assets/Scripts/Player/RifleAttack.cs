using UnityEngine;
using DG.Tweening;

public class RifleAttack : MonoBehaviour
{
    [SerializeField] private int damage = 10;
    [SerializeField] private float range = 100f;
    [SerializeField] private Camera playerCamera;
    [SerializeField] private float cooldown = 1.5f; // ����� �������� ��� ��������
    private float nextAttackTime = 0f;
    private Tween tween;

    public void Shoot()
    {
        if (Time.time < nextAttackTime) return; // ���� ������� ��� �� ���������� - ������� �� ������

        AudioManager.Instance.PlayShootSound();

        nextAttackTime = Time.time + cooldown; // ��������� ����� ��������� �����

        if (tween != null)
        {
            tween.Complete();
        }
        tween = transform.DOLocalRotate(new Vector3(-45, 0, 0), cooldown / 2).SetLoops(2, LoopType.Yoyo);

        RaycastHit hit;
        if (Physics.Raycast(playerCamera.transform.position, playerCamera.transform.forward, out hit, range))
        {
            if (hit.collider.CompareTag("Enemy"))
            {
                Enemy enemy = hit.collider.GetComponent<Enemy>();
                if (enemy != null)
                {
                    enemy.TakeDamage(damage);
                    Debug.Log("Rifle hit: " + hit.collider.name);
                }
            }
        }
    }
}
