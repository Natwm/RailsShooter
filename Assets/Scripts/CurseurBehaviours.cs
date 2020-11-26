using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class CurseurBehaviours : MonoBehaviour
{
    #region Param
    [Space]
    [Header("Mouse Param")]
    [SerializeField] private float mouseSpeed;

    [SerializeField] private float deathZoneRadiusInPixel = 15f;

    [Space]
    [Header("Shaking Param")]
    [SerializeField] private float strengthShake = 2f;


    [Space]
    [Header("Recule Param")]
    private float m_ReculeMultiplicator = 2f;
    private float m_ReculeShakeStrength = 2f;
    private float m_ReculeShakeDuration = 2f;
    private float m_ReculeDuration = .5f;

    public bool followMouse = false;
    private bool isMoving = false;
    private Vector3 lastPosition;

    private Timer tiredTimer;
    #endregion


    private void Start()
    {
        Cursor.visible = false;
        tiredTimer = new Timer(1f, WeaponShake);
    }

    // Update is called once per frame
    void Update()
    {

        if (Input.GetKeyDown(KeyCode.E))
        {
            WeaponRecul();
        }

        lastPosition = transform.position;

        if (followMouse)
            transform.position = Input.mousePosition;
        else
            MoveToCurseur();

        if (!isMoving)
        {
            if (!tiredTimer.IsStarted())
            {
                tiredTimer.ResetPlay();
            }

        }
        else
        {
            tiredTimer.Pause();
        }
    }

    void MoveToCurseur()
    {
        Vector3 pos = Vector3.MoveTowards(transform.position, Input.mousePosition, mouseSpeed);
        float distanceBtwVector = Vector3.Distance(Input.mousePosition, lastPosition);

        if (distanceBtwVector >= deathZoneRadiusInPixel)
        {
            DOTween.Kill("tiredShake");
            transform.position = Vector3.MoveTowards(transform.position, Input.mousePosition, mouseSpeed);
            isMoving = true;
        }
        else
        {
            isMoving = false;
        }
    }

    public void WeaponShake()
    {
        transform.DOShakePosition(50000f, strengthShake).SetId("tiredShake"); ;
    }

    void WeaponRecul()
    {
        transform.DOShakePosition(m_ReculeShakeDuration, m_ReculeShakeStrength);
        transform.DOMoveY(transform.position.y + m_ReculeMultiplicator, m_ReculeDuration);
    }

    public void UpdateReculParam(float reculeMultiplicator, float reculeShakeStrength, float reculeShakeDuration, float reculeDuration)
    {
        m_ReculeMultiplicator = reculeMultiplicator;
        m_ReculeShakeStrength = reculeShakeStrength;
        m_ReculeShakeDuration = reculeShakeDuration;
        m_ReculeDuration = reculeDuration;
    }

}
