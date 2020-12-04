using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthBottle : HealthManager
{
    public int amountOfHealt = 10;

    /*public void DeacreseLife(int damage, GameObject Bullet)
    {

    }*/

    protected override void Death(GameObject bullet)
    {
        if(bullet.GetComponent<BulletsBehaviours>()?.shooter.gameObject.layer == 10)
        {
            deathSoundEffect.start();
            bullet.GetComponent<BulletsBehaviours>()?.shooter.Heal(amountOfHealt);
            Destroy(gameObject);
        }
    }
}
