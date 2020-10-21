using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour
{
    [SerializeField]
    private GameObject bloodImpact;
    [SerializeField]
    private int damage = 5;
    private NavMeshAgent navMeshAgent;
    private Status status;
    public Transform target;
    public EnemySpawner enemySpawner;

    public void Setup(EnemySpawner enemySpawner, Transform target)
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        status = GetComponent<Status>();
        this.enemySpawner = enemySpawner;
        this.target = target;

        StartCoroutine("UpdateMove");
    }

    IEnumerator UpdateMove()
    {
        while(true)
        {
            Vector3 moveDirection = target.position - transform.position;
            moveDirection.y = 0;

            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(moveDirection), 0.01f);

            navMeshAgent.SetDestination(target.position);

            yield return new WaitForSeconds(1.0f);
        }
    }

    private void OnTriggerEnter(Collider collider)
    {
        if(collider.transform == target)
        {
            collider.GetComponent<PlayerController>().TakeDamage(damage);

            enemySpawner.DieEnemy();
            Destroy(this.gameObject);
        }
    }

    public void TakeDamage(int damage, RaycastHit hit)
    {
        Instantiate(bloodImpact, hit.point, Quaternion.LookRotation(hit.normal));

        bool isDie = status.DecreaseHP(damage);

        if(isDie == true)
        {
            StopCoroutine("UpdateMove");

            enemySpawner.DieEnemy();
            Destroy(this.gameObject);
        }
    }
}
