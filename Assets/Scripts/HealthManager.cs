using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthManager : MonoBehaviour
{
    #region PARAM
    [SerializeField] private int m_AmountOfLive;
    #endregion

    public void DeacreseLife( int damage)
    {
        m_AmountOfLive -= damage;
        if (m_AmountOfLive <= 0)
            Debug.Log("Dead");
    }

}
