using System.Collections.Generic;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

public class GUIDrawUtil
{
    private ReorderableList dependeciesList;
    private bool foldDependeciesList = true;
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
    public string TabTextField(int space, string name, string Target)
    {
        string value = "";
        GUILayout.BeginHorizontal();
        GUILayout.Space(space);
        value = EditorGUILayout.TextField(name, Target);
        GUILayout.EndHorizontal();
        return value;
    }
    public void TabLabel(int space, string name)
    {
        GUILayout.BeginHorizontal();
        GUILayout.Space(space);
        EditorGUILayout.LabelField(name);
        GUILayout.EndHorizontal();
    }
    public void DependenciesList(List<Dependence> dependencies)
    {
        dependeciesList = new ReorderableList(dependencies, typeof(Dependence), true, true, true, true);

        dependeciesList.drawHeaderCallback = rect =>
        {
            EditorGUI.LabelField(rect, "List");
        };
        dependeciesList.drawElementCallback = (rect, index, isActive, isFocused) =>
        {
            var item = dependencies[index];
            rect.height = EditorGUIUtility.singleLineHeight;

            float half = rect.width * 0.5f;
            item.packageName = EditorGUI.TextField(
                new Rect(rect.x, rect.y + 2.5f, half - 5, rect.height), "PackageName",
                item.packageName
            );

            item.version = EditorGUI.TextField(
                new Rect(rect.x + half, rect.y + 2.5f, half, rect.height), "Version",
                item.version
            );
        };

        dependeciesList.onAddCallback = l =>
        {
            dependencies.Add(new Dependence());
        };

        dependeciesList.onRemoveCallback = l =>
        {
            dependencies.RemoveAt(l.index);
        };
    }
    public void DoDependenciesList(int space)
    {
        GUILayout.BeginHorizontal();
        GUILayout.Space(space);
        foldDependeciesList = EditorGUILayout.Foldout(foldDependeciesList, "Dependencies");
        GUILayout.EndHorizontal();
        if (foldDependeciesList)
        {
            GUILayout.BeginHorizontal();
            GUILayout.Space(space * 2);
            dependeciesList.DoLayoutList();
            GUILayout.EndHorizontal();
        }
    }
}
