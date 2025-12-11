using System;
using System.IO;
using System.Linq;
using System.Reflection;
using Codice.Client.BaseCommands.Merge.Restorer;
using Codice.CM.Client.Gui;
using Unity.Android.Gradle.Manifest;
using UnityEditor;
using UnityEngine;

public class UPMDistributor : EditorWindow
{
    private UPMDistributorManifast manifast;
    private GUIDrawUtil drawUtil;
    private Vector2 wholeScrollPos;
    private string packageJsonPath;
    private bool foldoutGroupA = true;
    private GitPusher gitPusher;
    private string packageJsonFileName = "package.json";

    [MenuItem("UPM Publish/UPM Distributor")]
    public static void ShowWindow()
    {
        EditorWindow.GetWindow(typeof(UPMDistributor), false, "UPM Distributor");
    }
    private void OnEnable()
    {
        manifast = AssetDatabase.LoadAssetAtPath<UPMDistributorManifast>("Assets/UPMDistributor/UPMDistributorManifast.asset");
        gitPusher = new GitPusher();
        drawUtil = drawUtil == null ? new GUIDrawUtil() : drawUtil;
        drawUtil.DependenciesList(manifast.dependencies);
        drawUtil.SampleList(manifast.packageJson.samples);
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
            packageJsonPath = Path.Combine(manifast.SourcePath, packageJsonFileName);
            if (!File.Exists(packageJsonPath))
            {
                PackageJson json = new PackageJson();
                manifast.packageJson = json;
                File.WriteAllText(packageJsonPath, JsonUtility.ToJson(json));
                Debug.Log("파일이 없습니다.");
            }
            else
            {
                manifast.packageJson = JsonUtility.FromJson<PackageJson>(File.ReadAllText(packageJsonPath));
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
            packageJsonPath = Path.Combine(defFolder, packageJsonFileName);
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
        if (string.IsNullOrEmpty(manifast.SourcePath)) return;
        if (string.IsNullOrEmpty(packageJsonPath)) packageJsonPath = Path.Combine(manifast.SourcePath, packageJsonFileName);
        if (manifast.isPackageJson)
        {
            foldoutGroupA = EditorGUILayout.Foldout(foldoutGroupA, "package.json");
            if (foldoutGroupA)
            {
                manifast.packageJson.name = drawUtil.TabTextField(20, "Name", manifast.packageJson.name);
                manifast.packageJson.version = drawUtil.TabTextField(20, "Version", manifast.packageJson.version);
                manifast.packageJson.displayName = drawUtil.TabTextField(20, "DisplayName", manifast.packageJson.displayName);
                manifast.packageJson.description = drawUtil.TabTextField(20, "Description", manifast.packageJson.description);
                manifast.packageJson.unity = drawUtil.TabTextField(20, "Unity", manifast.packageJson.unity);
                //Dependencies
                drawUtil.DoDependenciesList(20);
                //Samples
                drawUtil.DoSampleList(20);

                drawUtil.TabLabel(20, "Author");
                manifast.packageJson.author.name = drawUtil.TabTextField(40, "Name", manifast.packageJson.author.name);
                manifast.packageJson.author.email = drawUtil.TabTextField(40, "Email", manifast.packageJson.author.email);
            }
        }
    }


    private void DrawPublishButton()
    {
        if (string.IsNullOrEmpty(manifast.SourcePath)) return;

        if (GUILayout.Button("Publish", GUILayout.Height(30)))
        {
            File.WriteAllText(packageJsonPath, manifast.packageJson.ToJson(manifast.dependencies));
            // gitPusher.Run(manifast);

            AssetDatabase.Refresh();
        }
    }

}
