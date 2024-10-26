using UnityEngine;
using DG.Tweening;

public class RifleAttack : MonoBehaviour
{
    [SerializeField] private int damage = 10;
    [SerializeField] private float range = 100f;
    [SerializeField] private Camera playerCamera; // Камера игрока
    private Tween tween;

    public void Shoot()
    {
        if (tween != null)
        {
            tween.Complete();
        }
        tween = transform.DOLocalRotate(new Vector3(-45, 0, 0), 0.25f).SetLoops(2, LoopType.Yoyo);

        RaycastHit hit;
        if (Physics.Raycast(playerCamera.transform.position, playerCamera.transform.forward, out hit, range))
        {
            if (hit.collider.CompareTag("Enemy"))
            {
                Enemy enemy = hit.collider.GetComponent<Enemy>(); // Получаем компонент Enemy
                if (enemy != null)
                {
                    enemy.TakeDamage(damage); // Наносим урон врагу
                    Debug.Log("Rifle hit: " + hit.collider.name);
                }
            }
        }
    }
}
