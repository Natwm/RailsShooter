using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponsBehaviours : MonoBehaviour
{
    #region PARAM
    [Space]
    [Header("INFORMATIONS")]
    [SerializeField] protected int m_Damage;
    [SerializeField] protected float m_FireRate;
    [SerializeField] protected LayerMask bulletCollisionLayerMask;

    [Space]
    [Header("MAGAZINE")]
    [SerializeField] private int m_TotalNumberOfBullets;
    [SerializeField] private int m_NumberOfBulletsPerMagazine;
    [SerializeField] private int m_ReloadTime;
    protected int currentNumberOfBullets;

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


    [Space]
    [Header("FLAG")]
    [SerializeField] protected bool AutoReload = false;
    [SerializeField] protected bool UseProjectile = true;

    [Space]
    [Header("OTHER")]
    [SerializeField] protected HealthManager myHealthManager;
    protected Timer fireRateTimer, reloadTimer;

    #endregion

    


    public virtual void Equip()
    {

    }

    public virtual void UnEquip()
    {
        
    }

    public virtual void Shoot()
    {
        if(currentNumberOfBullets > 0 && fireRateTimer.IsStarted() == false)
        {
            Vector3 cursorPosition =  WeaponManager.instance.CursorGO.GetComponent<RectTransform>().anchoredPosition;
            Vector3 mousePosition = Camera.main.ScreenToWorldPoint(cursorPosition + Camera.main.transform.forward);
            Vector3 direction = mousePosition - Camera.main.transform.position;

            currentNumberOfBullets --;
            shootEffect.start();
            fireRateTimer.ResetPlay();
            
            RaycastHit hit;            
            if(UseProjectile == true)
            {
                GameObject projectile = BulletPool.instance.GetBullet(m_projectilePrefab);
                projectile.transform.position = Camera.main.transform.position;
                projectile.transform.rotation = Quaternion.Euler(direction);
                projectile.GetComponent<Rigidbody>().velocity = direction.normalized * projectileSpeed;
                projectile.GetComponent<BulletsBehaviours>().Launch(m_Damage, myHealthManager);
            }
            else
            {
                if(Physics.SphereCast(Camera.main.transform.position, raycasRadius, direction, out hit, Mathf.Infinity, bulletCollisionLayerMask))
                {
                    if(hit.transform.gameObject.layer == 8)
                    {
                        hit.transform.GetComponent<BodyPartBehaviours>().GetDamage(m_Damage, myHealthManager);
                    }
                    if(hit.transform.gameObject.layer == 9)
                    {
                        hit.transform.GetComponent<BodyPartBehaviours>()?.GetDamage(m_Damage, myHealthManager);
                    }
                }
                else
                {
                    // doesn't touch
                }
            }

        }
        
        
        if(currentNumberOfBullets == 0 && AutoReload == true)
        {
            Reload();
        }
    }

    public virtual void Reload()
    {
        if(reloadTimer.IsStarted() == false)
        {
            reloadTimer.ResetPlay();
            reloadEffect.start();   
        }
    }

    public virtual void RefillMagazine()
    {
        currentNumberOfBullets = Mathf.Min(m_TotalNumberOfBullets, m_NumberOfBulletsPerMagazine);
        m_TotalNumberOfBullets -= currentNumberOfBullets;
        // reloadEffect.start();
    }

    void Start()
    {
        RefillMagazine();
        fireRateTimer = new Timer(m_FireRate);
        fireRateTimer.Play();
        reloadTimer = new Timer(m_ReloadTime, RefillMagazine);

        shootEffect = FMODUnity.RuntimeManager.CreateInstance(shootSound);
        FMODUnity.RuntimeManager.AttachInstanceToGameObject(shootEffect, GetComponent<Transform>(), GetComponentInParent<Rigidbody>());

        reloadEffect = FMODUnity.RuntimeManager.CreateInstance(reloadSound);
        FMODUnity.RuntimeManager.AttachInstanceToGameObject(reloadEffect, GetComponent<Transform>(), GetComponentInParent<Rigidbody>());

    }
}
