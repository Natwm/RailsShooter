using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestructibleObject : HealthManager
{
    Timer disappearTimer, fadingTimer;

    [Header ("DESTRUCTION")]
    [SerializeField] private bool shouldDisappear;
    [SerializeField] private float disappearTime;
    [SerializeField] private float fadingTime;
    private bool isFading;

    private void Start() 
    {
        if(fadingTime >= 0 && disappearTime >= fadingTime)
        {
            disappearTimer = new Timer(disappearTime - fadingTime, Hide);
        }
        else
        {
            disappearTimer = new Timer(disappearTime, Delete);
        }
    }

    private void Update() 
    {
        if(isFading == true)
        {  
            UpdateFading();
        }
    }

    private void UpdateFading()
    {
        Debug.LogError("Not yep implemented");
        // alpha = 1 - (fadingTimer.time / fadingTime);
    }

    private void Hide()
    {
        fadingTimer = new Timer(fadingTime, Delete);
        fadingTimer.ResetPlay();
        isFading = true;
    }

    private void Delete()
    {
        Destroy(this.gameObject);
    }

    protected override void Death()
    {
        if(shouldDisappear == true)
        {
            disappearTimer.Play();
        }

        Debug.Log("explOOOOOOOOsion : launch animation");
    }
}
