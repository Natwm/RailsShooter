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
            

            RaycastHit hit;            
            if(UseProjectile == true)
            {
                Debug.DrawLine(transform.position, transform.position + direction * 100, Color.green, 10);
                Debug.LogWarning("No projectile yet");
            }
            else
            {
                if(Physics.SphereCast(transform.position, raycasRadius, direction, out hit, Mathf.Infinity, bulletCollisionLayerMask))
                {
                    if(hit.transform.gameObject.layer == 10)
                    {
                        Debug.Log("Player touched");
                        hit.transform.GetComponent<BodyPartBehaviours>().GetDamage(m_Damage);
                    }
                    if(hit.transform.gameObject.layer == 9)
                    {
                        Debug.Log("Obstacle touched");
                    }
                    if(hit.transform.gameObject.layer == 8)
                    {
                        Debug.Log("Ally touched");
                        hit.transform.GetComponent<BodyPartBehaviours>().GetDamage(m_Damage);
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
}
