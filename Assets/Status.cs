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

    public void IncreaseHP(int increaseHP)
    {
        int previousHP = currentHP;

        currentHP += increaseHP;
        
        if(currentHP > maxHP)
        // 회복량이 최대체력을 넘어서지 못하도록
        {
            currentHP = maxHP;
        }

        // onHPEvent에 등록된 모든 객체의 메소드 호출
        onHPEvent.Invoke(previousHP, currentHP);
    }
}

[System.Serializable]
public class HPEvent : UnityEngine.Events.UnityEvent<int, int> { }