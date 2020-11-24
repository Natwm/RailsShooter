using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CurseurBehaviours : MonoBehaviour
{
    [SerializeField] private float mouseSpeed;

     private Vector3 lastPosition;

    [SerializeField] private float sensibility = 15f;

    public bool followMouse = false;

    private void Start()
    {
        Cursor.visible = false;
    }

    // Update is called once per frame
    void Update()
    {
        lastPosition = transform.position;

        if (followMouse)
            transform.position = Input.mousePosition;
        else
            MoveToCurseur();

        CalculeLastPos();

        if (Input.GetKeyDown(KeyCode.A))
        {
            TiredMove();
        }
    }

    void MoveToCurseur()
    {
        Vector3 pos = Vector3.MoveTowards(transform.position, Input.mousePosition, mouseSpeed);
        float distanceBtwVector = Vector3.Distance(Input.mousePosition, lastPosition);
        Debug.Log(distanceBtwVector);

        if (distanceBtwVector >= sensibility)
        {
            transform.position = Vector3.MoveTowards(transform.position, Input.mousePosition, mouseSpeed);
        }
    }

    void CalculeLastPos()
    {

    }

    void TiredMove()
    {
        Debug.Log("Move");
    }
}
