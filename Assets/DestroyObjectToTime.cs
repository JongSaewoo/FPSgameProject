using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyObjectToTime : MonoBehaviour
{
    [SerializeField]
    private float destroyTime;

    private void Awake()
    {
        StartCoroutine("DestroyObject");
    }

    // 지정된 시간이 지난 후 탄피 삭제
    IEnumerator DestroyObject()
    {
        yield return new WaitForSeconds(destroyTime);

        Destroy(gameObject);
    }
}
