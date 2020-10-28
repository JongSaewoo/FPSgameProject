using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionBarrel : InteractionObject
{
    [Header("Explosion Barrel")]
    [SerializeField]
    private GameObject explosionPrefab;
    [SerializeField]
    private float explosionDelayTime = 0.3f;
    [SerializeField]
    private float explosionRadius = 10.0f;
    [SerializeField]
    private float explosionForce = 1000.0f;
    private bool isExplode = false;

    public override void TakeDamage(int damage)
    {
        currentHP -= damage;

        if (currentHP <= 0 && isExplode == false)
        {
            StartCoroutine(ExplodeBarrel());
        }
    }

    IEnumerator ExplodeBarrel()
    {
        yield return new WaitForSeconds(explosionDelayTime);

        // 근처 배럴이 터지고 난 후 다시 현재 배럴을 터트리려고 할 때
        // stackOverflow 방지
        isExplode = true;

        // 폭발 이펙트
        Instantiate(explosionPrefab, transform.position, transform.rotation);

        // 폭발 범위에 있는 모든 오브젝트의 Collider정보를 감지하고 폭발 효과 처리
        Collider[] colliders = Physics.OverlapSphere(transform.position, explosionRadius);
        foreach (Collider hit in colliders)
        {
            // 폭발 범위에 존재하는 오브젝트가 상호작용 오브젝트일때의 처리
            InteractionObject interaction = hit.GetComponent<InteractionObject>();
            if (interaction != null)
            {
                interaction.TakeDamage(10000);
                continue;
            }


            // 폭발 범위에 존재하는 오브젝트가 좀비일때의 처리
            EnemyController enemy = hit.GetComponent<EnemyController>();
            if (enemy != null)
            {
                enemy.TakeDamage(10000);
                continue;
            }

            // 중력을 가지고 있는 오브젝트라면 폭발영향을 받아 밀려나도록 처리
            Rigidbody rigidbody = hit.GetComponent<Rigidbody>();
            if (rigidbody != null)
            {
                rigidbody.AddExplosionForce(explosionForce, transform.position, explosionRadius);
            }
        }

        // 배럴 오브젝트 삭제
        Destroy(this.gameObject);
    }
}
