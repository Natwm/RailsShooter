using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthManager : MonoBehaviour
{
    #region PARAM
    [SerializeField] protected int m_AmountOfLive;
    [SerializeField] protected Animator animator;

    protected FMOD.Studio.EventInstance hitSoundEffect;
    [FMODUnity.EventRef] [SerializeField] protected string hitSound;

    protected FMOD.Studio.EventInstance deathSoundEffect;
    [FMODUnity.EventRef] [SerializeField] protected string deathSound;

    #endregion

    private void Start()
    {
        hitSoundEffect = FMODUnity.RuntimeManager.CreateInstance(hitSound);
        FMODUnity.RuntimeManager.AttachInstanceToGameObject(hitSoundEffect, GetComponent<Transform>(), GetComponentInParent<Rigidbody>());

        deathSoundEffect = FMODUnity.RuntimeManager.CreateInstance(deathSound);
        FMODUnity.RuntimeManager.AttachInstanceToGameObject(deathSoundEffect, GetComponent<Transform>(), GetComponentInParent<Rigidbody>());
    }

    public virtual void DeacreseLife( int damage, GameObject Bullet)
    {
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

    protected virtual void Death(GameObject Bullet)
    {
        Debug.Log("Dead");
        animator.SetTrigger("Trigger_Die");
        deathSoundEffect.start();

        //Destroy(gameObject,1f);
    }
}
