using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(Gun))]
public class GunEditor : Editor
{
    int selected = 0;
    string[] toolbar = { "All", "Stats", "Position", "FX" };
    
    SerializedProperty damage;
    SerializedProperty RPM;
    SerializedProperty magSize;
    SerializedProperty FiremodeOptions;
    SerializedProperty semi, full, burst, gunPos, aimPos;
    SerializedProperty reload, ads;

    SerializedProperty muzzleF;

    protected virtual void OnEnable()
    {
        damage = serializedObject.FindProperty("damage");
        RPM = serializedObject.FindProperty("RPM");
        magSize = serializedObject.FindProperty("magSize");
        FiremodeOptions = serializedObject.FindProperty("SelectFire");
        semi = serializedObject.FindProperty("semi");
        full = serializedObject.FindProperty("full");
        burst = serializedObject.FindProperty("burst");
        reload = serializedObject.FindProperty("reloadSpeed");
        ads = serializedObject.FindProperty("adsSpeed");

        gunPos = serializedObject.FindProperty("gunPos");
        aimPos = serializedObject.FindProperty("aimPos");

        muzzleF = serializedObject.FindProperty("muzzleFlash");


    }

    public override void OnInspectorGUI()
    {
        
        Gun gun = (Gun)target;

        selected = GUILayout.Toolbar(selected, toolbar);
        switch (selected) {
            case 0:
                base.OnInspectorGUI();
                break;

            case 1:

                serializedObject.Update();               

                float dps = damage.floatValue * RPM.floatValue / 60;
                float TTK = (Mathf.Ceil(100 / damage.floatValue)) / (RPM.floatValue / 60);
                ProgressBar(dps / 1000, "DPS - "+ dps.ToString("0"));                
                ProgressBar(0.1f / TTK, "TTK - " + TTK.ToString("0.000") + " Seconds");

                EditorGUILayout.PropertyField(damage, new GUIContent("Damage", "Set the weapon's damage value"));
                EditorGUILayout.PropertyField(RPM, new GUIContent("RPM", "Set the weapon's damage value"));
                EditorGUILayout.PropertyField(magSize, new GUIContent("Magazine Size", "Set the weapon's damage value"));

                EditorGUILayout.PropertyField(reload, new GUIContent("Reload Speed", "Set the time it takes to reload"));
                EditorGUILayout.PropertyField(ads, new GUIContent("ADS Speed", "Set the time it takes to ADS"));

                EditorGUILayout.Space();
                EditorGUILayout.LabelField("Firemodes");
                EditorGUILayout.PropertyField(FiremodeOptions, new GUIContent("Firemode Default", "Set the default firemode"));
                EditorGUILayout.PropertyField(semi, new GUIContent("Semi-Auto", "Is the weapon is allowed to fire in semi-auto?"));
                EditorGUILayout.PropertyField(full, new GUIContent("Full Auto", "Is the weapon is allowed to fire in full auto?"));
                EditorGUILayout.PropertyField(burst, new GUIContent("Burst Fire", "Is the weapon is allowed to fire in burst fire?"));

                


                
                serializedObject.ApplyModifiedProperties();
                break;

            case 2:

                serializedObject.Update();

                EditorGUILayout.PropertyField(gunPos, new GUIContent("Default Position", "Set the defualt weapon position"));
                EditorGUILayout.PropertyField(aimPos, new GUIContent("Aim Position", "Set the weapon's aim position"));

                serializedObject.ApplyModifiedProperties();
                break;

            case 3:
                serializedObject.Update();

                EditorGUILayout.ObjectField(muzzleF, new GUIContent("Muzzle Flash", "Set the muzzle flash prefab the gun should use"));                       

                serializedObject.ApplyModifiedProperties();
                break;

            default:
                base.OnInspectorGUI();
                break;              
        }


        GUILayout.BeginHorizontal();

        

   
        GUILayout.EndHorizontal();

    }

    void ProgressBar(float value, string label)
    {
        // Get a rect for the progress bar using the same margins as a textfield:
        Rect rect = GUILayoutUtility.GetRect(18, 18, "TextField");
        EditorGUI.ProgressBar(rect, value, label);
        EditorGUILayout.Space();
    }
}