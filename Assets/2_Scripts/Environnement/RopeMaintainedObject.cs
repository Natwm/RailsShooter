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

    protected override void Death(GameObject bullet)
    {
        rig.isKinematic = false;
        ropeTransform.gameObject.SetActive(false);
    }
}
