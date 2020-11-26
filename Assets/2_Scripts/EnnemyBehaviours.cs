using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnnemyBehaviours : HealthManager
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

    private EnnemyWeaponsBehaviours weapon;
    private Animator m_animator;

    private enum Status
    {
        NONE,
        STOP,
        HIDE,
        MOVE,
        SHOOT,
    }

    [SerializeField] private List<Status> listOfAction;

    public Animator Animator { get => m_animator; set => m_animator = value; }

    private void Start()
    {
        m_ActionIndex = 0;
        m_MouvementIndex = 0;

        canDoAction = true;
        isRotating = false;

        SetListOfPOsition();

        player = FindObjectOfType<PlayerController>();
        weapon = GetComponentInChildren<EnnemyWeaponsBehaviours>();

        m_animator = GetComponentInChildren<Animator>();

        GameManager.instance.IncreaseAmountofEnnemy();
    }

    private void Update()
    {

        if (canDoAction || isRotating)
        {
            canDoAction = false;

            DoAction(listOfAction[m_ActionIndex]);
        }
    }

    void DoAction(Status action)
    {
        //Debug.Log(action);
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
        if (m_animator.GetBool("Trigger_Walk"))
            m_animator.SetBool("Trigger_Walk", false);
        //Debug.Log("EnnemyDoNothing");
        Timer waitTimer = new Timer(waitTime, NewAction);
        waitTimer.Play();
    }

    void EnnemyStopAndReload()
    {
        Debug.Log("reload");
        EnnemyDoNothing();
        weaponManager.Reload();
        Timer waitTimer = new Timer(waitTime, NewAction);
        waitTimer.Play();
    }

    void EnnemyHide()
    {
        if (m_animator.GetBool("Trigger_Walk"))
            m_animator.SetBool("Trigger_Walk", false);
        //play Animation hide
        Debug.Log("Hide");
        NewAction();
    }

    void EnnemyMovement()
    {
        if(!m_animator.GetBool("Trigger_Walk"))
            m_animator.SetBool("Trigger_Walk", true);

        CalculeRotation(listOfPosition[m_MouvementIndex]);
        if (!isRotating)
            transform.position = Vector3.MoveTowards(transform.position, listOfPosition[m_MouvementIndex], speed * Time.deltaTime);
            //rb.velocity = Mathf.Lerp(rb.velocity.magnitude, speed, .9f) * (listOfPosition[m_MouvementIndex] - transform.position);

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
        if (m_animator.GetBool("Trigger_Walk"))
            m_animator.SetBool("Trigger_Walk", false);
        CalculeRotation(player.transform.position);
        if (!isRotating)
        {
            Debug.Log("shoot");
            m_animator.SetTrigger("Trigger_Shoot");
            weapon.Shoot();
            Timer waitTimer = new Timer(waitTime, NewAction);
            waitTimer.Play();
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

        if(1 - Mathf.Abs(Quaternion.Dot(transform.rotation, TargetRotation)) < .1f)
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
        if (m_ActionIndex >= listOfAction.Count)
            m_ActionIndex = 0;


        canDoAction = true;
    }

    protected override void Death()
    {
        GameManager.instance.DecreaseAmountofEnnemy();
        Destroy(this.gameObject);
    }

    #region Getter && Setter

    void SetListOfPOsition()
    {
        for (int i = 0; i < m_PositionHolderGO.transform.childCount; i++)
        {
            listOfPosition.Add(m_PositionHolderGO.transform.GetChild(i).position);
            Vector3 pos = new Vector3(m_PositionHolderGO.transform.GetChild(i).position.x, 0, m_PositionHolderGO.transform.GetChild(i).position.z);
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
