using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : HealthManager
{
    [SerializeField] protected Animator animator;
    protected override void Death()
    {
        deathSoundEffect.start();
        GameManager.instance.GameOver();
    }
}
