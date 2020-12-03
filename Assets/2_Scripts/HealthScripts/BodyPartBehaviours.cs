using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BodyPartBehaviours : MonoBehaviour
{
    #region PARAM

    [Min(1)]
    [SerializeField] private int m_DamageMultiplicator = 1;
    [SerializeField] private int m_DamageAdd;
    [SerializeField] private int m_AmountOfArmor;
    [SerializeField] public HealthManager m_healthManager;


    #endregion

    public void GetDamage(int amountOfDamage, HealthManager shooter, GameObject bullet)
    {

        if (m_healthManager == shooter)
            return;
        
        int damageGive;
        int damageRecieve;

        damageRecieve = (amountOfDamage * m_DamageMultiplicator) + m_DamageAdd;


        if (m_AmountOfArmor > 0)
            damageGive = damageRecieve - m_AmountOfArmor;
        else
            damageGive = damageRecieve;

        if (m_AmountOfArmor > 0)
            m_AmountOfArmor -= damageRecieve;

        if (damageGive > 0)
            m_healthManager.DeacreseLife(damageGive, bullet);
    }
}
