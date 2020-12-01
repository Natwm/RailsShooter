using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Debug_TestParticules : MonoBehaviour
{
    public GameObject tester;

    private void OnCollisionEnter(Collision collision)
    {
        GameObject newObj = Instantiate(tester);
        newObj.transform.position = collision.GetContact(0).point;
    }
}
