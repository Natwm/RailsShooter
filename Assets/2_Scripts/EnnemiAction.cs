using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class EnnemiAction 
{
    public enum Status
    {
        NONE,
        STOP,
        HIDE_UP,
        HIDE_DOWN,
        MOVE,
        RUSH,
        HIDE_LEFT,
        HIDE_RIGHT,
        SHOOT,
    }
    [Tooltip(" L'action à effectué")]
    public Status ennemiState;

    [Min(1)]
    [Tooltip(" Modifie le temps d'attente lors de l'action 'HIDE_LEFT', 'HIDE_RIGHT', 'STOP' et 'NONE'")]
    public float waitTime;

    [Space]
    [Header ("Movement Speed")]
    public bool overrideSpeed = false;
    [Min(1)]
    [Tooltip (" Modifie la vitesse de l'ennemi lors de l'action 'MOVE' et 'RUSH'")]
    public float speed = 1f;

    [Space]
    [Header("Rotation Speed")]
    public bool overrideRotationSpeed = false;
    [Min(1)]
    [Tooltip(" Modifie la vitesse de rotation de l'ennemi Lors de 'SHOOT' et 'MOVE'")]
    public float rotationSpeed = 1f;

    [Space]
    [Header("Shoot")]
    [Min(1)]
    [Tooltip(" Nombre de fois que l'ennemi shoot lors des action 'SHOOT' et 'RUSH'")]
    public int amountOfShoot = 1;
}
