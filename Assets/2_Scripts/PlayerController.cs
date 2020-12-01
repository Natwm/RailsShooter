using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : HealthManager
{
    protected override void Death(GameObject bullet)
    {
        deathSoundEffect.start();
        GameManager.instance.GameOver();
    }

    public override void DeacreseLife(int damage, GameObject Bullet)
    {
        animator.SetTrigger("Trigger_PlayerHit");
        m_AmountOfLive -= damage;

        if (m_AmountOfLive <= 0)
        {
            Death(Bullet);
        }
        else
        {
            hitSoundEffect.start();
        }

    }

}
