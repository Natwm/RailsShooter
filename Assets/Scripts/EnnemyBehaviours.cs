﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnnemyBehaviours : MonoBehaviour
{
    private List<Vector3> listOfPosition = new List<Vector3>();
    public GameObject m_PositionHolderGO;

    [Space]
    [SerializeField] private float speed;
    [SerializeField] private float rotationSpeed;

    [Space]
    [SerializeField] private float minDistance;
    [SerializeField] private float minDistancePlayer;

    [SerializeField] private float waitTime;

    private PlayerController player;
    private WeaponManager weaponManager;

    private int m_MouvementIndex;
    private int m_ActionIndex;

    private bool canDoAction;
    private bool isRotating;

    private enum Status
    {
        NONE,
        STOP,
        HIDE,
        MOVE,
        SHOOT,
    }

    [SerializeField] private List<Status> listOfAction;

    private void Start()
    {
        m_ActionIndex = 0;
        m_MouvementIndex = 0;

        canDoAction = true;
        isRotating = false;

        SetListOfPOsition();

        player = FindObjectOfType<PlayerController>();
    }

    private void Update()
    {

        if (canDoAction || isRotating)
        {
            canDoAction = false;
            Debug.Log(listOfAction[m_ActionIndex]);
            Debug.Log(m_ActionIndex);
            DoAction(listOfAction[m_ActionIndex]);
            //EnnemyMovement();
            //CalculeRotation(listOfPosition[m_MouvementIndex]);
        }


    }

    void DoAction(Status action)
    {
        Debug.Log(action);
        switch (action)
        {
            case Status.NONE:
                EnnemyDoNothing();
                break;
            case Status.STOP:
                EnnemyStopAndReload();
                break;
            case Status.HIDE:
                EnnemyHide();
                break;
            case Status.MOVE:
                EnnemyMovement();
                break;
            case Status.SHOOT:
                EnnemyShoot();
                break;
            default:
                break;
        }
    }

    #region IA
    void EnnemyDoNothing()
    {
        Timer waitTimer = new Timer(waitTime, NewAction);
    }

    void EnnemyStopAndReload()
    {
        EnnemyDoNothing();
        weaponManager.Reload();
        Timer waitTimer = new Timer(waitTime, NewAction);
    }

    void EnnemyHide()
    {
        //play Animation hide
    }

    void EnnemyMovement()
    {
        CalculeRotation(listOfPosition[m_MouvementIndex]);
        transform.position = Vector3.Lerp(transform.position, listOfPosition[m_MouvementIndex], speed * 1 * Time.deltaTime);
        if (Vector3.Distance(transform.position, listOfPosition[m_MouvementIndex]) < minDistance)
        {
            m_MouvementIndex = NewAction(m_MouvementIndex);
            NewAction();
        }
        else
        {
            canDoAction = true;
        }
        
    }

    void EnnemyShoot()
    {
        CalculeRotation(player.transform.position);
        if (!isRotating)
        {
            Debug.Log("shoot");
            //weaponManager.Shoot();
            Timer waitTimer = new Timer(waitTime, NewAction);
        }
    }
    #endregion

    void PlayerDetection()
    {
    /*if (Vector3.Distance(transform.position, player.transform.position) <= minDistancePlayer)
            Debug.Log("shoot");*/
    }

    public void CalculeRotation(Vector3 position)
    {
        isRotating = true;

        Quaternion TargetRotation = Quaternion.LookRotation(position - transform.position);
        transform.rotation = Quaternion.Slerp(transform.rotation, TargetRotation, rotationSpeed * Time.deltaTime);

        if(1 - Mathf.Abs(Quaternion.Dot(transform.rotation, TargetRotation)) < 1f)
            isRotating = false;

    }

    int NewAction(int index)
    {
        index++;
        if (index >= listOfPosition.Count)
            index = 0;

        return index;
    }

    void NewAction()
    {
        m_ActionIndex++;
        if (m_ActionIndex >= listOfPosition.Count)
            m_ActionIndex = 0;

        Debug.Log(listOfPosition.Count);
        canDoAction = true;
    }

    #region Getter && Setter

    void SetListOfPOsition()
    {
        for (int i = 0; i < m_PositionHolderGO.transform.childCount; i++)
        {
            listOfPosition.Add(m_PositionHolderGO.transform.GetChild(i).position);
        }
    }
    #endregion

    #region Gizmo
    private void OnDrawGizmos()
    {
        for (int i = 1; i < m_PositionHolderGO.transform.childCount; i++)
        {
            Gizmos.DrawLine(m_PositionHolderGO.transform.GetChild(i - 1).position, m_PositionHolderGO.transform.GetChild(i).position);
        }
    }
    #endregion
}