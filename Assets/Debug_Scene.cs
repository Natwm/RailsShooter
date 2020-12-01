using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Debug_Scene : MonoBehaviour
{
    public Animator animatorToTest;
    public string Trigger;
    public GameObject boom;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyUp(KeyCode.R))
        {
            SceneManager.LoadScene("TestDa_Simon");
        }
        if (Input.GetKeyUp(KeyCode.Space))
        {
            animatorToTest.SetTrigger(Trigger);
            boom.SetActive(true);
        }
    }
}
