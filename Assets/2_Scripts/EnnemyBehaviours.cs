using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnnemyBehaviours : HealthManager
{

    private enum Status
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

    [Space]
    [Header("Status")]
    [SerializeField] private bool loopAction;

    [Space]
    [Header("Position GO")]
    private List<Vector3> listOfPosition_Action = new List<Vector3>();
    private List<Vector3> listOfPosition_Ronde = new List<Vector3>();
    public GameObject m_PositionHolderGO_Action;
    public GameObject m_PositionHolderGO_PreAction;

    [Space]
    [Header("Speed Variables")]
    [SerializeField] private float speed;
    [SerializeField] private float rotationSpeed;

    [Space]
    [Header("Distance morte")]
    [SerializeField] private float minDistance;
    [SerializeField] private float minDistancePlayer;

    [SerializeField] private float waitTime;

    private PlayerController player;
    private WeaponManager weaponManager;

    private int m_MouvementIndex;
    private int m_ActionIndex;
    private int m_PreActionIndex;

    private bool canDoAction;
    private bool isRotating;

    private EnnemyWeaponsBehaviours weapon;

    private NavMeshAgent agent;

    private Timer attackTimer;
    [SerializeField] private float timeBtwAttack;

    Timer waitTimer;
    Timer waitTimerpreAction;
    Timer hitWaitTimer;

    [Space]
    [Header ("IA")]
    [Tooltip (" This parameter content all action the ennemie can do before the actione phase ")]
    [SerializeField] private List<Status> listOfPreAction;
    [SerializeField] private List<Status> listOfAction;
   

    private void Start()
    {
        m_ActionIndex = 0;
        m_MouvementIndex = 0;

        canDoAction = true;
        isRotating = false;

        SetListOfPOsition();

        player = FindObjectOfType<PlayerController>();
        weapon = GetComponentInChildren<EnnemyWeaponsBehaviours>();

        animator = GetComponentInChildren<Animator>();

        GameManager.instance.IncreaseAmountofEnnemy();

        agent = GetComponent<NavMeshAgent>();

        attackTimer = new Timer(timeBtwAttack);
        waitTimer = new Timer(waitTime, NewAction);
        waitTimerpreAction = new Timer(waitTime, NewPreAction);
        hitWaitTimer = new Timer(.5f);
    }

    private void Update()
    {
        if (GameManager.instance.canAction)
        {
            if (canDoAction)
            {
                canDoAction = false;
                DoAction(listOfAction[m_ActionIndex], listOfPosition_Action);
            }
        }
        else
        {
            if (canDoAction)
            {
                canDoAction = false;
                DoAction(listOfPreAction[m_PreActionIndex], listOfPosition_Ronde);
            }
            
        }
        //EnnemyMovement();
        
    }

    void DoAction(Status action, List<Vector3> positions)
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
            case Status.HIDE_UP:
                EnnemyHideUp();
                break;
            case Status.HIDE_DOWN:
                break;
            case Status.MOVE:
                EnnemyMovement(positions);
                break;
            case Status.RUSH:
                Rush();
                break;
            case Status.HIDE_LEFT:
                HideLeft();
                break;
            case Status.HIDE_RIGHT:
                HideRight();
                break;
            case Status.SHOOT:
                EnnemyShoot();
                break;
            default:
                break;
        }
    }

    #region IA

    /*void EnnemyDoNothing()
    {
        if (animator.GetBool("IsWalking"))
            animator.SetBool("IsWalking", false);
        //Debug.Log("EnnemyDoNothing");
        if (!waitTimer.IsStarted())
        {
            Debug.Log("blabla pas lancé");
            waitTimer.Play();
            if (waitTimer.IsFinished())
                waitTimer.Reset();
        }
    }*/
    void EnnemyDoNothing()
    {
        if (animator.GetBool("IsWalking"))
            animator.SetBool("IsWalking", false);
        //Debug.Log("EnnemyDoNothing");
        if (GameManager.instance.canAction)
            waitTimer.ResetPlay();
        else
            waitTimerpreAction.ResetPlay();
    }

    void EnnemyStopAndReload()
    {
        Debug.Log("reload");
        EnnemyDoNothing();
        //weaponManager.Reload();
        //animator.SetTrigger("Trigger_Reload");

        /*Timer waitTimer = new Timer(waitTime, NewAction);
        waitTimer.Play();*/
    }

    void EnnemyHideUp()
    {
        if (animator.GetBool("IsWalking"))
            animator.SetBool("IsWalking", false);
        //play Animation hide
        Debug.Log("Hide");
        NewAction();
    }

    void EnnemyHideDown()
    {
        if (animator.GetBool("IsWalking"))
            animator.SetBool("IsWalking", false);
        //play Animation hide
        Debug.Log("Hide");
        NewAction();
    }

    void HideLeft()
    {
        if (animator.GetBool("IsWalking"))
            animator.SetBool("IsWalking", false);

        if (!animator.GetBool("IsHidingLeft"))
            animator.SetBool("IsHidingLeft", true);

        EnnemyDoNothing();
    }

    void HideRight()
    {
        if (animator.GetBool("IsWalking"))
            animator.SetBool("IsWalking", false);

        if (!animator.GetBool("IsHidingRight"))
            animator.SetBool("IsHidingRight", true);

        EnnemyDoNothing();
    }

    void Rush()
    {
        agent.speed *= 2.5f;
        if (Vector3.Distance(transform.position, player.gameObject.transform.position) < minDistancePlayer)
        {
            if (animator.GetBool("IsWalking"))
                animator.SetBool("IsWalking", false);
            agent.SetDestination(transform.position);
            attackTimer.ResetPlay();
            weapon.Shoot(animator, Vector3.zero);
        }
        else
        {
            if (!animator.GetBool("IsWalking"))
                animator.SetBool("IsWalking", true);
            agent.SetDestination(player.gameObject.transform.position);
        }
        canDoAction = true;
    }

    void Roulade()
    {
        if (!animator.GetBool("IsWalking"))
            animator.SetBool("IsWalking", true);

        CalculeRotation(listOfPosition_Action[m_MouvementIndex]);
        /*if (!isRotating)
            transform.position = Vector3.MoveTowards(transform.position, listOfPosition[m_MouvementIndex], speed * Time.deltaTime);
            //rb.velocity = Mathf.Lerp(rb.velocity.magnitude, speed, .9f) * (listOfPosition[m_MouvementIndex] - transform.position);*/

        agent.SetDestination(listOfPosition_Action[m_MouvementIndex]);

        if (Vector3.Distance(transform.position, listOfPosition_Action[m_MouvementIndex]) < minDistance)
        {
            //
            //animation roulade
            m_MouvementIndex = NewAction(m_MouvementIndex);
            NewAction();
        }
        else
        {
            canDoAction = true;
        }
    }

    void EnnemyMovement(List<Vector3> listOfPosition)
    {
        if (animator.GetBool("IsHidingLeft"))
            animator.SetBool("IsHidingLeft", false);

        if (animator.GetBool("IsHidingRight"))
            animator.SetBool("IsHidingRight", false);

        if (!animator.GetBool("IsWalking"))
            animator.SetBool("IsWalking", true);

        CalculeRotation(listOfPosition[m_MouvementIndex]);
        /*if (!isRotating)
            transform.position = Vector3.MoveTowards(transform.position, listOfPosition[m_MouvementIndex], speed * Time.deltaTime);
            //rb.velocity = Mathf.Lerp(rb.velocity.magnitude, speed, .9f) * (listOfPosition[m_MouvementIndex] - transform.position);*/

        agent.SetDestination(listOfPosition[m_MouvementIndex]);

        if (Vector3.Distance(transform.position, listOfPosition[m_MouvementIndex]) < minDistance)
        {
            m_MouvementIndex = NewAction(m_MouvementIndex);
            if (GameManager.instance.canAction)
                NewAction();
            else
                NewPreAction();
        }
        else
        {
            canDoAction = true;
        }

    }

    void EnnemyShoot()
    {
        if (animator.GetBool("Trigger_Walk"))
            animator.SetBool("Trigger_Walk", false);
        CalculeRotation(player.transform.position);

        if (!isRotating)
        {
            Debug.Log("shoot");
            animator.SetTrigger("Trigger_Shoot");
            weapon.Shoot(animator, Vector3.zero);
            Timer waitTimer = new Timer(waitTime, NewAction);
            waitTimer.Play();
        }
        else
        {
            canDoAction = true;
            Debug.Log("Rotation");
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

        if (1 - Mathf.Abs(Quaternion.Dot(transform.rotation, TargetRotation)) < .01f)
            isRotating = false;

    }

    int NewAction(int index)
    {
        /*index++;
        if (index >= listOfPosition_Action.Count)
            index = 0;

        return index;*/

        if (loopAction)
        {
            Debug.Log("loop");
            index++;
            if (index >= listOfPosition_Action.Count)
                index = 0;
        }
        else
        {
            if (index <= listOfPosition_Action.Count)
            {
                index++;
            }
            else
            {
                if (animator.GetBool("IsWalking"))
                    animator.SetBool("IsWalking", false);
            }
        }

        return index;
    }

    void NewAction()
    {
        Debug.Log("NewAction");
        m_ActionIndex++;
        if (m_ActionIndex >= listOfAction.Count)
            m_ActionIndex = 0;

        canDoAction = true;
        /*if (loopAction)
        {
            m_ActionIndex++;
            if (m_ActionIndex >= listOfAction.Count)
                m_ActionIndex = 0;
        }
        else
        {
            if (m_ActionIndex <= listOfAction.Count)
            {
                m_ActionIndex++;
            }
        }
        canDoAction = true;*/
    }

    void NewPreAction()
    {
        if (loopAction)
        {
            m_PreActionIndex++;
            if (m_PreActionIndex >= listOfPreAction.Count)
                m_PreActionIndex = 0;
        }
        else
        {
            if(m_PreActionIndex + 1 < listOfPreAction.Count)
            {
                m_PreActionIndex++;
            }
            else
            {
                if (animator.GetBool("IsWalking"))
                    animator.SetBool("IsWalking", false);
            }
        }
        canDoAction = true;

        /*m_PreActionIndex++;
        if (m_PreActionIndex >= listOfPreAction.Count)
            m_PreActionIndex = 0;

        */
    }

    public override void DeacreseLife(int damage)
    {
        m_AmountOfLive -= damage;

        if (m_AmountOfLive <= 0)
        {
            Death();
        }
        else
        {
            hitSoundEffect.start();
            animator.SetTrigger("Trigger_Hit");
            agent.SetDestination(transform.position);
            hitWaitTimer.ResetPlay();
        }
    }

    protected override void Death()
    {
        animator.SetTrigger("Trigger_Die");

        GameManager.instance.DecreaseAmountofEnnemy();
        Destroy(this.gameObject, 2);
    }

    #region Getter && Setter

    void SetListOfPOsition()
    {
        for (int i = 0; i < m_PositionHolderGO_Action.transform.childCount; i++)
        {
            listOfPosition_Action.Add(m_PositionHolderGO_Action.transform.GetChild(i).position);
        }

        for (int i = 0; i < m_PositionHolderGO_PreAction.transform.childCount; i++)
        {
            listOfPosition_Ronde.Add(m_PositionHolderGO_PreAction.transform.GetChild(i).position);
        }
    }
    #endregion

    #region Gizmo
    private void OnDrawGizmos()
    {
        for (int i = 1; i < m_PositionHolderGO_Action.transform.childCount; i++)
        {
            Gizmos.DrawLine(m_PositionHolderGO_Action.transform.GetChild(i - 1).position, m_PositionHolderGO_Action.transform.GetChild(i).position);
        }

        for (int i = 1; i < m_PositionHolderGO_PreAction.transform.childCount; i++)
        {
            Gizmos.DrawLine(m_PositionHolderGO_PreAction.transform.GetChild(i - 1).position, m_PositionHolderGO_PreAction.transform.GetChild(i).position);
        }
    }
    #endregion
}
