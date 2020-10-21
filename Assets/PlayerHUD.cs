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

    private void Awake()
    {
        weapon.onAmmoEvent.AddListener(AmmoHUDUpdate);
        status.onHPEvent.AddListener(HPHUDUpdate);
    }

    private void AmmoHUDUpdate(int currentAmmo, int maxAmmo)
    {
        textAmmo.text = currentAmmo + " / " + maxAmmo;
    }

    private void HPHUDUpdate(int previousHP, int currentHP)
    {
        textHP.text = "HP : " + currentHP;

        if(previousHP - currentHP > 0)
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
}
