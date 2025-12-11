using System.Collections.Generic;
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
    public List<Sample> samples;
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
    public string ToJson(List<Dependence> list)
    {
        string json = string.Empty;
        list.ForEach(a =>
        {
            if (!dependencies.ContainsKey(a.packageName))
            {
                dependencies.Add(a.packageName, a.version);
            }
        });
        json = JsonConvert.SerializeObject(this);
        return json;
    }
    public void FromJson()
    {

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

