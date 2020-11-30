using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RagdollManager : MonoBehaviour
{
    Rigidbody[] AllRigids;
    Collider[] AllColliders;
    public Collider Body;
    public Collider Head;
    Animator mainAnimation;
    public GameObject impact;
    // Start is called before the first frame update
    void Start()
    {
        AllRigids = GetComponentsInChildren<Rigidbody>();
        mainAnimation = GetComponent<Animator>();
        AllColliders = GetComponentsInChildren<Collider>();
        foreach (Collider item in AllColliders)
        {
            item.enabled = false;
        }
        
    }

    public void Ragdoll(Vector3 _impact)
    {
        foreach (Collider item in AllColliders)
        {
            item.enabled = true;
        }
        foreach (Rigidbody item in AllRigids)
        {
            item.useGravity = true;
            item.AddExplosionForce(50f, _impact, 100f,0.8f,ForceMode.Impulse);
        }
        mainAnimation.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyUp(KeyCode.Space))
        {
            Ragdoll(impact.transform.position);
        }
    }
}
