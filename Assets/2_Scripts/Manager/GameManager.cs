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
    private int totalAmountOfEnnemy;

    public bool canAction;

    private bool endGame;

    public int amountOfKill = 0;



    [Space]
    [Header("SOUND")]
    protected FMOD.Studio.EventInstance victoryEffect;
    [FMODUnity.EventRef] [SerializeField] private string victorySound;

    protected FMOD.Studio.EventInstance loseEffect;
    [FMODUnity.EventRef] [SerializeField] private string loseSound;

    protected FMOD.Studio.EventInstance MainMusique;
    [FMODUnity.EventRef] [SerializeField] private string MainMusiqueSound;

    protected FMOD.Studio.EventInstance AmbiantMusique;
    [FMODUnity.EventRef] [SerializeField] private string AmbiantMusiqueSound;


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

        MainMusique = FMODUnity.RuntimeManager.CreateInstance(MainMusiqueSound);
        FMODUnity.RuntimeManager.AttachInstanceToGameObject(MainMusique, GetComponent<Transform>(), GetComponentInParent<Rigidbody>());

        AmbiantMusique = FMODUnity.RuntimeManager.CreateInstance(AmbiantMusiqueSound);
        FMODUnity.RuntimeManager.AttachInstanceToGameObject(AmbiantMusique, GetComponent<Transform>(), GetComponentInParent<Rigidbody>());

        AmbiantMusique.start();
        AmbiantMusique.setParameterValue("volume", 1f);
        //MainMusique.start();
        //
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
        FindObjectOfType<SceneLoad>().SceneToLoad = "Narrative_Defeat";
        MainMusique.setParameterValue("fin", 1);
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

    public void UpdateSlowMotionUI(float value)
    {
        CanvaManager.instance.UpdateSlider(value);
    }

    public void ResetSloMotionUI()
    {
        CanvaManager.instance.ResetSlider();
    }

    public void ShowReload(bool status)
    {
        CanvaManager.instance.showRealod(status);
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
        totalAmountOfEnnemy++;
        amountOfEnnemy++;
    }

    public void DecreaseAmountofEnnemy()
    {
        amountOfKill ++;
        amountOfEnnemy--;
        if(totalAmountOfEnnemy / 2 < amountOfEnnemy)
        {
            MainMusique.setParameterValue("moitie", 1);
        }

        if(amountOfEnnemy <= 0)
        {
            MainMusique.setParameterValue("fin", 1);
            GameWin();
        }
    }

    public void SetMusiqueOn()
    {
        AmbiantMusique.setParameterValue("volume",0f);
        MainMusique.start();
    }

    public void StartSlowmo()
    {
        
        MainMusique.setParameterValue("ralenti", 1);
        
    }
    
    public void EndSlowmo()
    {
        MainMusique.setParameterValue("ralenti", 0f);
    }
}
