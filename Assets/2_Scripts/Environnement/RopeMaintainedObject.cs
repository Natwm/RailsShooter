using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class RopeMaintainedObject : HealthManager
{
    [SerializeField] private Transform ropeTransform;
    private Rigidbody rig;

    [Space]
    [Header("SOUND")]
    protected FMOD.Studio.EventInstance ropeEffect;
    [FMODUnity.EventRef] [SerializeField] private string ropeSound;


    void Awake()
    {
        rig = GetComponent<Rigidbody>();
        rig.isKinematic = true;
    }

    private void Start()
    {
        ropeEffect = FMODUnity.RuntimeManager.CreateInstance(ropeSound);
        FMODUnity.RuntimeManager.AttachInstanceToGameObject(ropeEffect, GetComponent<Transform>(), GetComponentInParent<Rigidbody>());
    }

    protected override void Death(GameObject bullet)
    {
        ropeEffect.start();
        rig.isKinematic = false;
        ropeTransform.gameObject.SetActive(false);
    }
}
