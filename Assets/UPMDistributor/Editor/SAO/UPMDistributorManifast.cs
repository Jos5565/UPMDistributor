using System.Collections.Generic;
using System.Security;
using Unity.Plastic.Newtonsoft.Json;
using UnityEditor.PackageManager.UI;
using UnityEngine;

[CreateAssetMenu(fileName = "UPMDistributorManifast", menuName = "UPM Publish/UPMDistributorManifast")]
public class UPMDistributorManifast : ScriptableObject
{
    public string PackagePath;
    public string SourcePath;
    public PackageJson packageJson;
    public List<Dependence> dependencies;
    public List<Sample> samples;
}
[System.Serializable]
public class PackageJson
{
    public string name;
    public string version;
    public string displayName;
    public string description;
    public string unity;
    public Dictionary<string, string> dependencies;
    [HideInInspector] public List<Sample> samples;
    public Author author;


    public PackageJson()
    {
        name = "com.yourname.mypackage";
        version = "1.0.0";
        displayName = "My Custom Package";
        description = "My Unity Package";
        unity = "6000.0";
        author = new Author { name = "your Name", email = "you@mail.com" };
        dependencies = new Dictionary<string, string>();
        samples = new List<Sample>();
    }
    public string ToJson(UPMDistributorManifast manifast)
    {
        string json = string.Empty;
        dependencies.Clear();
        manifast.dependencies.ForEach(a =>
        {
            if (!dependencies.ContainsKey(a.packageName))
            {
                dependencies.Add(a.packageName, a.version);
            }
        });
        this.samples = manifast.samples;

        json = JsonConvert.SerializeObject(this);
        return json;
    }
    public void FromJson(string json, UPMDistributorManifast manifast)
    {
        PackageJson pj = JsonConvert.DeserializeObject<PackageJson>(json);
        // Reset before Loading Data
        manifast.dependencies.Clear();
        manifast.samples.Clear();

        foreach (KeyValuePair<string, string> item in pj.dependencies)
        {
            Dependence dependence = new Dependence { packageName = item.Key, version = item.Value };
            manifast.dependencies.Add(dependence);
        }
        manifast.samples.AddRange(pj.samples);

        manifast.packageJson = pj;
    }
}
[System.Serializable]
public class Author
{
    public string name;
    public string email;
}
[System.Serializable]
public class Dependence
{
    public string packageName;
    public string version;
}
[System.Serializable]
public class Sample
{
    public string displayName;
    public string description;
    public string path;
}

