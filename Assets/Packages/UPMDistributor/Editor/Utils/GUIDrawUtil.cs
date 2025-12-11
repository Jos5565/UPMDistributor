using System.Collections.Generic;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

public class GUIDrawUtil
{
    private ReorderableList dependeciesList;
    private bool foldDependeciesList = true;
    private ReorderableList sampleList;
    private bool foldsampleList = true;
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
            EditorGUI.LabelField(rect, "Dependence List");
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
        GUILayout.Space(5);
        if (foldDependeciesList)
        {
            GUILayout.BeginHorizontal();
            GUILayout.Space(space * 2);
            dependeciesList.DoLayoutList();
            GUILayout.EndHorizontal();
        }
    }
    public void SampleList(List<Sample> samples)
    {
        sampleList = new ReorderableList(samples, typeof(Sample), true, true, true, true);
        sampleList.drawHeaderCallback = rect =>
        {
            EditorGUI.LabelField(rect, "Sample List");
        };
        sampleList.drawElementCallback = (rect, index, isActive, isFocused) =>
        {
            var item = samples[index];
            float line = EditorGUIUtility.singleLineHeight;
            float spacing = 3f;

            Rect r1 = new Rect(rect.x, rect.y + spacing, rect.width, line);
            Rect r2 = new Rect(rect.x, rect.y + line + spacing * 2, rect.width, line);
            Rect r3 = new Rect(rect.x, rect.y + (line * 2) + spacing * 3, rect.width, line);
            item.displayName = EditorGUI.TextField(r1, "DisplayName", item.displayName);
            item.description = EditorGUI.TextField(r2, "Description", item.description);
            item.path = EditorGUI.TextField(r3, "Path", item.path);
        };
        sampleList.elementHeightCallback = index =>
        {
            float line = EditorGUIUtility.singleLineHeight;
            float spacing = 4f;
            return line * 3 + spacing * 4;
        };

        sampleList.onAddCallback = l =>
        {
            samples.Add(new Sample());
        };

        sampleList.onRemoveCallback = l =>
        {
            samples.RemoveAt(l.index);
        };
    }
    public void DoSampleList(int space, IOUtil iOUtil)
    {
        GUILayout.BeginHorizontal();
        GUILayout.Space(space);
        foldsampleList = EditorGUILayout.Foldout(foldsampleList, "Samples");
        if (GUILayout.Button("Load Samples", GUILayout.Height(20)))
        {
            // TODO : On Load from ioUtil
            iOUtil.TrySample();
        }
        GUILayout.EndHorizontal();
        GUILayout.Space(5);
        if (foldsampleList)
        {
            GUILayout.BeginHorizontal();
            GUILayout.Space(space * 2);
            sampleList.DoLayoutList();
            GUILayout.EndHorizontal();
        }
    }
}
