using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponAnimationController : MonoBehaviour
{
    public void StartReload()
    {
        WeaponManager.instance.GetCurrentWeapon().isReloading = true;
    }
    
    public void EndReload()
    {
        WeaponManager.instance.GetCurrentWeapon().RefillMagazine();
        WeaponManager.instance.GetCurrentWeapon().isReloading = false;
    }
}
