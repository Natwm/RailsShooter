﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletsBehaviours : MonoBehaviour
{
    #region PARAM
    private int damage;
    private HealthManager shooter;

    FMOD.Studio.EventInstance wallHitEffect;
    [FMODUnity.EventRef] [SerializeField] private string wallHitSound;

    FMOD.Studio.EventInstance groundHitEffect;
    [FMODUnity.EventRef] [SerializeField] private string groundHitSound;

    [SerializeField] private LayerMask DamagebleLayer;
    #endregion

    private BulletPool bulletPool;
    private Timer timeToLive;

    public void OnPoolExit(BulletPool pool)
    {
        bulletPool = pool;
        this.gameObject.SetActive(true);
    }

    public void OnPoolEnter()
    {
        bulletPool.AddBullet(this);
        this.gameObject.SetActive(false);
    }

    void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.layer == 8 || other.gameObject.layer == 10)
        {
            if(other.GetComponent<BodyPartBehaviours>().m_healthManager != shooter)
            {
                other.GetComponent<BodyPartBehaviours>().GetDamage(damage, shooter);
                this.OnPoolEnter();
            }
        }

        if(other.gameObject.layer == DamagebleLayer)
        {
            other.GetComponent<BodyPartBehaviours>()?.GetDamage(damage, shooter);
            
            switch (other.gameObject.GetComponent<Renderer>().material.name)
            {
                case "Ground (Instance)":
                    groundHitEffect.start();
                    break;

                case "Wall (Instance)":
                    wallHitEffect.start();
                    break;
            }
            
            this.OnPoolEnter();
        }
    }

    public void Launch(int damage, HealthManager shooter)
    {
        this.damage = damage;
        this.shooter = shooter;
        timeToLive = new Timer(5, OnPoolEnter);
        timeToLive.ResetPlay();
    }

    private void Start()
    {
        wallHitEffect = FMODUnity.RuntimeManager.CreateInstance(wallHitSound);
        FMODUnity.RuntimeManager.AttachInstanceToGameObject(wallHitEffect, GetComponent<Transform>(), GetComponentInParent<Rigidbody>());

        groundHitEffect = FMODUnity.RuntimeManager.CreateInstance(groundHitSound);
        FMODUnity.RuntimeManager.AttachInstanceToGameObject(groundHitEffect, GetComponent<Transform>(), GetComponentInParent<Rigidbody>());
    }

    void Update()
    {
        transform.rotation = Quaternion.Euler(GetComponent<Rigidbody>().velocity);
    }
}