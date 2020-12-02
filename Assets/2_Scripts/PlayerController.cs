using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : HealthManager
{

    private void Start()
    {
        hitSoundEffect = FMODUnity.RuntimeManager.CreateInstance(hitSound);
        FMODUnity.RuntimeManager.AttachInstanceToGameObject(hitSoundEffect, GetComponent<Transform>(), GetComponentInParent<Rigidbody>());

        deathSoundEffect = FMODUnity.RuntimeManager.CreateInstance(deathSound);
        FMODUnity.RuntimeManager.AttachInstanceToGameObject(deathSoundEffect, GetComponent<Transform>(), GetComponentInParent<Rigidbody>());
    }

    protected override void Death(GameObject bullet)
    {
        deathSoundEffect.start();
        GameManager.instance.GameOver();
    }

    public override void DeacreseLife(int damage, GameObject Bullet)
    {
        animator.SetTrigger("Trigger_PlayerHit");
        m_AmountOfLive -= damage;

        GameManager.instance.UpdateAmountOfLife(m_AmountOfLive);

        if (m_AmountOfLive <= 0)
        {
            Death(Bullet);
        }
        else
        {
            Debug.Log("tet");
            hitSoundEffect.start();
        }

    }

}
