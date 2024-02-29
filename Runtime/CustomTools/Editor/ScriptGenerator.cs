#if UNITY_EDITOR

using System;
using System.IO;
using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace DarkenSoda.CustomTools.Editor {
    public static class ScriptGenerator {
        private static string fullpath;

        [InitializeOnLoadMethod]
        private static void InitializePath() {
            string currentDirectory = GetCurrentFileName();
            currentDirectory = Directory.GetParent(currentDirectory).FullName;
            
            fullpath = Path.Combine(currentDirectory, "ScriptTemplates");

            string GetCurrentFileName([System.Runtime.CompilerServices.CallerFilePath] string fileName = null) {
                return Path.GetDirectoryName(fileName);
            }
        }

        [MenuItem("Assets/Create/New Script/Class", priority = 45, validate = false)]
        public static void CreateClass() {
            ProjectWindowUtil.CreateScriptAssetFromTemplateFile($"{fullpath}/MonoBehaviour.txt", "NewMonoBehaviour.cs");
        }

        [MenuItem("Assets/Create/New Script/Scriptable Object", priority = 46, validate = false)]
        public static void CreateScriptableObject() {
            ProjectWindowUtil.CreateScriptAssetFromTemplateFile($"{fullpath}/ScriptableObject.txt", "NewScriptableObject.cs");
        }

        [MenuItem("Assets/Create/New Script/Interface", priority = 47, validate = false)]
        public static void CreateInterface() {
            ProjectWindowUtil.CreateScriptAssetFromTemplateFile($"{fullpath}/Interface.txt", "NewInterface.cs");
        }

        [MenuItem("Assets/Create/New Script/Enum", priority = 48, validate = false)]
        public static void CreateEnum() {
            ProjectWindowUtil.CreateScriptAssetFromTemplateFile($"{fullpath}/Enum.txt", "NewEnum.cs");
        }

        [MenuItem("Assets/Create/New Script/Struct", priority = 49, validate = false)]
        public static void CreateStruct() {
            ProjectWindowUtil.CreateScriptAssetFromTemplateFile($"{fullpath}/Struct.txt", "NewStruct.cs");
        }
    }
}

#endif
