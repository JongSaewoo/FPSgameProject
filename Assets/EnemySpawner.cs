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
    private GameObject enemyHeadShotPrefab;
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
            GameObject cloneEnemy = SelectEnemyType(index);
            // GameObject cloneEnemy = Instantiate(enemyPrefab, spawnPoints[index].position, Quaternion.identity);

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

    private GameObject SelectEnemyType(int index)
    {
        GameObject cloneEnemy;

        int enemyType = Random.Range(0, 100);
        int enemyMode = Random.Range(0, 100);

        if(enemyType < 70)
        {
            // 좀비타입 : 일반좀비(enemy) 등장 확률 70%, HP : 100, 타격 범위 : 전체
            //            헤드샷좀비(enemyHeadShot) 등장 확률 30%, HP : 50, 타격 범위 : 머리만
            cloneEnemy = Instantiate(enemyPrefab, spawnPoints[index].position, Quaternion.identity);
        }
        else
        {
            // 좀비모드 : 일반상태 등장 확률 70%, 이동속도 1, 프리펩 색 : 기본
            //            버서커(광분)상태 등장 확률 20%, 이동속도 3, 프리펩 색 : 전체 빨간색
            cloneEnemy = Instantiate(enemyHeadShotPrefab, spawnPoints[index].position, Quaternion.identity);
        }

        if(enemyMode >= 80)
        {
            cloneEnemy.GetComponentInChildren<SkinnedMeshRenderer>().material.color = Color.red;
            cloneEnemy.GetComponent<Animator>().speed = 3;
            cloneEnemy.GetComponent<UnityEngine.AI.NavMeshAgent>().speed = 3;
        }

        return cloneEnemy;
    }
}

[System.Serializable]
public class EnemyCountEvent : UnityEngine.Events.UnityEvent<int, int> { }