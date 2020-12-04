using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnnemyWeaponsBehaviours : WeaponsBehaviours
{
    public GameObject shootpos;

    public override void Shoot(Animator anim, Vector3 direction)
    {
        GameObject projectile;
        if (currentNumberOfBullets > 0)
        {
            // print("Ennemy Shoot");
            Vector3 targetPosition = GameManager.instance.player.transform.position;
            direction = targetPosition - transform.position;

            currentNumberOfBullets --;
            //shootEffect.start();
            fireRateTimer.ResetPlay();

            RaycastHit hit;

            Debug.DrawRay(transform.position, direction);

            if (UseProjectile == true)
            {
                projectile = BulletPool.instance.GetBullet(m_projectilePrefab);
                projectile.transform.position = shootpos.transform.position;
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
                        hit.transform.GetComponent<BodyPartBehaviours>().GetDamage(m_Damage, myHealthManager, null);
                    }
                    if (hit.transform.gameObject.layer == 9)
                    {
                        hit.transform.GetComponent<BodyPartBehaviours>()?.GetDamage(m_Damage, myHealthManager, null);
                    }
                    if (hit.transform.gameObject.layer == 10)
                    {
                        hit.transform.GetComponent<BodyPartBehaviours>().GetDamage(m_Damage, myHealthManager, null);
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

    public override void Reload()
    {
        if(!isReloading && currentNumberOfBullets < m_NumberOfBulletsPerMagazine)
        {
            GameManager.instance.UpdateAmountOfBulltes(currentNumberOfBullets);
        }
    }

}
