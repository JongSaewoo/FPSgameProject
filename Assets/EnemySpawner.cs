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
    private int arriveEnemyCount;

    [HideInInspector]
    public EnemyCountEvent onEnemyCountEvent = new EnemyCountEvent();

    private void Awake()
    {
        arriveEnemyCount = maxEnemyCount;

        onEnemyCountEvent.Invoke(arriveEnemyCount, maxEnemyCount);

        StartCoroutine("SpawnEnemy");
    }

    IEnumerator SpawnEnemy()
    {
        int enemyCount = 0;

        while (enemyCount < maxEnemyCount)
        {
            int index = Random.Range(0, spawnPoints.Length);
            GameObject cloneEnemy = Instantiate(enemyPrefab, spawnPoints[index].position, Quaternion.identity);

            cloneEnemy.GetComponent<EnemyController>().Setup(this, target);

            enemyCount++;

            yield return new WaitForSeconds(spawnTime);
        }
    }

    public void DieEnemy()
    {
        arriveEnemyCount--;

        onEnemyCountEvent.Invoke(arriveEnemyCount, maxEnemyCount);

        if(arriveEnemyCount == 0)
        {
            Debug.Log("Stage Clear");
        }
    }
}

[System.Serializable]
public class EnemyCountEvent : UnityEngine.Events.UnityEvent<int, int> { }