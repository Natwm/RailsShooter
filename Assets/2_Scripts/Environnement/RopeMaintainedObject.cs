using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class RopeMaintainedObject : HealthManager
{
    [SerializeField] private Transform ropeTransform;
    private Rigidbody rig;

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
