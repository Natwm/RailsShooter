using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UiDisplay : MonoBehaviour
{
    public bool Jauge;
    public Sprite hpImage;
    public Vector3 VecUiSize;
    public int AmoutOfHp;
    int MaxHp;
    [Range(0,100)]
    public float Margin;
    Vector2 UiPos;

    List<GameObject> allHpObjects;

    // Start is called before the first frame update
    void Start()
    {
        MaxHp = AmoutOfHp;
        if (!Jauge)
        {
            SpawnUi();
        }
        else
        {

        }
        
    }
    void SpawnUi()
    {
        allHpObjects = new List<GameObject>();
        allHpObjects.Capacity = MaxHp;
        for (int i = 0; i < MaxHp; i++)
        {
            GameObject tempSprite = new GameObject();
            tempSprite.transform.parent = transform;
            tempSprite.transform.localPosition = UiPos;
            tempSprite.transform.localScale = VecUiSize;
            tempSprite.AddComponent<Image>().sprite = hpImage;
            tempSprite.name = (i + 1).ToString();
            UiPos.x += Margin;
            allHpObjects.Add(tempSprite);
        }
    }

    void UpdateUi_Int()
    {
        for (int i = 0; i < allHpObjects.Count; i++)
        {
            if (i <= AmoutOfHp)
            {
                allHpObjects[i].SetActive(true);
            }
            else
            {
                allHpObjects[i].SetActive(false);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        UpdateUi_Int();
    }
}
