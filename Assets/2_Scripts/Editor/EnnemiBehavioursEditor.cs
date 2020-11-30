using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(EnnemyBehaviours))]
public class EnnemiBehavioursEditor : Editor
{

    private void OnSceneGUI()
    {
        EnnemyBehaviours t = target as EnnemyBehaviours;
        EnnemiAction.Status ennemiStatus = t.GetCurrentState();

        for (int i = 0; i < t.m_PositionHolderGO_PreAction.transform.childCount; i++)
        {
            Handles.Label(t.m_PositionHolderGO_PreAction.transform.GetChild(i).position, i.ToString());
        }

        for (int i = 0; i < t.m_PositionHolderGO_Action.transform.childCount; i++)
        {
            Handles.Label(t.m_PositionHolderGO_Action.transform.GetChild(i).position, i.ToString());

        }

        //Handles.Label(t.transform.position, ennemiStatus.ToString());
    }

}
