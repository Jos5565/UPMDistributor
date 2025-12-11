using System;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;

public class IOUtil
{
    private UPMDistributorManifast manifast;
    public string packageJsonPath;
    private string samplePath;
    private string packageJsonFileName = "package.json";
    public IOUtil(UPMDistributorManifast manifast)
    {
        this.manifast = manifast;
    }
    public bool SetupSourcePackage()
    {
        try
        {
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
                // Load PackageJson from folder
                manifast.packageJson.FromJson(File.ReadAllText(packageJsonPath), manifast);

            }
            return true;
        }
        catch (Exception ex)
        {
            Debug.LogError(ex.ToString());
            return false;
        }

    }
    public bool CreateDefalut()
    {
        try
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
            manifast.SourcePath = Path.GetFullPath(defFolder);
            //Create Sample Folder
            samplePath = Path.Combine(defFolder, "Sample");
            System.IO.Directory.CreateDirectory(samplePath);
            AssetDatabase.Refresh();
            return true;

        }
        catch (Exception ex)
        {
            Debug.LogError(ex.ToString());
            return false;
        }
    }
    public bool TryOpenPackageJsonDef()
    {
        if (string.IsNullOrEmpty(manifast.SourcePath))
        {
            return false;
        }
        if (string.IsNullOrEmpty(packageJsonPath))
        {
            packageJsonPath = Path.Combine(manifast.SourcePath, packageJsonFileName);
        }
        return true;
    }
    public bool TrySample()
    {
        try
        {
            if (string.IsNullOrEmpty(samplePath)) samplePath = Path.Combine(manifast.SourcePath, "Sample");
            DirectoryInfo info = new DirectoryInfo(samplePath);
            var directories = info.EnumerateDirectories();
            if (directories.Count() <= 0)
            {
                return false;
            }
            else
            {
                manifast.samples.Clear();
                foreach (DirectoryInfo i in directories)
                {
                    Sample sample = new Sample
                    {
                        displayName = i.Name,
                        description = "",
                        path = $"Sample~/{i.Name}"
                    };
                    manifast.samples.Add(sample);
                }
                return true;
            }
        }
        catch (Exception ex)
        {
            Debug.LogError(ex.ToString());
            return false;
        }
    }

}

