using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sound_Anim : MonoBehaviour
{

    FMOD.Studio.EventInstance stepSoundEffect;
    [FMODUnity.EventRef] [SerializeField] private string stepSound;

    // Start is called before the first frame update
    void Start()
    {
        stepSoundEffect = FMODUnity.RuntimeManager.CreateInstance(stepSound);
        FMODUnity.RuntimeManager.AttachInstanceToGameObject(stepSoundEffect, GetComponent<Transform>(), GetComponentInParent<Rigidbody>());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PlayAnimSound()
    {
        stepSoundEffect.start();
    }
}
