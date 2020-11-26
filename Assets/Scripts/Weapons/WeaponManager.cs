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
    [SerializeField] public GameObject CursorGO;

    [SerializeField] private float weaponSwapDuration;

    #endregion

    private Timer weaponSwapTimer;
    private WeaponsBehaviours currentWeapon;

    private void Awake()
    {
        if(instance != null)
            Debug.LogWarning("Multiple instance of same Singleton : WeaponManager");
        instance = this;

        currentWeapon = m_ListOfWeapon[0];
        weaponSwapTimer = new Timer(weaponSwapDuration);
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
        weaponSwapTimer.ResetPlay();
        currentWeapon.UnEquip();
        weapon.Equip();
        currentWeapon = weapon;

        Debug.Log("Swap Weapon (" + currentWeapon.name + ") : Play Sound here");
    }

    public WeaponsBehaviours GetCurrentWeapon()
    {
        return currentWeapon;
    }

    public void Shoot()
    {
        if(weaponSwapTimer.IsStarted() == false)
        {
            currentWeapon.Shoot();
        }
    }

    public void Reload()
    {
        currentWeapon.Reload();
    }

    void Update(){
        if (Input.GetMouseButton(0))
            Shoot();
        
        if(Input.GetKeyDown("e"))
            SwapWeaponDown();
        
        if(Input.GetKeyDown("r"))
            Reload();
    }
}
