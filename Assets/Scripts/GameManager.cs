using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    #region PARAM
    public PlayerController player;
    #endregion

    void Awake()
    {
        if(instance != null)
            Debug.LogWarning("Multiple instance of same Singleton : GameManager");
        instance = this;
    }
}
