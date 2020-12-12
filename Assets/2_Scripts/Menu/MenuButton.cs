using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class MenuButton : MonoBehaviour
{
    [Space]
    [Header("SOUND")]
    protected FMOD.Studio.EventInstance selectEffect;
    [FMODUnity.EventRef] [SerializeField] private string selectSound;

    protected FMOD.Studio.EventInstance hoverEffect;
    [FMODUnity.EventRef] [SerializeField] private string hoverSound;


    private void Start()
    {
        selectEffect = FMODUnity.RuntimeManager.CreateInstance(selectSound);
        FMODUnity.RuntimeManager.AttachInstanceToGameObject(selectEffect, transform, GetComponentInParent<Rigidbody>());

        hoverEffect = FMODUnity.RuntimeManager.CreateInstance(hoverSound);
        FMODUnity.RuntimeManager.AttachInstanceToGameObject(hoverEffect, GetComponent<Transform>(), GetComponentInParent<Rigidbody>());

    }

    public void Play() {
        selectEffect.start();
        FindObjectOfType<Intro>().introScreenEffect.setParameterValue("start",1f);
        MenuManager.instance.Play();
    }

    public void Return() {
        selectEffect.start();
        MenuManager.instance.Return();
    }

    public void SwapPage(MenuPage to) {
        selectEffect.start();
        MenuManager.instance.SwapUIMenu(to);
    }

    public void Leave() {
        selectEffect.start();
        MenuManager.instance.Leave();
    }

    public void HoverSound()
    {
        hoverEffect.start();
    }
}
