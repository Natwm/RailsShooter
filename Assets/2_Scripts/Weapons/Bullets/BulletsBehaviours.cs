using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletsBehaviours : MonoBehaviour
{
    #region PARAM
    private int damage;
    [HideInInspector] public HealthManager shooter;

    [Space]
    [Header("Sound")]
    FMOD.Studio.EventInstance dirtHitEffect;
    [FMODUnity.EventRef] [SerializeField] private string dirtHitSound;

    FMOD.Studio.EventInstance steelHitEffect;
    [FMODUnity.EventRef] [SerializeField] private string steelHitSound;

    FMOD.Studio.EventInstance glassHitEffect;
    [FMODUnity.EventRef] [SerializeField] private string glassHitSound;

    [SerializeField] private LayerMask DamagebleLayer;

    [SerializeField] private GameObject hitHumanParticules;
    [SerializeField] private GameObject hitDecorsParticules;
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
    public Quaternion CalculeRotation(Vector3 pos)
    {
        return Quaternion.LookRotation(GameManager.instance.player.transform.position - pos);
    }

    void OnTriggerEnter(Collider other)
    {
        Debug.Log(other.gameObject.layer + " " + other.gameObject.name);
        if(other.gameObject.layer == 8 || other.gameObject.layer == 10 || other.gameObject.layer == 9)
        {
            if(other.gameObject.layer == 8)
            {
                GameObject bulletParticule = Instantiate(hitHumanParticules, transform.position, Quaternion.identity);
                bulletParticule.transform.LookAt(GameManager.instance.player.transform.position);
            }
            
            if (other.GetComponent<BodyPartBehaviours>().m_healthManager != shooter)
            {
                other.GetComponent<BodyPartBehaviours>()?.GetDamage(damage, shooter, this.gameObject);
            }
        }
        this.OnPoolEnter();
    }

    public void Launch(int damage, HealthManager shooter)
    {
        this.damage = damage;
        this.shooter = shooter;

        TrailRenderer tr = transform.GetChild(1).GetComponent<TrailRenderer>();
        tr.Clear();

        timeToLive = new Timer(5, OnPoolEnter);
        timeToLive.ResetPlay();
    }

    private void Start()
    {
        dirtHitEffect = FMODUnity.RuntimeManager.CreateInstance(dirtHitSound);
        FMODUnity.RuntimeManager.AttachInstanceToGameObject(dirtHitEffect, GetComponent<Transform>(), GetComponentInParent<Rigidbody>());

        glassHitEffect = FMODUnity.RuntimeManager.CreateInstance(glassHitSound);
        FMODUnity.RuntimeManager.AttachInstanceToGameObject(glassHitEffect, GetComponent<Transform>(), GetComponentInParent<Rigidbody>());

        steelHitEffect = FMODUnity.RuntimeManager.CreateInstance(steelHitSound);
        FMODUnity.RuntimeManager.AttachInstanceToGameObject(steelHitEffect, GetComponent<Transform>(), GetComponentInParent<Rigidbody>());

    }

    void Update()
    {
        transform.rotation = Quaternion.Euler(GetComponent<Rigidbody>().velocity);
    }
}
