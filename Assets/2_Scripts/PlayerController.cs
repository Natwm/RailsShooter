using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class PlayerController : HealthManager
{
    public GameObject positionHolder;

    private int positionIndex = 0;

    [SerializeField] private PlayerPosition[] condition;

    private bool canMove = false;
    

    private void Start()
    {
        hitSoundEffect = FMODUnity.RuntimeManager.CreateInstance(hitSound);
        FMODUnity.RuntimeManager.AttachInstanceToGameObject(hitSoundEffect, GetComponent<Transform>(), GetComponentInParent<Rigidbody>());

        deathSoundEffect = FMODUnity.RuntimeManager.CreateInstance(deathSound);
        FMODUnity.RuntimeManager.AttachInstanceToGameObject(deathSoundEffect, GetComponent<Transform>(), GetComponentInParent<Rigidbody>());
    }

    private void Update()
    {
        if(positionIndex <= condition.Length && GameManager.instance.amountOfKill >= condition[positionIndex].AmountOfEnnemiToKill)
        {
            transform.DOMove(condition[positionIndex].position.position, condition[positionIndex].moveSpeed);
            //transform.DORotate(condition[positionIndex].position.position, condition[positionIndex].moveSpeed);
            GameManager.instance.amountOfKill = 0;

            positionIndex++;
        }
    }

    protected override void Death(GameObject bullet)
    {
        deathSoundEffect.start();
        GameManager.instance.GameOver();
    }

    public override void DeacreseLife(int damage, GameObject Bullet)
    {
        animator.SetTrigger("Trigger_PlayerHit");
        m_AmountOfLive -= damage;

        GameManager.instance.UpdateAmountOfLife(m_AmountOfLive);

        if (m_AmountOfLive <= 0)
        {
            Death(Bullet);
        }
        else
        {
            Debug.Log("tet");
            hitSoundEffect.start();
        }

    }


    #region Gizmo
    private void OnDrawGizmos()
    {
        if (positionHolder != null)
        {
            for (int i = 1; i < positionHolder.transform.childCount; i++)
            {
                Gizmos.DrawLine(positionHolder.transform.GetChild(i - 1).position, positionHolder.transform.GetChild(i).position);
            }
        }

    }
    #endregion


}
