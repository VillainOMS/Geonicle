using UnityEngine;

public class SpawnPoint : MonoBehaviour
{
    [SerializeField] private Enemy enemyPrefab; // ������ �����, �������� ����� ��������
    private Enemy enemy;

    public Enemy Enemy { get => enemy; }

    public void SpawnEnemy()
    {
        enemy = Instantiate(enemyPrefab,transform.position,Quaternion.identity);
        enemy.onDie += OnEnemyDie;
    }

    private void OnEnemyDie()
    {
        enemy.onDie -= OnEnemyDie;
        enemy = null;
    }
}
