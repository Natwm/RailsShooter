using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponsBehaviours : MonoBehaviour
{
    #region PARAM
    [Space]
    [Header("INFORMATIONS")]
    [SerializeField] private int m_Damage;
    [SerializeField] private LayerMask bulletCollisionLayerMask;

    [Space]
    [Header("MAGAZINE")]
    [SerializeField] private int m_TotalNumberOfBullets;
    [SerializeField] private int m_NumberOfBulletsPerMagazine;
    [SerializeField] private int m_ReloadTime;
    private int currentNumberOfBullets;

    [Space]
    [Header("PROJECTILE")]
    [SerializeField] private GameObject m_projectilePrefab;
    [SerializeField] private float projectileSpeed;

    [Space]
    [Header("RAYCAST SHOOT")]
    [SerializeField] private float raycasRadius;

    [Space]
    [Header("FLAG")]
    [SerializeField] private bool AutoReload = false;
    [SerializeField] private bool UseProjectile = true;
    private bool IReload;
    #endregion


    public virtual void Equip()
    {

    }

    public virtual void UnEquip()
    {
        
    }

    public virtual void Shoot()
    {
        if(currentNumberOfBullets > 0)
        {
            print("Shoot");
            Vector3 cursorPosition =  WeaponManager.instance.CursorGO.GetComponent<RectTransform>().anchoredPosition;
            Vector3 mousePosition = Camera.main.ScreenToWorldPoint(cursorPosition + Camera.main.transform.forward);
            Vector3 direction = mousePosition - Camera.main.transform.position;

            currentNumberOfBullets --;

            RaycastHit hit;            
            if(UseProjectile == true)
            {
                Debug.DrawLine(Camera.main.transform.position, Camera.main.transform.position + direction * 100, Color.green, 10);
                Debug.LogWarning("No projectile yet");
            }
            else
            {
                if(Physics.SphereCast(Camera.main.transform.position, raycasRadius, direction, out hit, Mathf.Infinity, bulletCollisionLayerMask))
                {
                    if(hit.transform.gameObject.layer == 8)
                    {
                        Debug.Log("Ennemy touched");
                        hit.transform.GetComponent<BodyPartBehaviours>().GetDamage(m_Damage);
                    }
                    if(hit.transform.gameObject.layer == 9)
                    {
                        Debug.Log("Obstacle touched");
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
                Debug.Log("Reload");
                Reload();
        }
    }

    public virtual void Reload()
    {
        IReload = true;
        Timer t = new Timer(m_ReloadTime, RefillMagazine);
        t.Play();
    }

    public virtual void RefillMagazine()
    {
        IReload = false;
        currentNumberOfBullets = Mathf.Min(m_TotalNumberOfBullets, m_NumberOfBulletsPerMagazine);
        m_TotalNumberOfBullets -= currentNumberOfBullets;
    }

    void Start()
    {
        RefillMagazine();
    }
}
