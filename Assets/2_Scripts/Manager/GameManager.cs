﻿using System.Collections;
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

    private bool endGame;
    #endregion

    void Awake()
    {
        if(instance != null)
            Debug.LogWarning("Multiple instance of same Singleton : GameManager");
        instance = this;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && endGame)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }

    public void GameOver()
    {
        endGame = true;
        Debug.Log("GameOver");
        CanvaManager.instance.EndGame("Défaite");
        //SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void GameWin()
    {
        endGame = true;
        CanvaManager.instance.EndGame("Victoire");
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
