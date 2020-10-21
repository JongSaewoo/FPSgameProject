using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField]
    private Transform target;
    [SerializeField]
    private Transform[] spawnPoints;
    [SerializeField]
    private GameObject enemyPrefab;
    [SerializeField]
    private float spawnTime = 0.5f;
    [SerializeField]
    private int maxEnemyCount = 50;

    private void Awake()
    {
        StartCoroutine("SpawnEnemy");
    }

    IEnumerator SpawnEnemy()
    {
        int enemyCount = 0;

        while (enemyCount < maxEnemyCount)
        {
            int index = Random.Range(0, spawnPoints.Length);
            GameObject cloneEnemy = Instantiate(enemyPrefab, spawnPoints[index].position, Quaternion.identity);

            cloneEnemy.GetComponent<EnemyController>().Setup(target);

            enemyCount++;

            yield return new WaitForSeconds(spawnTime);
        }
    }
}
