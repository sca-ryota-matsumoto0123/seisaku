using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BulletScript : MonoBehaviour
{
    [SerializeField, Min(1)]
    int Maxhp = 100;
    [SerializeField]
    Transform bulletSpawn = null;
    [SerializeField, Min(1)]
    int damage = 1;
    [SerializeField, Min(1)]
    int maxAmmo = 30;
    [SerializeField, Min(1)]
    float maxRange = 30;
    [SerializeField]
    LayerMask hitLayers = 0;
    [SerializeField, Min(0.01f)]
    float fireInterval = 0.1f;
    [SerializeField]
    ParticleSystem muzzleFlashParticle = null;
    [SerializeField]
    GameObject bulletHitEffectPrefab = null;
    [SerializeField]
    float resupplyInterval = 5;
    [SerializeField]
    Image ammoGauge = null;
    [SerializeField]
    Image resupplyGauge = null;
    [SerializeField]
    Image HpGauge = null;
    [SerializeField]
    GameManager gameManager = null;

    bool fireTimerIsActive = false;
    RaycastHit hit;
    WaitForSeconds fireIntervalWait;

    int currentAmmo = 0;
    int minHp = 0;
    bool resupplyTimerIsActive = false;
    // Start is called before the first frame update

    public int CurrentAmmo
    {
        set 
        {
            currentAmmo = Mathf.Clamp(value, 0 , maxAmmo); 
        
            float scaleX = currentAmmo / (float)maxAmmo;
            ammoGauge.rectTransform.localScale = new Vector3(scaleX, 1, 1);
        }

        get
        {
            return currentAmmo;
        }
    }

    public int MinHp
    {
        set
        {
            minHp = Mathf.Clamp(value, 0, Maxhp);

            float scaleX = minHp / (float)Maxhp;
            HpGauge.rectTransform.localScale = new Vector3(scaleX, 1, 1);
        }
        get
        {
            return minHp;
        }
    }

    void Start()
    {
        fireIntervalWait = new WaitForSeconds(fireInterval);
        MinHp = Maxhp;
        CurrentAmmo = maxAmmo;
        print(name);
        print(tag);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButton("Fire1"))
        {
            Fire();
        }
        if (Input.GetKeyDown(KeyCode.R) && !resupplyTimerIsActive && CurrentAmmo <= 0)
        {
            StartCoroutine(nameof(Reload));
        }
        if(minHp <= 0)
        {
            gameManager.GameOver();
        }
        
    }

    void Fire()
    {
        if (fireTimerIsActive || CurrentAmmo <= 0)
        {
            return;
        }

        muzzleFlashParticle.Play();

        if(Physics.Raycast(bulletSpawn.position, bulletSpawn.forward, out hit, maxRange,hitLayers, QueryTriggerInteraction.Ignore))
        {
            BulletHit();
        }
        
        StartCoroutine(nameof(FireTimer));

        CurrentAmmo--;
    }

    void BulletHit()
    {
        Debug.Log("弾が「"+hit.collider.gameObject.name+"」にヒットしました。");

        Instantiate(bulletHitEffectPrefab, hit.point, Quaternion.LookRotation(hit.normal));

        if (hit.collider.gameObject.CompareTag("Enemy"))
        {
            EnemyControll enemy = hit.collider.gameObject.GetComponent<EnemyControll>();
            enemy.Damage(damage);
        }
    }

    IEnumerator Reload()
    {
        resupplyTimerIsActive = true;

        float timer = 0;

        while(timer < resupplyInterval)
        {
            resupplyGauge.rectTransform.localScale = new Vector3(timer / resupplyInterval, 1, 1);
            timer += Time.deltaTime;

            yield return null;
        }

        CurrentAmmo = maxAmmo;

        resupplyTimerIsActive = false;

    }

    IEnumerator FireTimer()
    {
        fireTimerIsActive = true;
        yield return fireIntervalWait;
        fireTimerIsActive = false;
    }

    public void StopResupplyTimer()
    {
        StopCoroutine(nameof(Reload));
        resupplyTimerIsActive = false;
    }
}
