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

    private bool endGame;

    [Space]
    [Header("SOUND")]
    protected FMOD.Studio.EventInstance victoryEffect;
    [FMODUnity.EventRef] [SerializeField] private string victorySound;

    protected FMOD.Studio.EventInstance loseEffect;
    [FMODUnity.EventRef] [SerializeField] private string loseSound;

    #endregion

    void Awake()
    {
        if(instance != null)
            Debug.LogWarning("Multiple instance of same Singleton : GameManager");
        instance = this;
    }

    private void Start()
    {
        victoryEffect = FMODUnity.RuntimeManager.CreateInstance(victorySound);
        FMODUnity.RuntimeManager.AttachInstanceToGameObject(victoryEffect, GameManager.instance.player.transform, GetComponentInParent<Rigidbody>());

        loseEffect = FMODUnity.RuntimeManager.CreateInstance(loseSound);
        FMODUnity.RuntimeManager.AttachInstanceToGameObject(loseEffect, GetComponent<Transform>(), GetComponentInParent<Rigidbody>());
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
        loseEffect.start();
        endGame = true;
        Debug.Log("GameOver");
        Destroy(player);
        CanvaManager.instance.EndGame("Défaite");
        //SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    public void GameWin()
    {
        victoryEffect.start();
        endGame = true;
        CanvaManager.instance.EndGame("Victoire");
        Debug.Log("Win");
    }

    public void UpdateAmountOfLife(int life)
    {
        CanvaManager.instance.UpdateAmountOfLife(life);
    }

    public void UpdateAmountOfBulltes(int bullets)
    {
        CanvaManager.instance.UpdateAmountOfBullets(bullets);
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
