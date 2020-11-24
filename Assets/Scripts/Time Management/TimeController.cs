using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TimeController : MonoBehaviour
{
    public static TimeController instance;
    [SerializeField] private float normalFlowTime = 1;
    [SerializeField] private float slowMotionFlowTime = 0.1f;
    [SerializeField] private float FlowTimeLerpValue = 0.2f;
    private float targetFlowTime = 1;
    private float baseFixedDeltaTime;

    void Awake()
    {
        targetFlowTime = 1;
        baseFixedDeltaTime = Time.fixedDeltaTime;
    }

    public void SetTime(float secondDuration)
    {
        targetFlowTime = secondDuration;
    }

    void Update()
    {
        if(Input.GetKeyDown("[+]"))
            SetTime(slowMotionFlowTime);
        if(Input.GetKeyDown("[-]"))
            SetTime(normalFlowTime);
    }

    void FixedUpdate()
    {
        Time.timeScale = Mathf.Lerp(Time.timeScale, targetFlowTime, FlowTimeLerpValue);
        Time.fixedDeltaTime = baseFixedDeltaTime * Time.timeScale;
    }
}
