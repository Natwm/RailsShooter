using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HggAnim : MonoBehaviour
{
    public EnnemyWeaponsBehaviours weapons;

    [Space]
    [Header("SOUND")]
    protected FMOD.Studio.EventInstance stepEffect;
    [FMODUnity.EventRef] [SerializeField] private string stepSound;

    private void Start()
    {
        stepEffect = FMODUnity.RuntimeManager.CreateInstance(stepSound);
        FMODUnity.RuntimeManager.AttachInstanceToGameObject(stepEffect, GameManager.instance.player.transform, GetComponentInParent<Rigidbody>());
    }

    public void Hit()
    {
        weapons.Shoot(null, Vector3.zero);
    }

    public void StepSound()
    {
        stepEffect.start();
    }
}
