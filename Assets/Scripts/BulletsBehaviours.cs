using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletsBehaviours : MonoBehaviour
{
    #region PARAM
    private int damage;
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
    }

    public void Launch(int damage)
    {
        this.damage = damage;
        timeToLive = new Timer(5, OnPoolEnter);
        timeToLive.ResetPlay();
    }

    void Update()
    {
        transform.rotation = Quaternion.Euler(GetComponent<Rigidbody>().velocity);
    }
}
