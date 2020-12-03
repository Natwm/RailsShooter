using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class PlayerPosition 
{
    public Transform stepRotation;

    public Vector3 stepPosition;

    [Min (1)]
    public int AmountOfEnnemiToKill = 1;

    [Min(1)]
    public float moveSpeed = 1f;
}
