using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using DG.Tweening;

public class MenuManager : MonoBehaviour {
  public static MenuManager instance;
  public GameObject startingMenuPage;
  private MenuPage currentlyActiveMenuPage;
  private MenuPage currentOverlay;

  private Stack<MenuPage> overlays = new Stack<MenuPage>();

  void Start() {
    SetStaticInstance();
    currentlyActiveMenuPage = startingMenuPage.GetComponent<MenuPage>();

    foreach(MenuPage m in FindObjectsOfType<MenuPage>())
    {
      m.gameObject.SetActive(false);
    }    
    
    currentlyActiveMenuPage.gameObject.SetActive(true);
    Camera.main.transform.position = currentlyActiveMenuPage.cameraTransform.position;
    Camera.main.transform.rotation = currentlyActiveMenuPage.cameraTransform.rotation;
  }

  private void SetStaticInstance () {
    if (instance == null) {
      instance = this;
    }
    else 
      Debug.Log("There are multiple MenuManager instances");
  }

  public void SwapUIMenu( MenuPage to ) {
    if (to.isOverlay) {
      overlays.Push(to);
      DisplayMenuPage(to);
    }
    else {
      RemoveMenuPage(currentlyActiveMenuPage);
      DisplayMenuPage(to);
      currentlyActiveMenuPage = to;

      Camera.main.transform.DOMove(to.cameraTransform.position, 2);
      Camera.main.transform.DORotate(to.cameraTransform.rotation.eulerAngles, 2);
    }
  }

  public void Return() {
    if (overlays.Count != 0)
      RemoveMenuPage(overlays.Pop());
    else if (currentlyActiveMenuPage.returnMenu != null) {
      SwapUIMenu(currentlyActiveMenuPage.returnMenu);
    } 
  }

  private void RemoveMenuPage(MenuPage page) {
    page.gameObject.SetActive(false);
    if(!page.isOverlay) {
      foreach(MenuPage overlay in overlays.ToArray())
        RemoveMenuPage(overlay);
      overlays.Clear();
    }
  }

  private void DisplayMenuPage(MenuPage page) {
    page.gameObject.SetActive(true);
  }

  public void Play() {
    GameManager.instance.LoadGame(); 
  }

  public void Leave() {
    Application.Quit();
  }
}
