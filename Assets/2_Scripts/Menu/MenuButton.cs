using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class MenuButton : MonoBehaviour
{
    public void Play() {
        MenuManager.instance.Play();
    }
    public void Return() {
        MenuManager.instance.Return();
    }

    public void SwapPage(MenuPage to) {
        MenuManager.instance.SwapUIMenu(to);
    }

    public void Leave() {
        MenuManager.instance.Leave();
    }
}
