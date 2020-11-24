using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthManager : MonoBehaviour
{
    #region PARAM
    [SerializeField] private int m_AmountOfLive;
    [SerializeField] private GameObject hitParticules;

    #endregion

    public void DeacreseLife( int damage)
    {
        Instantiate(hitParticules, transform.position,Quaternion.identity);
        m_AmountOfLive -= damage;
        if (m_AmountOfLive <= 0)
        {
            Debug.Log("Dead");
            Destroy(gameObject);
        }
            
    }

}
