using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor ( typeof ( GUIObject ) )]
public class GUIObjectEditor : Editor {

    public override void OnInspectorGUI () {
        GUIObject myGUIObject = ( GUIObject ) target;

        EditorGUILayout.LabelField ( "GuiName" ,myGUIObject.GuiName );
        EditorGUILayout.LabelField ( "AssetBundlePath" , myGUIObject.AssetBundlePath );
    }
}
