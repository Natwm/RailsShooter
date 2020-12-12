using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class AoEDamageObject : HealthManager
{
    [Space]
    [Header("Explosion Info")]
    [SerializeField] private int m_ExplosionDamage;

    [Space]
    [Header ("Explosion Param")]
    [SerializeField] private Transform m_ExplosionTransform;
    [SerializeField] private float m_ExplosionRadius;
    [SerializeField] private LayerMask m_ExplosionLayer;

    [SerializeField] private GameObject m_Explosionfx;

    [Space]
    [Header("Sound")]
    FMOD.Studio.EventInstance explosionHitEffect;
    [FMODUnity.EventRef] [SerializeField] private string explosionHitSound;

    private void Start()
    {
        explosionHitEffect = FMODUnity.RuntimeManager.CreateInstance(explosionHitSound);
        FMODUnity.RuntimeManager.AttachInstanceToGameObject(explosionHitEffect, GetComponent<Transform>(), GetComponentInParent<Rigidbody>());
    }

    protected override void Death(GameObject bullet)
    {
        explosionHitEffect.start();
        //deathSoundEffect.start();
        Debug.Log("Explosion");
        GameObject objectCheck = null;

        Collider[] list = Physics.OverlapSphere(m_ExplosionTransform.position, m_ExplosionRadius, m_ExplosionLayer);
        Debug.Log(list.Length);

        for (int i = 0; i < list.Length; i++)
        {
            if (list[i].gameObject != this.transform.GetChild(0).gameObject)
            {
                objectCheck = list[i].gameObject;

                if (list[i].gameObject.GetComponent<BodyPartBehaviours>() != null)
                {
                    Debug.Log(list[i].gameObject.name);
                    list[i].gameObject.GetComponent<BodyPartBehaviours>().GetDamage(m_ExplosionDamage, this, list[i].gameObject);
                }
            }
        }
        GameObject a = Instantiate(m_Explosionfx, transform.position, Quaternion.identity);
        a.transform.DOMoveY(2,1);
        a.transform.DOScale(6, 1);

        Destroy(this.gameObject);
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(m_ExplosionTransform.position, m_ExplosionRadius);
    }
}
