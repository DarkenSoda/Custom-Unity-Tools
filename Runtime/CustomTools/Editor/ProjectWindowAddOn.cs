#if UNITY_EDITOR

using UnityEditor;
using UnityEngine;

namespace DarkenSoda.CustomTools.Editor {
    [InitializeOnLoad]
    public static class ProjectWindowAddOn {
        private static double LastCacheUpdateTime = -3d;
        private static EditorWindow ProjectWindow = null;
        private static Texture FolderIcon = EditorGUIUtility.FindTexture("FolderOpened On Icon");
        private static Texture ScriptIcon = EditorGUIUtility.FindTexture("d_cs Script Icon");

        // Define the menu items
        private static GUIContent[] menuItems = new GUIContent[] {
            new GUIContent("Class"),
            new GUIContent("Scriptable Object"),
            new GUIContent("Interface"),
            new GUIContent("Enum"),
            new GUIContent("Struct"),
        };

        [InitializeOnLoadMethod]
        public static void EditorInit() {
            EditorApplication.update += () => {
                if (ProjectWindow != null || EditorApplication.timeSinceStartup <= LastCacheUpdateTime + 2d) return;
                LastCacheUpdateTime = EditorApplication.timeSinceStartup;
                foreach (var window in Resources.FindObjectsOfTypeAll<EditorWindow>()) {
                    if (window.GetType().Name.EndsWith("ProjectBrowser")) {
                        window.wantsMouseMove = true;
                        ProjectWindow = window;
                        break;
                    }
                }
            };

            // Project Item GUI
            EditorApplication.projectWindowItemOnGUI -= ProjectItemGUI;
            EditorApplication.projectWindowItemOnGUI += ProjectItemGUI;
            static void ProjectItemGUI(string guid, Rect selectionRect) {
                selectionRect.width += selectionRect.x;
                selectionRect.x = 0;
                if (selectionRect.Contains(Event.current.mousePosition)) {
                    // Draw the selection rectangle.
                    EditorGUI.DrawRect(selectionRect, new Color(1, 1, 1, 0.06f));

                    string folder = AssetDatabase.GUIDToAssetPath(guid);
                    if (AssetDatabase.IsValidFolder(folder)) {
                        float buttonWidth = 20f; // Adjust this value based on your icon size or desired width
                        float buttonSpacing = 30f; // Adjust the spacing between buttons

                        // Calculate position for scriptIcon button
                        float scriptIconButtonX = selectionRect.x + selectionRect.width - buttonSpacing;
                        Rect scriptIconButtonRect = new Rect(scriptIconButtonX, selectionRect.y - 4, buttonWidth, selectionRect.height + 7);
                        if (GUI.Button(scriptIconButtonRect, ScriptIcon, new GUIStyle("label") { fixedWidth = 20 })) {
                            // Show Script menu
                            Selection.activeObject = AssetDatabase.LoadAssetAtPath<UnityEngine.Object>(folder);
                            EditorUtility.DisplayCustomMenu(scriptIconButtonRect, menuItems, -1, ShowMenu, null);
                        }

                        // Calculate position for folderIcon button
                        float folderIconButtonX = scriptIconButtonX - buttonSpacing;
                        Rect folderIconButtonRect = new Rect(folderIconButtonX, selectionRect.y - 4, buttonWidth, selectionRect.height + 7);
                        if (GUI.Button(folderIconButtonRect, FolderIcon, new GUIStyle("label") { fixedWidth = 20 })) {
                            // Create Folder.
                            Selection.activeObject = AssetDatabase.LoadAssetAtPath<UnityEngine.Object>(folder);
                            ProjectWindowUtil.CreateFolder();
                        }
                    }
                }
                if (EditorWindow.mouseOverWindow != null && Event.current.type == EventType.MouseMove) {
                    EditorWindow.mouseOverWindow.Repaint();
                    Event.current.Use();
                }
            }

            static void ShowMenu(object userData, string[] options, int selected) {
                switch(options[selected]) {
                    case "Class":
                        ScriptGenerator.CreateClass();
                        break;
                    case "Scriptable Object":
                        ScriptGenerator.CreateScriptableObject();
                        break;
                    case "Interface":
                        ScriptGenerator.CreateInterface();
                        break;
                    case "Enum":
                        ScriptGenerator.CreateEnum();
                        break;
                    case "Struct":
                        ScriptGenerator.CreateStruct();
                        break;
                    default: return;
                }
            }
        }

    }
}

#endif
