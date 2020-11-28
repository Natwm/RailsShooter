using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    #region PARAM
    public PlayerController player;

    private int amountOfEnnemy;

    public bool canAction;
    #endregion

    void Awake()
    {
        if(instance != null)
            Debug.LogWarning("Multiple instance of same Singleton : GameManager");
        instance = this;
    }

    public void GameOver()
    {
        Debug.Log("GameOver");
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void GameWin()
    {
        Debug.Log("Win");
    }

    public void IncreaseAmountofEnnemy()
    {
        amountOfEnnemy++;
    }

    public void DecreaseAmountofEnnemy()
    {
        amountOfEnnemy--;
        if(amountOfEnnemy <= 0)
        {
            GameWin();
        }
    }
}
