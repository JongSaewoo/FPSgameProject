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
    private Transform target;

    public void Setup(Transform target)
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        status = GetComponent<Status>();
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

            Destroy(this.gameObject);
        }
    }
}
