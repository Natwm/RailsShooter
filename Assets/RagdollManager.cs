using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RagdollManager : MonoBehaviour
{
    public bool isDebug;
    Rigidbody[] AllRigids;
    Collider[] AllColliders;
    public Rigidbody mainRigid;
    
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
        foreach (Rigidbody item in AllRigids)
        {
            item.isKinematic = true;
        }
        
        if(Head != null)
        {
            Head.enabled = true;
        }
    }

    public void Ragdoll(Vector3 _impact)
    {
        Body.enabled = false;

        Destroy(mainRigid);
        foreach (Collider item in AllColliders)
        {
            item.enabled = true;
        }
        foreach (Rigidbody item in AllRigids)
        {
            item.isKinematic = false;
            item.useGravity = true;
            item.AddExplosionForce(10f, _impact, 1f,1f,ForceMode.Impulse);
        }
        mainAnimation.enabled = false;
    }

    private void Update()
    {
        if (Input.GetKey(KeyCode.Space) && isDebug)
        {
            Ragdoll(impact.transform.position);
        }
    }
}
