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

    public RagdollManager m_Ragdoll;

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

        waitShoot = new Timer(weapon.m_FireRate + 1);

        hitSoundEffect = FMODUnity.RuntimeManager.CreateInstance(hitSound);
        FMODUnity.RuntimeManager.AttachInstanceToGameObject(hitSoundEffect, GetComponent<Transform>(), GetComponentInParent<Rigidbody>());

        deathSoundEffect = FMODUnity.RuntimeManager.CreateInstance(deathSound);
        FMODUnity.RuntimeManager.AttachInstanceToGameObject(deathSoundEffect, GetComponent<Transform>(), GetComponentInParent<Rigidbody>());
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
                EnnemyHideDown();
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

    void SetState(bool standUp, bool walking, bool hidingRight, bool hidingLeft)
    {
        animator.SetBool("StandUp", standUp);
        animator.SetBool("IsWalking", walking);
        animator.SetBool("IsHidingRight", hidingRight);
        animator.SetBool("IsHidingLeft", hidingLeft);
    }

    void ResetState()
    {
        SetState(true, false, false, false);
    }

    #region IA

    void EnnemyDoNothing(EnnemiAction action)
    {
        ResetState();
        
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
        ResetState();
        //play Animation hide
        Debug.Log("Hide");
        NewAction();
    }

    void EnnemyHideDown()
    {
        SetState(false, false, false, false);
        
        //play Animation hide
        NewAction();
    }

    void HideLeft(EnnemiAction action)
    {
        SetState(true, false, false, true);

        EnnemyDoNothing(action);
    }

    void HideRight(EnnemiAction action)
    {
        SetState(false, false, true, false);


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
            ResetState();
            agent.SetDestination(transform.position);
            animator.SetTrigger("Trigger_Shoot");
            // weapon.Shoot(animator, Vector3.zero);
        }
        else
        {
            SetState(true, true, false, false);
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

        SetState(true, true, false, false);


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
        SetState(true, false, false, false);
        CalculeRotation(player.transform.position, action);

        if (!isRotating && (waitShoot.IsFinished() || waitShoot.IsPaused()))
        {
            if(NBShoot_remaning > 0)
            {
                NBShoot_remaning--;
                animator.SetTrigger("Trigger_Shoot");
                //weapon.Shoot(animator, Vector3.zero);
                canDoAction = true;
                waitShoot.ResetPlay();
            }
            else
            {
                NewAction();
            }
        }
        else
        {
            canDoAction = true;
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

        if (loopAction)
        {
            Debug.Log("loop");
            index++;
            if (index >= listOfPosition_Action.Count)
                index = 0;
        }
        else
        {
            if (index < listOfPosition_Action.Count)
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

    public override void DeacreseLife(int damage, GameObject bullet)
    {
        m_AmountOfLive -= damage;

        if (m_AmountOfLive <= 0)
        {
            Death(bullet);
        }
        else
        {
            Debug.Log("aie");

            hitSoundEffect.start();

            animator.SetTrigger("Trigger_Hit");
            agent.SetDestination(transform.position);
            hitWaitTimer.ResetPlay();
        }
    }

    protected override void Death(GameObject Bullet)
    {
        if(!TimeController.instance.IsSlowMotion())
            TimeController.instance.increaseSlowMoTime();

        //animator.SetTrigger("Trigger_Die");
        deathSoundEffect.start();
        colliders.SetActive(false);

        Destroy(m_PositionHolderGO_Action);
        Destroy(m_PositionHolderGO_PreAction);

        GameManager.instance.DecreaseAmountofEnnemy();
        m_Ragdoll.Ragdoll(Bullet.transform.position);

        hitWaitTimer.Pause();
        waitShoot.Pause();
        TimeController.instance.MaintainSlowMotion();

        Destroy(agent);
        Destroy(gameObject, 1f);
        
        
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
        if(m_PositionHolderGO_Action != null)
        {
            for (int i = 1; i < m_PositionHolderGO_Action.transform.childCount; i++)
            {
                Gizmos.DrawLine(m_PositionHolderGO_Action.transform.GetChild(i - 1).position, m_PositionHolderGO_Action.transform.GetChild(i).position);
            }
        }
        if(m_PositionHolderGO_PreAction != null)
        {
            for (int i = 1; i < m_PositionHolderGO_PreAction.transform.childCount; i++)
            {
                Gizmos.DrawLine(m_PositionHolderGO_PreAction.transform.GetChild(i - 1).position, m_PositionHolderGO_PreAction.transform.GetChild(i).position);
            }
        }
    }
    #endregion

    public EnnemiAction.Status GetCurrentState()
    {
        return listOfPreAction[m_PreActionIndex].ennemiState;
    }
}
