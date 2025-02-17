using UnityEngine;
using System.Collections;

public class EnemySpawner : MonoBehaviour
{
    public GameObject enemyPrefab;
    public Transform quadraticSpawnPoint, cubicSpawnPoint;
    public Transform endPoint;
    public float spawnInterval = 1f;

    public void StartWave(int enemyCount)
    {
        StartCoroutine(SpawnEnemies(enemyCount));
    }

    IEnumerator SpawnEnemies(int count)
    {
        for (int i = 0; i < count; i++)
        {
            bool useCubic = i % 2 == 0;
            Vector3 spawnPos = useCubic ? cubicSpawnPoint.position : quadraticSpawnPoint.position;
            GameObject enemy = Instantiate(enemyPrefab, spawnPos, Quaternion.identity);
            enemy.GetComponent<Enemy>().Initialize(useCubic, endPoint.position);
            yield return new WaitForSeconds(spawnInterval);
        }
    }
}
