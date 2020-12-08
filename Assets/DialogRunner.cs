using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DialogRunner : MonoBehaviour
{
    public TMP_Text TitleText;
    public TMP_Text SubText;
    public TMP_Text[] DialogTexts;
    public int DialogIndex;
    public float textShift;
    public int waitTime;
    // Start is called before the first frame update
    void Awake()
    {
        DialogTexts = GetComponentsInChildren<TMP_Text>();
        foreach (TMP_Text item in DialogTexts)
        {
            item.CrossFadeAlpha(0f, 0.001f, false);
        }
        TitleText.CrossFadeAlpha(0, 0.001f, false);
        SubText.CrossFadeAlpha(0, 0.001f, false);


    }

    private void Start()
    {
        Invoke("TitleScreen",waitTime);
    }

    public void TitleScreen()
    {
        TitleText.CrossFadeAlpha(1, 0.01f, false);
        SubText.CrossFadeAlpha(1, 1f,false);
    }

    public void NewLine()
    {
        if (DialogIndex < DialogTexts.Length)
        {
        if (DialogIndex >= 1)
        {
            TMP_Text previousLine = DialogTexts[DialogIndex - 1];
            previousLine.CrossFadeAlpha(0.5f, 0.1f, true);
            
        }
         if (DialogIndex >= 2)
        {
            TMP_Text oldLine = DialogTexts[DialogIndex - 2];
            
            oldLine.CrossFadeAlpha(0f, 0.1f, true);
        }
        if (DialogIndex > 0)
        {
                Vector3 currentPosition = transform.position;
                Vector3 nextPosition = new Vector3(currentPosition.x, currentPosition.y + textShift, currentPosition.z);
                transform.position = nextPosition;
        }
        
        DialogTexts[DialogIndex].CrossFadeAlpha(1, 0.1f, true);
        DialogIndex++;
        }
        else
        {

            DialogTexts[DialogIndex-1].CrossFadeAlpha(0, 0.2f, true);
            DialogTexts[DialogIndex-2].CrossFadeAlpha(0, 0.1f, true);
        }
    }

    // Update is called once per frame
    void Update()
    {
       
        if (Input.GetKeyUp(KeyCode.Space))
        {
            TitleText.CrossFadeAlpha(0, 0.1f, true);
            SubText.CrossFadeAlpha(0, 0.1f, true);
            
            NewLine();
        }
    }
}
