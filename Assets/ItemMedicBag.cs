using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemMedicBag : Item
{
    [SerializeField]
    private int increaseHP = 30;
    
    public override void Use(GameObject entity)
    {
        entity.GetComponent<Status>().IncreaseHP(increaseHP);

        Destroy(this.gameObject);
        // Use메소드가 호출되면 플레이어 status에서 
        // (increaseHP=30)만큼 회복시키고, 아이템은 Destroy된다.
    }
}
