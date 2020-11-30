using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(EnnemyBehaviours))]
public class EnnemiBehavioursEditor : Editor
{
    [DrawGizmo(GizmoType.Selected)]
    static void DrawPathInfos(EnnemyBehaviours target, GizmoType gizmoType)
    {
        // text area permet de dessiner le texte dans une "case" (plus lisible)
        GUI.skin.textArea.fontSize = 30;
        GUI.contentColor = Color.green;
        for (int i = 0; i < target.m_PositionHolderGO_Action.transform.childCount; i++)
        {
            Handles.Label(target.m_PositionHolderGO_Action.transform.GetChild(i).position, (i + 1).ToString(), GUI.skin.textArea);
        }

        for (int i = 0; i < target.m_PositionHolderGO_PreAction.transform.childCount; i++)
        {
            Handles.Label(target.m_PositionHolderGO_PreAction.transform.GetChild(i).position, (i+1).ToString(), GUI.skin.textArea);
        }

        Handles.Label(target.transform.position, target.GetCurrentState().ToString(), GUI.skin.textArea);
    }

}
