using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnnemyBehaviours : HealthManager
{

    [Space]
    [Header("Status")]
    [SerializeField] private bool loopAction;

    [Space]
    [Header("Position GO")]
    private List<Vector3> listOfPosition_Action = new List<Vector3>();
    private List<Vector3> listOfPosition_Ronde = new List<Vector3>();
    public GameObject m_PositionHolderGO_Action;
    public GameObject m_PositionHolderGO_PreAction;
    public GameObject colliders;

    [Space]
    [Header("Distance morte")]
    [SerializeField] private float minDistance;
    [SerializeField] private float minDistancePlayer;

    [SerializeField] private float waitTime;

    [Space]
    [Header ("Var")]
    [SerializeField] private float ennemiSpeed;
    [SerializeField] private float ennemiRotationSpeed;
    [SerializeField] private int NBShoot_remaning;

    private PlayerController player;
    private WeaponManager weaponManager;

    private int m_MouvementIndex;
    private int m_ActionIndex;
    private int m_PreActionIndex;

    private bool canDoAction;
    private bool isRotating;

    private EnnemyWeaponsBehaviours weapon;

    private NavMeshAgent agent;

    Timer hitWaitTimer;
    Timer waitShoot;

    [Space]
    [Header ("IA")]
    [Tooltip (" This parameter content all action the ennemie can do before the actione phase ")]
    [SerializeField] private List<EnnemiAction> listOfPreAction;
    [SerializeField] private List<EnnemiAction> listOfAction;

    public List<EnnemiAction> ListOfPreAction { get => listOfPreAction; set => listOfPreAction = value; }
    public List<EnnemiAction> ListOfAction { get => listOfAction; set => listOfAction = value; }

    private void Start()
    {
        m_ActionIndex = 0;
        m_MouvementIndex = 0;

        canDoAction = true;
        isRotating = false;

        SetListOfPOsition();

        //retirer des parents

        player = FindObjectOfType<PlayerController>();
        weapon = GetComponentInChildren<EnnemyWeaponsBehaviours>();

        animator = GetComponentInChildren<Animator>();

        GameManager.instance.IncreaseAmountofEnnemy();

        agent = GetComponent<NavMeshAgent>();

        hitWaitTimer = new Timer(.5f);

        waitShoot = new Timer(weapon.m_FireRate);
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

    void DoAction(EnnemiAction action, List<Vector3> positions)
    {
        Debug.Log(action);
        switch (action.ennemiState)
        {
            case EnnemiAction.Status.NONE:
                EnnemyDoNothing(action);
                break;
            case EnnemiAction.Status.STOP:
                EnnemyStopAndReload(action);
                break;
            case EnnemiAction.Status.HIDE_UP:
                EnnemyHideUp();
                break;
            case EnnemiAction.Status.HIDE_DOWN:
                break;
            case EnnemiAction.Status.MOVE:
                EnnemyMovement(positions, action);
                break;
            case EnnemiAction.Status.RUSH:
                Rush(action);
                break;
            case EnnemiAction.Status.HIDE_LEFT:
                HideLeft(action);
                break;
            case EnnemiAction.Status.HIDE_RIGHT:
                HideRight(action);
                break;
            case EnnemiAction.Status.SHOOT:
                EnnemyShoot(action);
                break;
            default:
                break;
        }
    }

    #region IA

    void EnnemyDoNothing(EnnemiAction action)
    {
        if (animator.GetBool("IsWalking"))
            animator.SetBool("IsWalking", false);
        //Debug.Log("EnnemyDoNothing");
        if (GameManager.instance.canAction)
        {
            Timer stopTimerAction = new Timer(action.waitTime, NewAction);
            stopTimerAction.Play();
        }
        else
        {
            Timer stopTimerPreAction = new Timer(action.waitTime, NewPreAction);
            stopTimerPreAction.Play();
        }
    }

    void EnnemyStopAndReload(EnnemiAction action)
    {
        Debug.Log("reload");
        EnnemyDoNothing(action);
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

    void HideLeft(EnnemiAction action)
    {
        if (animator.GetBool("IsWalking"))
            animator.SetBool("IsWalking", false);

        if (!animator.GetBool("IsHidingLeft"))
            animator.SetBool("IsHidingLeft", true);

        EnnemyDoNothing(action);
    }

    void HideRight(EnnemiAction action)
    {
        if (animator.GetBool("IsWalking"))
            animator.SetBool("IsWalking", false);

        if (!animator.GetBool("IsHidingRight"))
            animator.SetBool("IsHidingRight", true);

        Debug.Log(action.waitTime);
        EnnemyDoNothing(action);
    }

    void Rush(EnnemiAction action)
    {
        if (action.overrideSpeed)
            ennemiSpeed = action.speed;

        agent.speed = ennemiSpeed;

        if (Vector3.Distance(transform.position, player.gameObject.transform.position) < minDistancePlayer)
        {
            if (animator.GetBool("IsWalking"))
                animator.SetBool("IsWalking", false);
            agent.SetDestination(transform.position);
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

    void Roulade(EnnemiAction action)
    {
        if (!animator.GetBool("IsWalking"))
            animator.SetBool("IsWalking", true);

        CalculeRotation(listOfPosition_Action[m_MouvementIndex], action);
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

    void EnnemyMovement(List<Vector3> listOfPosition, EnnemiAction action)
    {
        if (action.overrideSpeed)
            ennemiSpeed = action.speed;

        agent.speed = ennemiSpeed;

        if (animator.GetBool("IsHidingLeft"))
            animator.SetBool("IsHidingLeft", false);

        if (animator.GetBool("IsHidingRight"))
            animator.SetBool("IsHidingRight", false);

        if (!animator.GetBool("IsWalking"))
            animator.SetBool("IsWalking", true);

        CalculeRotation(listOfPosition[m_MouvementIndex], action);
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

    void EnnemyShoot(EnnemiAction action)
    {
        if (animator.GetBool("Trigger_Walk"))
            animator.SetBool("Trigger_Walk", false);
        CalculeRotation(player.transform.position, action);

        if (!isRotating && (waitShoot.IsFinished() || waitShoot.IsPaused()))
        {
            Debug.Log("shoot");
            animator.SetTrigger("Trigger_Shoot");

            NBShoot_remaning--;

            if(NBShoot_remaning > 0)
            {
                weapon.Shoot(animator, Vector3.zero);
                canDoAction = true;
                waitShoot.ResetPlay();
            }
            else
            {
                NewPreAction();
            }
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

    public void CalculeRotation(Vector3 position, EnnemiAction action)
    {
        if (action.overrideRotationSpeed)
            ennemiRotationSpeed = action.rotationSpeed;

        isRotating = true;

        Quaternion TargetRotation = Quaternion.LookRotation(position - transform.position);
        transform.rotation = Quaternion.Slerp(transform.rotation, TargetRotation, ennemiRotationSpeed * Time.deltaTime);

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

        switch (listOfAction[m_ActionIndex].ennemiState)
        {
            case EnnemiAction.Status.SHOOT:
                NBShoot_remaning = listOfAction[m_ActionIndex].amountOfShoot;
                break;

            default:
                break;
        }

        canDoAction = true;
        
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

        switch (listOfPreAction[m_PreActionIndex].ennemiState)
        {
            case EnnemiAction.Status.SHOOT:
                NBShoot_remaning = listOfPreAction[m_PreActionIndex].amountOfShoot;
                break;

            default:
                break;
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
        colliders.SetActive(false);

        GameManager.instance.DecreaseAmountofEnnemy();
        Destroy(this.gameObject,1);
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

        m_PositionHolderGO_Action.transform.parent = null;
        m_PositionHolderGO_PreAction.transform.parent = null;
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

    public EnnemiAction.Status GetCurrentState()
    {
        return listOfPreAction[m_PreActionIndex].ennemiState;
    }
}
