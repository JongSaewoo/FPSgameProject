using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponAssaultRifle : MonoBehaviour
{
    public Animator Animator
    {
        private set; get;
    }
    private AudioSource audioSource;

    [Header("Status")]
    [SerializeField]
    private int damage = 50;

    [Header("Fire Effects")]
    [SerializeField]
    private GameObject muzzleFlashEffect;
    [SerializeField]
    private GameObject casingPrefab;
    [SerializeField]
    private GameObject impactPrefab;

    [Header("SpawnPoints")]
    [SerializeField]
    private Transform casingSpawnPoint;
    [SerializeField]
    private Transform bulletSpawnPoint;

    [Header("Sounds")]
    [SerializeField]
    private AudioClip audioClipFire;
    [SerializeField]
    private AudioClip audioClipReload;

    [Space]
    [SerializeField]
    private WeaponSetting weaponSetting;

    [HideInInspector]
    public AmmoEvent onAmmoEvent = new AmmoEvent();

    private bool isAttack = false;
    private bool isAttackStop = false;
    private bool isReload = false;

    private int currentAmmo;    // 탄수 설정할 변수

    private void OnEnable()
    {
        onAmmoEvent.Invoke(currentAmmo, weaponSetting.maxAmmo);
    }

    private void Awake()
    {
        Animator = this.GetComponent<Animator>();
        audioSource = this.GetComponent<AudioSource>();

        muzzleFlashEffect.SetActive(false);

        currentAmmo = weaponSetting.maxAmmo;
    }

    public void StartAttack()
    {
        if (isAttack == false)
        {
            StartCoroutine(TryAttack());
        }
    }

    public void StopAttack()
    {
        isAttackStop = true;
    }

    public void StartReload()
    {
        if (isReload == false)
        {
            StopAttack();
            StartCoroutine(TryReload());
        }
    }

    IEnumerator TryAttack()
    {
        isAttack = true;

        while (!isAttackStop)
        {
            if (Animator.GetFloat("movementSpeed") > 0.5f)
            {
                break;
            }

            if (currentAmmo <= 0)
            {
                StartReload();
                break;
            }
            currentAmmo--;

            onAmmoEvent.Invoke(currentAmmo, weaponSetting.maxAmmo);

            Animator.ResetTrigger("onJump");

            Animator.Play("Fire");
            StartCoroutine(OnFireEffects());
            PlaySound(audioClipFire);
            SpawnCasing();

            TwoStepRayCast();

            yield return new WaitForSeconds(weaponSetting.fireRate);
        }

        isAttack = false;
        isAttackStop = false;
    }

    IEnumerator OnFireEffects()
    {
        muzzleFlashEffect.SetActive(true);

        yield return new WaitForSeconds(weaponSetting.fireRate * 0.3f);

        muzzleFlashEffect.SetActive(false);
    }

    private void PlaySound(AudioClip clip)
    {
        audioSource.Stop();
        audioSource.clip = clip;
        audioSource.Play();
    }

    private void SpawnCasing()
    {
        GameObject cloneCasing = Instantiate(casingPrefab, casingSpawnPoint.position, Random.rotation);
        cloneCasing.GetComponent<Casing>().Setup(transform.right);
    }

    private void TwoStepRayCast()
    {
        Ray ray;
        RaycastHit hit;
        Vector3 targetPoint = Vector3.zero;

        ray = Camera.main.ViewportPointToRay(Vector2.one * 0.5f);
        if (Physics.Raycast(ray, out hit, weaponSetting.fireDistance))
        {
            targetPoint = hit.point;
        }
        else
        {
            targetPoint = ray.origin + ray.direction * weaponSetting.fireDistance;
        }
        Debug.DrawRay(ray.origin, ray.direction * weaponSetting.fireDistance, Color.red);

        Vector3 attackDirection = (targetPoint - bulletSpawnPoint.position).normalized;
        if (Physics.Raycast(bulletSpawnPoint.position, attackDirection, out hit, weaponSetting.fireDistance))
        {
            if(hit.transform.tag.Equals("Enemy"))
            {
                hit.transform.GetComponent<EnemyController>().TakeDamage(damage, hit);
            }
            else
            {
                Instantiate(impactPrefab, hit.point, Quaternion.LookRotation(hit.normal));
            }
        }
        Debug.DrawRay(bulletSpawnPoint.position, attackDirection*weaponSetting.fireDistance, Color.blue);
    }

    IEnumerator TryReload()
    {
        isReload = true;

        Animator.SetTrigger("onReload");
        PlaySound(audioClipReload);

        while (true)
        {
            if (audioSource.isPlaying == false && Animator.GetCurrentAnimatorStateInfo(0).IsName("Movement"))
            {
                break;
            }
            yield return null;
        }

        isReload = false;
        currentAmmo = weaponSetting.maxAmmo;

        onAmmoEvent.Invoke(currentAmmo, weaponSetting.maxAmmo);
    }
}

[System.Serializable]
public class AmmoEvent : UnityEngine.Events.UnityEvent<int, int> { }
