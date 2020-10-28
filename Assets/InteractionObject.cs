using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class InteractionObject : MonoBehaviour
{
    [Header("Interaction Object")]
    [SerializeField]
    protected int maxHP;
    [SerializeField]
    protected int currentHP;

    private void Awake()
    {
        currentHP = maxHP;
    }

    public abstract void TakeDamage(int damage);
}
