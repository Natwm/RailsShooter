using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TimeController : MonoBehaviour
{
    public static TimeController instance;

    private int slowmoTimer = 0;

    [Tooltip ("Valeurs min pour relancer le slowmo")]
    [SerializeField] private float minKillForSlowMotion;

    [SerializeField] private float slowMotionBaseDuration;
    [SerializeField] private float slowMotionGain;


    [Space]
    [Header ("FLOW TIME CONTROL")]
    [SerializeField] private float normalFlowTime = 1;
    [SerializeField] private float slowMotionFlowTime = 0.1f;
    [SerializeField] private float FlowTimeLerpValue = 0.2f;

    [Space]
    [Header("Sound")]
    FMOD.Studio.EventInstance slowMotionEffect;
    [FMODUnity.EventRef] [SerializeField] private string slowMotionSnapshot;

    protected FMOD.Studio.EventInstance startSlowEffect;
    [FMODUnity.EventRef] [SerializeField] protected string startSlowSound;

    protected FMOD.Studio.EventInstance endSlowEffect;
    [FMODUnity.EventRef] [SerializeField] protected string endSlowSound;


    private float targetFlowTime = 1;
    private float baseFixedDeltaTime;
    private TimeNonAffectedTimer slowMotionTimer;

    public bool isActivedOnce = false;

    public TimeNonAffectedTimer SlowMotionTimer { get => slowMotionTimer; set => slowMotionTimer = value; }

    void Awake()
    {
        if (instance != null)
            Debug.LogWarning("Multiple instance of same Singleton : GameManager");
        instance = this;

        targetFlowTime = 1;
        Time.fixedDeltaTime = 0.02f;
        baseFixedDeltaTime = Time.fixedDeltaTime;
        slowMotionTimer = new TimeNonAffectedTimer(slowMotionBaseDuration, EndSlowMotion);
    }

    private void Start()
    {
        slowMotionEffect = FMODUnity.RuntimeManager.CreateInstance(slowMotionSnapshot);
        FMODUnity.RuntimeManager.AttachInstanceToGameObject(slowMotionEffect, GameManager.instance.player.transform, GameManager.instance.player.GetComponent<Rigidbody>());

        endSlowEffect = FMODUnity.RuntimeManager.CreateInstance(endSlowSound);
        FMODUnity.RuntimeManager.AttachInstanceToGameObject(endSlowEffect, GameManager.instance.player.GetComponent<Transform>(), GameManager.instance.player.GetComponentInParent<Rigidbody>());

        startSlowEffect = FMODUnity.RuntimeManager.CreateInstance(startSlowSound);
        FMODUnity.RuntimeManager.AttachInstanceToGameObject(startSlowEffect, GameManager.instance.player.GetComponent<Transform>(), GameManager.instance.player.GetComponentInParent<Rigidbody>());

        slowMotionEffect.start();
        slowMotionEffect.setParameterValue("Instance", 0f);
    }

    void SetTime(float secondDuration)
    {
        targetFlowTime = secondDuration;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Z) && slowmoTimer >= minKillForSlowMotion)
        {
            slowmoTimer = 0;
            StartSlowMotion();
        }

        if(Input.GetKeyDown("[+]"))
            StartSlowMotion();
        if(Input.GetKeyDown("[-]"))
            MaintainSlowMotion();
    }

    void FixedUpdate()
    {
        Time.timeScale = Mathf.Lerp(Time.timeScale, targetFlowTime, FlowTimeLerpValue);
        Time.fixedDeltaTime = baseFixedDeltaTime * Time.timeScale;
    }

    public void LaunchSlowMotion()
    {
        slowMotionEffect.setParameterValue("Intensity", 100f);

        GameManager.instance.StartSlowmo();

        slowMotionTimer = new TimeNonAffectedTimer(slowmoTimer, EndSlowMotion);

        isActivedOnce = true;
        slowMotionTimer.ResetPlay();
    }

    public void StartSlowMotion()
    {
        SetTime(slowMotionFlowTime);
        slowMotionEffect.setParameterValue("Intensity", 100f);

        startSlowEffect.start();

        GameManager.instance.StartSlowmo();

        slowMotionTimer = new TimeNonAffectedTimer(slowMotionBaseDuration, EndSlowMotion);

        isActivedOnce = true;
        slowMotionTimer.ResetPlay();
    }

    public void MaintainSlowMotion()
    {
        if(slowMotionTimer.IsStarted() == true)
        {
            slowMotionTimer.Pause();
            slowMotionTimer = new TimeNonAffectedTimer((slowMotionTimer.endTime - slowMotionTimer.Time) + slowMotionGain, EndSlowMotion);
            slowMotionTimer.ResetPlay();
        }
    }

    private void EndSlowMotion()
    {
        SetTime(normalFlowTime);
        slowMotionEffect.setParameterValue("Intensity", 0f);
        GameManager.instance.EndSlowmo();
        endSlowEffect.start();
    }

    public void increaseSlowMoTime()
    {
        slowmoTimer += 1;
    }

    public bool IsSlowMotion()
    {
        return slowMotionTimer.IsStarted();
    }
}
