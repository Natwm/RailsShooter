using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : HealthManager
{
    protected override void Death()
    {
        deathSoundEffect.start();
        GameManager.instance.GameOver();
    }
}
