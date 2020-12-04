using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponManager : MonoBehaviour
{
    #region STATIC
    public static WeaponManager instance;
    #endregion

    #region PARAM
    [SerializeField] private List<WeaponsBehaviours> m_ListOfWeapon = new List<WeaponsBehaviours>();
    [SerializeField] public GameObject CursorGO;

    [SerializeField] private float weaponSwapDuration;
    [SerializeField] [Range (0.1f, 5)] private float rotationSpeed;
    [SerializeField] private Animator weaponAnimator;

    #endregion

    private Timer weaponSwapTimer;
    public WeaponsBehaviours currentWeapon;
    public Vector3 direction;

    public Animator WeaponAnimator { get => weaponAnimator; set => weaponAnimator = value; }
    public bool hasWeaponOut = false;


    private void Awake()
    {
        if(instance != null)
            Debug.LogWarning("Multiple instance of same Singleton : WeaponManager");
        instance = this;

        currentWeapon = m_ListOfWeapon[0];
        weaponSwapTimer = new Timer(weaponSwapDuration);
    }

    public void SwapWeaponUp()
    {
        int index = m_ListOfWeapon.IndexOf(currentWeapon);
        index ++;
        
        if(index == m_ListOfWeapon.Count)
            index = 0;

        SwapWeapon(m_ListOfWeapon[index]);
    }

    public void SwapWeaponDown()
    {
        int index = m_ListOfWeapon.IndexOf(currentWeapon);
        index --;
        
        if(index == -1)
            index = m_ListOfWeapon.Count - 1;

        SwapWeapon(m_ListOfWeapon[index]);
    }

    private void SwapWeapon(WeaponsBehaviours weapon)
    {
        if (weapon.name == "AR")
            weaponAnimator.SetTrigger("Trigger_SwitchToAuto");
        else
            weaponAnimator.SetTrigger("Trigger_SwitchToGun");

        weaponSwapTimer.ResetPlay();
        currentWeapon.UnEquip();
        weapon.Equip();
        currentWeapon = weapon;
        GameManager.instance.UpdateAmountOfBulltes(weapon.currentNumberOfBullets);
        Debug.Log("Swap Weapon (" + currentWeapon.name + ") : Play Sound here");
    }

    public WeaponsBehaviours GetCurrentWeapon()
    {
        return currentWeapon;
    }

    public void Shoot()
    {
        if(weaponSwapTimer.IsStarted() == false && hasWeaponOut == true)
        {
            if (!GameManager.instance.canAction)
            {
                GameManager.instance.canAction = true;
                GameManager.instance.SetMusiqueOn();
            }
                

            if (!TimeController.instance.SlowMotionTimer.IsStarted() && !TimeController.instance.isActivedOnce)
            {
                TimeController.instance.StartSlowMotion();
            }

            currentWeapon.Shoot(weaponAnimator, direction);
        }
        else
        {
            if(hasWeaponOut == false)
            {
                TakeWeapon();
            }
        }
    }

    public void TakeWeapon()
    {
        weaponAnimator.SetTrigger("Trigger_ShowWeapon");
    }

    public void Reload()
    {
        currentWeapon.Reload();
    }

    void Update(){
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(new Vector3 (Input.mousePosition.x, Input.mousePosition.y,5));

        direction = mousePosition - Camera.main.transform.position;
        
        transform.localRotation = Quaternion.Euler(new Vector3(- direction.y, direction.x, 0) * rotationSpeed);

        if (Input.GetMouseButton(0))
            Shoot();
        
        if(Input.GetKeyDown("e"))
            SwapWeaponDown();
        
        if(Input.GetKeyDown("r"))
            Reload();

    }
}
