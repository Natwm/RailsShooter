using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Intro : MonoBehaviour
{
    public FMOD.Studio.EventInstance introScreenEffect;
    [FMODUnity.EventRef] [SerializeField] private string introScreenSound;
    // Start is called before the first frame update
    void Start()
    {
        introScreenEffect = FMODUnity.RuntimeManager.CreateInstance(introScreenSound);
        FMODUnity.RuntimeManager.AttachInstanceToGameObject(introScreenEffect, GetComponent<Transform>(), GetComponentInParent<Rigidbody>());

        introScreenEffect.setParameterValue("start", 0f);
        introScreenEffect.start();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
