using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponManager : MonoBehaviour
{
    #region STATIC
    public static WeaponManager instance;
    #endregion

    #region PARAM
    [SerializeField] private List<WeaponsBehaviours> m_ListOfWeapon = new List<WeaponsBehaviours>();
    #endregion

    private WeaponsBehaviours currentWeapon;

    private void Awake()
    {
        if(instance != null)
            Debug.LogWarning("Multiple instance of same Singleton : WeaponManager");
        instance = this;
    }

    public void SwapWeaponUp()
    {
        int index = m_ListOfWeapon.IndexOf(currentWeapon);
        index ++;
        
        if(index == m_ListOfWeapon.Count)
            index = 0;

        SwapWeapon(m_ListOfWeapon[index]);
    }

    public void SwapWeaponDown()
    {
        int index = m_ListOfWeapon.IndexOf(currentWeapon);
        index --;
        
        if(index == -1)
            index = m_ListOfWeapon.Count - 1;

        SwapWeapon(m_ListOfWeapon[index]);
    }

    private void SwapWeapon(WeaponsBehaviours weapon)
    {
        currentWeapon.UnEquip();
        weapon.Equip();
        currentWeapon = weapon;
    }

    public WeaponsBehaviours GetCurrentWeapon()
    {
        return currentWeapon;
    }
}
