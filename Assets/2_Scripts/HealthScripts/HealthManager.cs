using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthManager : MonoBehaviour
{
    #region PARAM
    [SerializeField] public int m_AmountOfLive;
    [SerializeField] protected Animator animator;

    [Space]
    [Header ("Sound")]
    protected FMOD.Studio.EventInstance hitSoundEffect;
    [FMODUnity.EventRef] [SerializeField] protected string hitSound;

    protected FMOD.Studio.EventInstance deathSoundEffect;
    [FMODUnity.EventRef] [SerializeField] protected string deathSound;


    #endregion

    private void Start()
    {
        hitSoundEffect = FMODUnity.RuntimeManager.CreateInstance(hitSound);
        FMODUnity.RuntimeManager.AttachInstanceToGameObject(hitSoundEffect, transform, GetComponentInParent<Rigidbody>());

        deathSoundEffect = FMODUnity.RuntimeManager.CreateInstance(deathSound);
        FMODUnity.RuntimeManager.AttachInstanceToGameObject(deathSoundEffect, transform, GetComponentInParent<Rigidbody>());
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

    public virtual void Heal(int heal)
    {
        if(heal <= 0)
        {
            Debug.LogError("heal must be positive");
            return;
        }
        
        m_AmountOfLive += heal;
    }

    protected virtual void Death(GameObject Bullet)
    {
        Debug.Log("Dead");
        animator.SetTrigger("Trigger_Die");
        deathSoundEffect.start();

        //Destroy(gameObject,1f);
    }
}
