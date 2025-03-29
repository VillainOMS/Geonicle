using UnityEngine;

public class SpawnPoint : MonoBehaviour
{
    [SerializeField] private Enemy enemyPrefab;
    [SerializeField] private bool isElitePoint = false;

    private Enemy enemy;
    public Enemy Enemy => enemy;

    public void SpawnEnemy(bool isEliteWave)
    {
        // ќбычный спавнпоинт работает всегда.
        // Ёлитный Ч только при элитной волне.
        if (!isElitePoint || (isElitePoint && isEliteWave))
        {
            enemy = Instantiate(enemyPrefab, transform.position, Quaternion.identity);

            if (isElitePoint && isEliteWave)
            {
                enemy.MakeElite();
            }

            enemy.onDie += OnEnemyDie;
        }
    }

    private void OnEnemyDie()
    {
        if (enemy != null)
        {
            enemy.onDie -= OnEnemyDie;
            enemy = null;
        }
    }
}
