using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BodyPartBehaviours : MonoBehaviour
{
    #region PARAM

    [Min(1)]
    [SerializeField] private int m_DamageMultiplicator;
    [SerializeField] private int m_DamageAdd;
    [SerializeField] private int m_AmountOfArmor;
    [SerializeField] private HealthManager m_healthManager;


    #endregion

    private void Start()
    {
        //m_healthManager = GetComponentInParent<HealthManager>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            GetDamage(4);
        }
    }

    public void GetDamage(int amountOfDamage)
    {
        int damageGive;
        int damageRecieve;

        damageRecieve = (amountOfDamage * m_DamageMultiplicator) + m_DamageAdd;

        if (m_AmountOfArmor > 0)
            damageGive = damageRecieve - m_AmountOfArmor;
        else
            damageGive = damageRecieve;

        if (m_AmountOfArmor > 0)
            m_AmountOfArmor -= damageRecieve;

        if(damageGive > 0)
            m_healthManager.DeacreseLife(damageGive);
        else
            Debug.Log("not enought"); 
    }
}
