using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCollider : MonoBehaviour
{
    private void OnTriggerEnter(Collider collider)
    {
        if(collider.tag == "Item")
        // 부딪힌 오브젝트의 tag가 Item이면 부딪힌 오브젝트의
        // Use()메소드(item.cs -> itemMedicBag.cs ->)를 호출
        {
            collider.GetComponent<Item>().Use(transform.parent.gameObject);
        }
    }
}
