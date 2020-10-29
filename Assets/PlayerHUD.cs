using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerHUD : MonoBehaviour
{
    [Header("Components")]
    [SerializeField]
    private WeaponAssaultRifle weapon;
    [SerializeField]
    private Status status;
    [SerializeField]
    private EnemySpawner enemySpawner;

    [Header("Ammo UI")]
    [SerializeField]
    private TextMeshProUGUI textAmmo;

    [Header("HP & BloodScreen UI")]
    [SerializeField]
    private TextMeshProUGUI textHP;
    [SerializeField]
    private Image imageBloodScreen;
    [SerializeField]
    private AnimationCurve bloodScreenCurve;

    [Header("Enemy Informtion UI")]
    [SerializeField]
    private TextMeshProUGUI textEnemyCount;

    private void Awake()
    {
        weapon.onAmmoEvent.AddListener(AmmoHUDUpdate);
        status.onHPEvent.AddListener(HPHUDUpdate);
        enemySpawner.onEnemyCountEvent.AddListener(EnemyCountUpdate);
    }

    private void AmmoHUDUpdate(int currentAmmo, int maxAmmo)
    {
        textAmmo.text = currentAmmo + " / " + maxAmmo;
    }

    private void HPHUDUpdate(int previousHP, int currentHP)
    {
        textHP.text = "HP : " + currentHP;

        if (previousHP <= currentHP) return;
        // 기존에 체력이 감소되면 Blood UI가 활성화 되는데
        // 회복했을 땐 처리하지 않도록 함.

        if(previousHP - currentHP > 0)
        // 중간에 StopCoroutine으로 중단할 필요가 있는 
        // 코루틴의 경우 문자열로 작동시키고 종료
        {
            StopCoroutine("UpdateBloodScreen");
            StartCoroutine("UpdateBloodScreen", 1);
        }
    }

    IEnumerator UpdateBloodScreen(float maxViewTime)
    {
        float currentTime = 0;
        float percent = 0;

        while(percent<1)
        {
            percent = currentTime / maxViewTime;
            currentTime += Time.deltaTime;

            Color color = imageBloodScreen.color;
            color.a = Mathf.Lerp(1, 0, bloodScreenCurve.Evaluate(percent));
            imageBloodScreen.color = color;

            yield return null;
        }
    }

    private void EnemyCountUpdate(int currentEnemy, int maxEnemy)
    {
        textEnemyCount.text = "Enemy " + currentEnemy + "/" + maxEnemy;
    }
}
