using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestructibleObject : InteractionObject
{
    [Header("Destructible Object")]
    [SerializeField]
    private GameObject destructiblePrefab;

    public override void TakeDamage(int damage)
    {
        currentHP -= damage;

        if(currentHP <= 0)
        {
            Instantiate(destructiblePrefab, transform.position, transform.rotation);

            Destroy(this.gameObject);
        }
    }
}
