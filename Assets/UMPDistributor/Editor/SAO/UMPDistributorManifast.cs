using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "UMPDistributorManifast", menuName = "UMP Publish/UMPDistributorManifast")]
public class UMPDistributorManifast : ScriptableObject
{
    public string PackagePath;
    public string SourcePath;
    public PackageJson packageJson;
    public bool isPackageJson => !string.IsNullOrEmpty(packageJson.name);

}
[System.Serializable]
public class PackageJson
{
    public string name;
    public string version;
    public string displayName;
    public string description;
    public string unity;
    public Author author;

    public PackageJson()
    {
        name = "com.yourname.mypackage";
        version = "1.0.0";
        displayName = "My Custom Package";
        description = "My Unity Package";
        unity = "6000.0";
        author = new Author { name = "your Name", email = "you@mail.com" };
    }
}
[System.Serializable]
public class Author
{
    public string name;
    public string email;
}


