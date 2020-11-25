using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TimeController : MonoBehaviour
{
    public static TimeController instance;
    [SerializeField] private float slowMotionBaseDuration;
    [SerializeField] private float slowMotionGain;

    [Space]
    [Header ("FLOW TIME CONTROL")]
    [SerializeField] private float normalFlowTime = 1;
    [SerializeField] private float slowMotionFlowTime = 0.1f;
    [SerializeField] private float FlowTimeLerpValue = 0.2f;


    private float targetFlowTime = 1;
    private float baseFixedDeltaTime;
    private TimeNonAffectedTimer slowMotionTimer;

    void Awake()
    {
        targetFlowTime = 1;
        baseFixedDeltaTime = Time.fixedDeltaTime;
        slowMotionTimer = new TimeNonAffectedTimer(slowMotionBaseDuration, EndSlowMotion);
    }

    void SetTime(float secondDuration)
    {
        targetFlowTime = secondDuration;
    }

    void Update()
    {
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

    public void StartSlowMotion()
    {
        SetTime(slowMotionFlowTime);
        
        slowMotionTimer = new TimeNonAffectedTimer(slowMotionBaseDuration, EndSlowMotion);
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
    }
}
