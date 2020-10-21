using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Status : MonoBehaviour
{
    [Header("Walk & Run Speed")]
    [SerializeField]
    private float walkSpeed;
    [SerializeField]
    private float runSpeed;

    [Header("Health")]
    [SerializeField]
    private int maxHP = 100;
    private int currentHP;

    [HideInInspector]
    public HPEvent onHPEvent = new HPEvent();

    public float WalkSpeed
    {
        get { return walkSpeed; }
    }

    public float RunSpeed
    {
        get { return runSpeed; }
    }

    private void Awake()
    {
        currentHP = maxHP;
    }

    public bool DecreaseHP(int decreaseHP)
    {
        int previousHP = currentHP;

        if(currentHP - decreaseHP > 0)
        {
            currentHP -= decreaseHP;
        }
        else
        {
            currentHP = 0;
            return true;
        }

        onHPEvent.Invoke(previousHP, currentHP);

        return false;
    }
}

[System.Serializable]
public class HPEvent : UnityEngine.Events.UnityEvent<int, int> { }