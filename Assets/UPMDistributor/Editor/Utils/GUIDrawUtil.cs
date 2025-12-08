using UnityEditor;
using UnityEngine;

public class GUIDrawUtil
{
    public void DrawOpenFolder(string fieldName, string pathTarget, string buttonName)
    {
        EditorGUILayout.BeginHorizontal();
        pathTarget = EditorGUILayout.TextField(fieldName, pathTarget);
        if (GUILayout.Button(buttonName))
        {
            pathTarget = EditorUtility.OpenFolderPanel("Select Folder", "", "");
        }
        EditorGUILayout.EndHorizontal();
    }
}
