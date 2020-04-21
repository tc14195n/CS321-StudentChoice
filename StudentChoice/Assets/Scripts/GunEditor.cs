using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(Gun))]
public class GunEditor : Editor
{

    public override void OnInspectorGUI()
    {
        //Available Firemodes

        GUILayout.BeginHorizontal();

        if (GUILayout.Button("Semi Auto"))
        {

        }
        if (GUILayout.Button("Full Auto"))
        {

        }
        if (GUILayout.Button("Burst"))
        {

        }
        GUILayout.EndHorizontal();

        base.OnInspectorGUI();
    }
}
