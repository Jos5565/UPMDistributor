using System.Diagnostics;
using System.Text;
using System.Threading;
using UnityEngine;

public class GitPusher
{
    private UPMDistributorManifast manifast;
    public void Run(UPMDistributorManifast manifast)
    {
        this.manifast = manifast;
        if (string.IsNullOrEmpty(manifast.PackagePath))
        {
            UnityEngine.Debug.LogError("❌ Package Path is Null");
            return;
        }
        if (string.IsNullOrEmpty(manifast.SourcePath))
        {
            UnityEngine.Debug.LogError("❌ Source Path is Null");
            return;
        }
#if UNITY_EDITOR_OSX
        MacBash(ReplaceCommned());
        MacBash(GitFetchPull());
        MacBash(GitAdd());
        MacBash(GitCommit());
        MacBash(GitPush());
#endif
    }

    private void MacBash(string command)
    {
        Process process = new Process();
        process.StartInfo.FileName = "/bin/bash";
        process.StartInfo.Arguments = $"-c \"{command}\"";
        process.StartInfo.UseShellExecute = false;
        process.StartInfo.RedirectStandardOutput = true;
        process.StartInfo.RedirectStandardError = true;
        process.StartInfo.CreateNoWindow = false;

        StringBuilder output = new StringBuilder();

        process.OutputDataReceived += (s, e) =>
        {
            if (!string.IsNullOrEmpty(e.Data))
                output.AppendLine(e.Data);
        };

        process.ErrorDataReceived += (s, e) =>
        {
            if (!string.IsNullOrEmpty(e.Data))
                output.AppendLine("❌ " + e.Data);
        };

        process.Start();
        process.BeginOutputReadLine();
        process.BeginErrorReadLine();
        process.WaitForExit();

        UnityEngine.Debug.Log("✅ Result:\n" + output);
    }
    private string ReplaceCommned()
    {
        string command = $"rsync -av --delete --exclude='.git' --exclude='.gitignore' --exclude='.gitattributes'  --exclude='README.md' \"{manifast.SourcePath}/.\" \"{manifast.PackagePath}\"";
        return command;
    }
    private string GitFetchPull()
    {
        string command = $"cd {manifast.PackagePath} && git fetch && git pull";
        return command;
    }

    private string GitAdd()
    {
        string command = $"cd {manifast.PackagePath} && git add .";
        return command;
    }

    private string GitCommit()
    {
        StringBuilder sb = new StringBuilder();
        sb.Append("git commit -m ");
        sb.Append("\"");
        sb.Append($"PackageVersion{manifast.packageJson.version}");
        sb.Append("\"");
        string msg = sb.ToString();
        string command = $"cd {manifast.PackagePath} && {msg}";
        return command;
    }

    private string GitPush()
    {
        string command = $"cd {manifast.PackagePath} && {"git push -f"}";
        return command;
    }


}
