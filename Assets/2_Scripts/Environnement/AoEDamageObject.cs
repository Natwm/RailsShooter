using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    protected override void Death()
    {
        Debug.Log("Explosion");
        GameObject objectCheck = null;

        Collider[] list = Physics.OverlapSphere(m_ExplosionTransform.position, m_ExplosionRadius, m_ExplosionLayer);

        for (int i = 0; i < list.Length; i++)
        {
            if (list[i].gameObject != this.transform.GetChild(0).gameObject)
            {
                if (objectCheck == null || list[i].transform.parent != objectCheck.transform.parent)
                {
                    if (list[i].gameObject.GetComponent<BodyPartBehaviours>() != null)
                    {
                        Debug.Log(list[i].gameObject.name);
                        list[i].gameObject.GetComponent<BodyPartBehaviours>().GetDamage(m_ExplosionDamage, this);
                    }  
                }
                objectCheck = list[i].gameObject;
            }
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(m_ExplosionTransform.position, m_ExplosionRadius);
    }
}
