using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HggAnim : MonoBehaviour
{
    public EnnemyWeaponsBehaviours weapons;
    public void Hit()
    {
        weapons.Shoot(null, Vector3.zero);
    }
}
