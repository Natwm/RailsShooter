﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CanvaManager : MonoBehaviour
{
    public static CanvaManager instance;

    [Space]
    [Header("Panel")]
    [SerializeField] private GameObject endGame_Panel;
    [SerializeField] private GameObject infos_Panel;

    [Space]
    [Header ("Text")]
    [SerializeField] private TMP_Text m_EndGame_Text;
    [SerializeField] private TMP_Text m_AmountOfLife_Text;
    [SerializeField] private TMP_Text m_AmountOfBullet_Text;

    void Awake()
    {
        if (instance != null)
            Debug.LogWarning("Multiple instance of same Singleton : GameManager");
        instance = this;
    }

    private void Start()
    {
        if (endGame_Panel.active)
        {
            endGame_Panel.SetActive(false);
        }
        m_AmountOfLife_Text.text = GameManager.instance.player.m_AmountOfLive.ToString();
        m_AmountOfBullet_Text.text = GameManager.instance.player.transform.GetComponentInChildren<WeaponManager>().currentWeapon.currentNumberOfBullets.ToString();
    }

    public void UpdateAmountOfLife(int life)
    {
        m_AmountOfLife_Text.text = "Amount Of Life : " + life.ToString();
    }
    public void UpdateAmountOfBullets(int bullets)
    {
        m_AmountOfBullet_Text.text = "Amount Of Bullet : "+ bullets.ToString();
    }

    public void EndGame(string message)
    {
        endGame_Panel.active = true;
        m_EndGame_Text.text = message;
    }

}