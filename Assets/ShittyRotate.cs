using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShittyRotate : MonoBehaviour
{
    public float RotateSpeed;
    public Vector3 Rotate;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(Rotate*RotateSpeed);
    }
}
