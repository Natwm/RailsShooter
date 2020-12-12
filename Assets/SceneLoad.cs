﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class SceneLoad : MonoBehaviour
{
    public string SceneToLoad;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void LoadScene()
    {
        GameManager.instance.introScreenEffect.setParameterValue("start", 1f);
        SceneManager.LoadScene(SceneToLoad);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
