using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponsBehaviours : MonoBehaviour
{
    #region PARAM
    [Space]
    [Header("INFORMATIONS")]
    [SerializeField] private int m_Damage;
    [SerializeField] private int m_NumberOfBullets;

    [Space]
    [Header("RELOAD")]
    [SerializeField] private int m_ReloadTime;

    [Space]
    [Header("FLAG")]
    [SerializeField] private bool IReload = false;
    #endregion


    public virtual void Equip()
    {

    }

    public virtual void UnEquip()
    {
        
    }

}
