using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponsBehaviours : MonoBehaviour
{
    #region PARAM
    [Space]
    [Header("INFORMATIONS")]
    [SerializeField] protected int m_Damage;
    [Min (.1f)]
     public float m_FireRate = 1f;
    [SerializeField] protected LayerMask bulletCollisionLayerMask;

    [Space]
    [Header("MAGAZINE")]
    [SerializeField] private int m_TotalNumberOfBullets;
    [SerializeField] private int m_NumberOfBulletsPerMagazine;
    [SerializeField] private float m_ReloadTime;
    public int currentNumberOfBullets;

    [Space]
    [Header("PROJECTILE")]
    [SerializeField] protected GameObject m_projectilePrefab;
    [SerializeField] protected float projectileSpeed;

    [Space]
    [Header("RAYCAST SHOOT")]
    [SerializeField] protected float raycasRadius;

    [Space]
    [Header("SOUND")]
    protected FMOD.Studio.EventInstance shootEffect;
    [FMODUnity.EventRef] [SerializeField] private string shootSound;
    protected FMOD.Studio.EventInstance reloadEffect;
    [FMODUnity.EventRef] [SerializeField] private string reloadSound;
    protected FMOD.Studio.EventInstance buckshotEffect;
    [FMODUnity.EventRef] [SerializeField] private string buckshotSound;
    protected FMOD.Studio.EventInstance emptyMagasinSoundEffect;
    [FMODUnity.EventRef] [SerializeField] private string emptyMagasinSound;


    [Space]
    [Header("FLAG")]
    [SerializeField] protected bool AutoReload = false;
    [SerializeField] protected bool UseProjectile = true;
    [HideInInspector] public bool isReloading = false;

    [Space]
    [Header("OTHER")]
    [SerializeField] protected HealthManager myHealthManager;
    protected Timer fireRateTimer;

    public int TotalNumberOfBullets { get => m_TotalNumberOfBullets; set => m_TotalNumberOfBullets = value; }
    

    #endregion



    //Vector3 shootTargetDebug;

    public virtual void Equip()
    {

    }

    public virtual void UnEquip()
    {
        
    }

    public virtual void Shoot(Animator anim, Vector3 direction)
    {
        if(currentNumberOfBullets > 0 && fireRateTimer.IsStarted() == false && isReloading == false)
        {
            anim.SetTrigger("Trigger_Shoot");

            currentNumberOfBullets --;

            GameManager.instance.UpdateAmountOfBulltes(currentNumberOfBullets);

            GameObject projectile = null;
            //shootEffect.start();
            fireRateTimer.ResetPlay();
            
            RaycastHit hit;

            Debug.DrawRay(Camera.main.transform.position, direction * 100, Color.green, 10f);

            if(Physics.Raycast(Camera.main.transform.position, direction, out hit, Mathf.Infinity, bulletCollisionLayerMask))
            {
               /* shootTargetDebug = hit.point;
                Debug.Break();*/

                if (UseProjectile == true)
                {
                    direction = hit.point - transform.position;
                    projectile = BulletPool.instance.GetBullet(m_projectilePrefab);
                    projectile.transform.position = transform.position;
                    projectile.transform.rotation = Quaternion.Euler(direction);
                    projectile.GetComponent<Rigidbody>().velocity = direction.normalized * projectileSpeed;
                    projectile.GetComponent<BulletsBehaviours>().Launch(m_Damage, myHealthManager);
                }
                else
                {
                    if(hit.transform.gameObject.layer == 8 || hit.transform.gameObject.layer == 9)
                    {
                        hit.transform.GetComponent<BodyPartBehaviours>().GetDamage(m_Damage, myHealthManager,projectile);
                    }
                }
            }
            else
            {
                projectile = BulletPool.instance.GetBullet(m_projectilePrefab);
                projectile.transform.position = transform.position;
                projectile.transform.rotation = transform.rotation;
                projectile.GetComponent<Rigidbody>().velocity = direction.normalized * projectileSpeed;
                projectile.GetComponent<BulletsBehaviours>().Launch(m_Damage, myHealthManager);
            }

        }
        
        
        if(currentNumberOfBullets == 0 && AutoReload == true)
        {
            WeaponManager.instance.WeaponAnimator.SetTrigger("Trigger_Reload");
            Reload();
        }
    }

    public virtual void Reload()
    {
        if(!isReloading && currentNumberOfBullets < m_NumberOfBulletsPerMagazine)
        {
            WeaponManager.instance.WeaponAnimator.SetTrigger("Trigger_Reload");
            GameManager.instance.UpdateAmountOfBulltes(currentNumberOfBullets);
        }
    }

    public virtual void RefillMagazine()
    {
        m_TotalNumberOfBullets -= (m_NumberOfBulletsPerMagazine - currentNumberOfBullets);
        currentNumberOfBullets = Mathf.Min(m_TotalNumberOfBullets, m_NumberOfBulletsPerMagazine);
        GameManager.instance.UpdateAmountOfBulltes(currentNumberOfBullets);
        // reloadEffect.start();
    }

    void Start()
    {
        RefillMagazine();
        fireRateTimer = new Timer(m_FireRate);
        fireRateTimer.Play();

        shootEffect = FMODUnity.RuntimeManager.CreateInstance(shootSound);
        FMODUnity.RuntimeManager.AttachInstanceToGameObject(shootEffect, GetComponent<Transform>(), GetComponentInParent<Rigidbody>());

        reloadEffect = FMODUnity.RuntimeManager.CreateInstance(reloadSound);
        FMODUnity.RuntimeManager.AttachInstanceToGameObject(reloadEffect, GetComponent<Transform>(), GetComponentInParent<Rigidbody>());

        emptyMagasinSoundEffect = FMODUnity.RuntimeManager.CreateInstance(emptyMagasinSound);
        FMODUnity.RuntimeManager.AttachInstanceToGameObject(emptyMagasinSoundEffect, GetComponent<Transform>(), GetComponentInParent<Rigidbody>());
    }

    private void OnDrawGizmos()
    {
       // Gizmos.DrawWireSphere(shootTargetDebug, 0.5f);
    }
}
