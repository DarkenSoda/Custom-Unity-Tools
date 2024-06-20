#if UNITY_EDITOR

using UnityEditor;
using UnityEngine;
using UnityEditor.PackageManager;
using UnityEditor.PackageManager.Requests;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DarkenSoda.CustomTools.EditorScripts
{
    public static class Setup
    {
        [MenuItem("Tools/Setup/Create Initial Folders")]
        public static void CreateInitialFolders()
        {
            Folders.CreateDirectories("Game", "Animation", "Art", "Art/Environment", "Art/Models", "Materials",
                "Prefabs", "Scripts/ScriptableObjects", "Scripts/UI", "ScriptableObjects", "Scenes", "Settings");

            AssetDatabase.Refresh();
        }

        public static void InstallInitialPackages(List<string> packages)
        {
            Packages.InstallPackages(packages.ToArray());
        }

        static class Folders
        {
            internal static void CreateDirectories(string root, params string[] directories)
            {
                string fullpath = Path.Combine(Application.dataPath, root);
                DirectoryInfo currentDirectory = new DirectoryInfo(fullpath);
                if (!currentDirectory.Exists)
                {
                    currentDirectory.Create();
                }
                foreach (var directory in directories)
                {
                    currentDirectory.CreateSubdirectory(directory);
                }
            }
        }

        static class Packages
        {
            static AddRequest request;
            static Queue<string> packageQueue = new();

            internal static void InstallPackages(params string[] packages)
            {
                // TODO: Check if packages are already installed
                foreach (var package in packages)
                {
                    packageQueue.Enqueue(package);
                }

                InstallNextPackage();
            }

            private static async void Progress()
            {
                if (request.IsCompleted)
                {
                    if (request.Status == StatusCode.Success)
                        Debug.Log("Installed: " + request.Result.packageId);
                    else if (request.Status == StatusCode.Failure)
                        Debug.Log(request.Error.message);

                    EditorApplication.update -= Progress;

                    if (packageQueue.Count > 0)
                    {
                        await Task.Delay(1000);
                        InstallNextPackage();
                    }
                }
            }

            private static void InstallNextPackage()
            {
                if (packageQueue.Count > 0)
                {
                    request = Client.Add(packageQueue.Dequeue());
                    EditorApplication.update += Progress;
                }
            }
        }
    }
}

#endif
