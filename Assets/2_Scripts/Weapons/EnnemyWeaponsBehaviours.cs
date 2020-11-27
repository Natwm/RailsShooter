using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnnemyWeaponsBehaviours : WeaponsBehaviours
{
    public override void Shoot()
    {
        if(currentNumberOfBullets > 0)
        {
            print("Ennemy Shoot");
            Vector3 targetPosition = GameManager.instance.player.transform.position;
            Vector3 direction = targetPosition - transform.position;

            currentNumberOfBullets --;
            shootEffect.start();
            fireRateTimer.ResetPlay();

            RaycastHit hit;
            if (UseProjectile == true)
            {
                GameObject projectile = BulletPool.instance.GetBullet(m_projectilePrefab);
                projectile.transform.position = Camera.main.transform.position;
                projectile.transform.rotation = Quaternion.Euler(direction);
                projectile.GetComponent<Rigidbody>().velocity = direction.normalized * projectileSpeed;
                projectile.GetComponent<BulletsBehaviours>().Launch(m_Damage, myHealthManager);
            }
            else
            {
                if (Physics.SphereCast(Camera.main.transform.position, raycasRadius, direction, out hit, Mathf.Infinity, bulletCollisionLayerMask))
                {
                    if (hit.transform.gameObject.layer == 8)
                    {
                        hit.transform.GetComponent<BodyPartBehaviours>().GetDamage(m_Damage, myHealthManager);
                    }
                    if (hit.transform.gameObject.layer == 9)
                    {
                        hit.transform.GetComponent<BodyPartBehaviours>()?.GetDamage(m_Damage, myHealthManager);
                    }
                    if (hit.transform.gameObject.layer == 10)
                    {
                        hit.transform.GetComponent<BodyPartBehaviours>().GetDamage(m_Damage, myHealthManager);
                    }
                }
                else
                {
                    // doesn't touch
                }
            }

        }


        if (currentNumberOfBullets == 0 && AutoReload == true)
        {
            Reload();
        }

    }

}
