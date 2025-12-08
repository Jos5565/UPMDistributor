using System;
using System.IO;
using System.Linq;
using System.Reflection;
using Codice.Client.BaseCommands.Merge.Restorer;
using Codice.CM.Client.Gui;
using Unity.Android.Gradle.Manifest;
using UnityEditor;
using UnityEngine;

public class UMPDistributor : EditorWindow
{
    private UMPDistributorManifast manifast;
    private Vector2 wholeScrollPos;
    private string filePath;
    private bool foldoutGroupA = true;
    private GitPusher gitPusher;

    [MenuItem("UMP Publish/UMP Distributor")]
    public static void ShowWindow()
    {
        EditorWindow.GetWindow(typeof(UMPDistributor), false, "UMP Distributor");
    }
    private void OnEnable()
    {
        manifast = AssetDatabase.LoadAssetAtPath<UMPDistributorManifast>("Assets/UMPDistributor/UMPDistributorManifast.asset");
        gitPusher = new GitPusher();
    }
    private void OnGUI()
    {
        wholeScrollPos = EditorGUILayout.BeginScrollView(wholeScrollPos, GUILayout.ExpandWidth(true), GUILayout.ExpandHeight(true)); // 스크롤 시작

        DrawSourceFolder();
        EditorGUILayout.Space(10);
        DrawCreateDefaultProjectButton();
        EditorGUILayout.Space(10);
        DrawExportFolder();
        EditorGUILayout.Space(10);
        DrawPackageJsonOptions();
        EditorGUILayout.Space(10);
        DrawPublishButton();
        EditorGUILayout.EndScrollView(); // 스크롤 끝
    }

    private void DrawSourceFolder()
    {
        EditorGUILayout.BeginHorizontal();
        manifast.SourcePath = EditorGUILayout.TextField("Package Source Path", manifast.SourcePath);
        if (GUILayout.Button("Open"))
        {
            manifast.SourcePath = EditorUtility.OpenFolderPanel("Select Folder", "", "");
            filePath = Path.Combine(manifast.SourcePath, "package.json");
            if (!File.Exists(filePath))
            {
                PackageJson json = new PackageJson();
                manifast.packageJson = json;
                File.WriteAllText(filePath, JsonUtility.ToJson(json));
                Debug.Log("파일이 없습니다.");
            }
            else
            {
                manifast.packageJson = JsonUtility.FromJson<PackageJson>(File.ReadAllText(filePath));
            }
        }
        EditorGUILayout.EndHorizontal();
    }
    private void DrawCreateDefaultProjectButton()
    {
        if (!string.IsNullOrEmpty(manifast.SourcePath)) return;
        if (GUILayout.Button("Create DefaultProject", GUILayout.Height(30)))
        {
            string defFolder = "Assets/UPMDefaultProject";
            System.IO.Directory.CreateDirectory(defFolder);

            //Create package.json
            string packageJsonPath = Path.Combine(defFolder, "package.json");
            PackageJson json = new PackageJson();
            manifast.packageJson = json;
            File.WriteAllText(packageJsonPath, JsonUtility.ToJson(json));

            //Create README.md
            string readMePath = Path.Combine(defFolder, "README.md");
            File.WriteAllText(readMePath, "");
            //Create Runtime Folder
            string runtimeFolderPath = Path.Combine(defFolder, "Runtime");
            System.IO.Directory.CreateDirectory(runtimeFolderPath);

            AssetDatabase.Refresh();

            manifast.SourcePath = Path.GetFullPath(defFolder);
        }
    }
    private void DrawExportFolder()
    {
        EditorGUILayout.BeginHorizontal();
        manifast.PackagePath = EditorGUILayout.TextField("Package Export Path", manifast.PackagePath);
        if (GUILayout.Button("Open"))
        {
            manifast.PackagePath = EditorUtility.OpenFolderPanel("Select Folder", "", "");
        }
        EditorGUILayout.EndHorizontal();
    }

    private void DrawPackageJsonOptions()
    {
        if (string.IsNullOrEmpty(manifast.SourcePath) || string.IsNullOrEmpty(filePath)) return;
        if (manifast.isPackageJson)
        {
            foldoutGroupA = EditorGUILayout.Foldout(foldoutGroupA, "package.json");
            if (foldoutGroupA)
            {
                manifast.packageJson.name = DrawHorizontalSpaceTextField(20, "Name", manifast.packageJson.name);
                manifast.packageJson.version = DrawHorizontalSpaceTextField(20, "Version", manifast.packageJson.version);
                manifast.packageJson.displayName = DrawHorizontalSpaceTextField(20, "DisplayName", manifast.packageJson.displayName);
                manifast.packageJson.description = DrawHorizontalSpaceTextField(20, "Description", manifast.packageJson.description);
                manifast.packageJson.unity = DrawHorizontalSpaceTextField(20, "Unity", manifast.packageJson.unity);
                DrawHorizontalSpaceLabel(20, "Author");
                manifast.packageJson.author.name = DrawHorizontalSpaceTextField(40, "Name", manifast.packageJson.author.name);
                manifast.packageJson.author.email = DrawHorizontalSpaceTextField(40, "Email", manifast.packageJson.author.email);
            }
        }
    }
    private string DrawHorizontalSpaceTextField(int space, string name, string Target)
    {
        string value = "";
        GUILayout.BeginHorizontal();
        GUILayout.Space(space);
        value = EditorGUILayout.TextField(name, Target);
        GUILayout.EndHorizontal();
        return value;
    }
    private void DrawHorizontalSpaceLabel(int space, string name)
    {
        GUILayout.BeginHorizontal();
        GUILayout.Space(space);
        EditorGUILayout.LabelField(name);
        GUILayout.EndHorizontal();
    }

    private void DrawPublishButton()
    {
        if (string.IsNullOrEmpty(manifast.SourcePath)) return;
        string filePath = Path.Combine(manifast.SourcePath, "package.json");
        if (GUILayout.Button("Publish", GUILayout.Height(30)))
        {
            File.WriteAllText(filePath, JsonUtility.ToJson(manifast.packageJson));
            gitPusher.Run(manifast);

            AssetDatabase.Refresh();
        }
    }

}
