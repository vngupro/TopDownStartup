using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class WallsPaletteEditorWin : EditorWindow
{
    public Object source;
    [MenuItem ("Window/My Window")]
    
    static void Init()
    {
        var window = GetWindowWithRect<WallsPaletteEditorWin>(new Rect(0, 0, 800, 800));
        window.Show();
    }

    void OnGUI()
    {

        for (int i = 0; i < 3; i++)
        {
            GUILayout.BeginHorizontal();
            source = EditorGUILayout.ObjectField(source, typeof(Object), true, GUILayout.Width(100), GUILayout.Height(100), GUILayout.ExpandWidth(false));
            if(i != 1) source = EditorGUILayout.ObjectField(source, typeof(Object), true, GUILayout.Width(100), GUILayout.Height(100), GUILayout.ExpandWidth(false));
            source = EditorGUILayout.ObjectField(source, typeof(Object), true, GUILayout.Width(100), GUILayout.Height(100), GUILayout.ExpandWidth(false));
            GUILayout.Space(10);
            GUILayout.EndHorizontal();
            GUILayout.Space(10);
        }
        
    }
}
