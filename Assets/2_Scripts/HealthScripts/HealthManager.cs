﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthManager : MonoBehaviour
{
    #region PARAM
    [SerializeField] private int m_AmountOfLive;
    [SerializeField] private Animator animator;

    FMOD.Studio.EventInstance hitSoundEffect;
    [FMODUnity.EventRef] [SerializeField] private string hitSound;

    FMOD.Studio.EventInstance deathSoundEffect;
    [FMODUnity.EventRef] [SerializeField] private string deathSound;
    #endregion

    private void Start()
    {
        hitSoundEffect = FMODUnity.RuntimeManager.CreateInstance(hitSound);
        FMODUnity.RuntimeManager.AttachInstanceToGameObject(hitSoundEffect, GetComponent<Transform>(), GetComponentInParent<Rigidbody>());

        deathSoundEffect = FMODUnity.RuntimeManager.CreateInstance(deathSound);
        FMODUnity.RuntimeManager.AttachInstanceToGameObject(deathSoundEffect, GetComponent<Transform>(), GetComponentInParent<Rigidbody>());
    }

    public void DeacreseLife( int damage)
    {
        m_AmountOfLive -= damage;
        
        if (m_AmountOfLive <= 0)
        {
            Death();
        }
        else
        {
            hitSoundEffect.start();
            animator.SetTrigger("Trigger_Hit");
        }
            
    }

    protected virtual void Death()
    {
        Debug.Log("Dead");
        animator.SetTrigger("Trigger_Die");
        deathSoundEffect.start();
        //Destroy(gameObject,1f);
    }
}
