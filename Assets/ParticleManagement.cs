using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMOD;

public class ParticleManagement : MonoBehaviour
{

    public ParticleSystem shoot;

    FMOD.Studio.EventInstance Bang;
    [FMODUnity.EventRef] [SerializeField] private string BangSound;

    FMOD.Studio.EventInstance Bang_Auto;
    [FMODUnity.EventRef] [SerializeField] private string AutoSound;

    FMOD.Studio.EventInstance Reload;
    [FMODUnity.EventRef] [SerializeField] private string ReloadSound;

    public ParticleSystem douille;

    public ParticleSystem shoot_aotu;
    public ParticleSystem douillasse;

    public EnnemyWeaponsBehaviours weapon;

    public void PlayShoot()
    {
        shoot.Play();
        Bang.start();
        douille.Play();
    }

    public void PlayShootAuto()
    {
        shoot_aotu.Play();
        Bang_Auto.start();
        douillasse.Play();
    }

    public void PlayReload()
    {
        Reload.start();
    }

    public void EnemyShoot()
    {
        shoot.gameObject.SetActive(true);
        shoot.Play();
        Bang.start();

        weapon.Shoot(GetComponent<Animator>(), Vector3.zero);
    }

    void Start()
    {
        Bang = FMODUnity.RuntimeManager.CreateInstance(BangSound);
        FMODUnity.RuntimeManager.AttachInstanceToGameObject(Bang,transform, GetComponentInParent<Rigidbody>());

        Bang_Auto = FMODUnity.RuntimeManager.CreateInstance(AutoSound);
        FMODUnity.RuntimeManager.AttachInstanceToGameObject(Bang_Auto, transform, GetComponentInParent<Rigidbody>());

        Reload = FMODUnity.RuntimeManager.CreateInstance(ReloadSound);
        FMODUnity.RuntimeManager.AttachInstanceToGameObject(Reload, transform, GetComponentInParent<Rigidbody>());
    }


}
