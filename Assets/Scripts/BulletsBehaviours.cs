using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletsBehaviours : MonoBehaviour
{
    #region PARAM
    private int damage;

    FMOD.Studio.EventInstance wallHitEffect;
    [FMODUnity.EventRef] [SerializeField] private string wallHitSound;

    FMOD.Studio.EventInstance groundHitEffect;
    [FMODUnity.EventRef] [SerializeField] private string groundHitSound;
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
            other.GetComponent<BodyPartBehaviours>().GetDamage(damage);
            this.OnPoolEnter();
        }

        if(other.gameObject.layer == 9)
        {
            Debug.Log(other.gameObject.GetComponent<Renderer>().material.name);
            switch (other.gameObject.GetComponent<Renderer>().material.name)
            {
                case "Ground (Instance)":
                    groundHitEffect.start();
                    break;

                case "Wall (Instance)":
                    wallHitEffect.start();
                    break;
            }
            
        }
    }

    public void Launch(int damage)
    {
        this.damage = damage;
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
