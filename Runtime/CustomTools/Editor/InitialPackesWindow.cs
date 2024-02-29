#if UNITY_EDITOR

using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using DarkenSoda.CustomTools.Scripts;
using DarkenSoda.CustomTools.ScriptableObjects;

namespace DarkenSoda.CustomTools.Editor {
    public class InitialPackagesWindow : EditorWindow {
        public PackagesToInstallSO packages;
        private Dictionary<string, PackageData> data = new();

        [MenuItem("Tools/Setup/Install Packages")]
        static void Init() {
            InitialPackagesWindow window =
                EditorWindow.GetWindow<InitialPackagesWindow>("Select Packages", typeof(EditorWindow));
            window.minSize = new Vector2(200, 300);
            window.maxSize = new Vector2(350, 500);
            window.Show();
        }

        private void OnGUI() {
            GUILayout.BeginArea(new Rect(Screen.width / 20, 10, Screen.width * 0.9f, Screen.height * 0.8f), "Packages", new GUIStyle("window") {
                stretchHeight = true,
                stretchWidth = true,
            });

            packages = (PackagesToInstallSO)EditorGUILayout.ObjectField("Packages To Install", packages, typeof(PackagesToInstallSO), false);

            data = new Dictionary<string, PackageData>();
            if (packages != null) {
                foreach (var package in packages.Packages) {
                    if (data.ContainsKey(package.Link))
                        continue;
                    data.Add(package.Link, package);
                }
            }

            if (GUILayout.Button("Select All", new GUIStyle("button") { margin = new RectOffset(10, 0, 10, 10), fixedWidth = 75, fixedHeight = 23 })) {
                foreach (var package in data) {
                    package.Value.isSelected = true;
                }
            }

            foreach (var package in data) {
                package.Value.isSelected = GUILayout.Toggle(package.Value.isSelected, package.Value.Name,
                        new GUIStyle("toggle") {
                            fontSize = 14,
                            margin = new RectOffset(10, 0, 5, 7),
                        });
            }
            GUILayout.EndArea();

            GUILayout.FlexibleSpace();
            if (GUILayout.Button("Install Selected Packages",
                    new GUIStyle("button") {
                        fontSize = 16,
                        stretchWidth = true,
                        wordWrap = true,
                        clipping = TextClipping.Clip,
                        margin = new RectOffset(10, 10, 5, 10),
                    }, GUILayout.Height(40))) {
                List<string> packagesToInstall = new();

                foreach (var package in data) {
                    if (package.Value.isSelected) {
                        packagesToInstall.Add(package.Value.Link);
                    }
                }

                Setup.InstallInitialPackages(packagesToInstall);
            }
        }
    }
}

#endif
