using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CanvaManager : MonoBehaviour
{
    public static CanvaManager instance;

    [SerializeField] private GameObject victoryObject;

    [SerializeField] private TMP_Text m_VictoryText;

    void Awake()
    {
        if (instance != null)
            Debug.LogWarning("Multiple instance of same Singleton : GameManager");
        instance = this;
    }

    private void Start()
    {
        if (victoryObject.active)
        {
            victoryObject.SetActive(false);
        }
    }

    public void EndGame(string message)
    {
        victoryObject.active = true;
        m_VictoryText.text = message;
    }

}
