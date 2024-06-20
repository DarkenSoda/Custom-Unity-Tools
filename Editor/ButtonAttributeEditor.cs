using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using DarkenSoda.CustomTools.Scripts.Attributes;
using UnityEditor;
using UnityEngine;

namespace DarkenSoda.CustomTools.Scripts.EditorScripts
{
    [CustomEditor(typeof(MonoBehaviour), true)]
    public class ButtonAttributeEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            serializedObject.Update();
            DrawDefaultInspectorWithButtons();
            serializedObject.ApplyModifiedProperties();
        }

        private void DrawDefaultInspectorWithButtons()
        {
            var monoBehaviour = target as MonoBehaviour;
            var type = monoBehaviour.GetType();
            int ID = target.GetInstanceID();

            // Iterate through all serialized properties
            SerializedProperty property = serializedObject.GetIterator();
            if (property.NextVisible(true))
            {
                do
                {
                    EditorGUILayout.PropertyField(property, true);

                    if (property.propertyType == SerializedPropertyType.Generic)
                    {
                        // Handle nested serializable classes
                        var fieldInfo = type.GetField(property.name, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
                        if (fieldInfo != null)
                        {
                            var fieldValue = fieldInfo.GetValue(monoBehaviour);
                            if (fieldValue != null)
                            {
                                if (property.isExpanded)
                                {
                                    EditorGUI.indentLevel += 2;
                                    DrawButtons(monoBehaviour, fieldValue, fieldValue.GetType(), ID);
                                    EditorGUI.indentLevel -= 2;
                                }
                            }
                        }
                    }
                } while (property.NextVisible(false));
            }

            // Draw buttons for parent monobehaviour
            DrawButtons(monoBehaviour, monoBehaviour, type, ID);
        }

        private static void DrawButtons(MonoBehaviour parentMonobehaviour, object targetObject, Type type, int ID)
        {
            var methods = type.GetMethods(BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);

            foreach (var method in methods)
            {
                var attributes = method.GetCustomAttributes(typeof(ButtonAttribute), true);
                foreach (var attribute in attributes)
                {
                    GUILayout.Space(EditorGUIUtility.singleLineHeight);

                    var buttonAttribute = attribute as ButtonAttribute;
                    var label = string.IsNullOrEmpty(buttonAttribute.label) ? method.Name : buttonAttribute.label;

                    var parameters = method.GetParameters();

                    bool wasEnabled = GUI.enabled;
                    if (buttonAttribute.playModeOnly)
                    {
                        GUI.enabled = Application.isPlaying;    // Disable button when not in play mode
                        EditorGUILayout.LabelField($"(Play mode only)");
                    }

                    if (parameters.Length == 0)
                    {
                        EditorGUILayout.BeginHorizontal();

                        EditorGUILayout.LabelField(label);
                        if (GUILayout.Button("Click"))
                        {
                            if (method.ReturnType == typeof(IEnumerator))
                            {
                                var coroutine = method.Invoke(targetObject, null) as IEnumerator;
                                parentMonobehaviour.StartCoroutine(coroutine);
                            }
                            else
                            {
                                method.Invoke(targetObject, null);
                            }
                        }

                        EditorGUILayout.EndHorizontal();
                    }
                    else
                    {
                        bool foldout = EditorPrefs.GetBool($"{ID}.{type.Name}.{method.Name}.foldout", false);
                        foldout = EditorGUILayout.Foldout(foldout, label);
                        EditorPrefs.SetBool($"{ID}.{type.Name}.{method.Name}.foldout", foldout);

                        if (foldout)
                        {
                            object[] parameterValues = GetParameterValues(ID, type, method, parameters);
                            if (GUILayout.Button("Click"))
                            {
                                if (method.ReturnType == typeof(IEnumerator))
                                {
                                    var coroutine = method.Invoke(targetObject, parameterValues) as IEnumerator;
                                    parentMonobehaviour.StartCoroutine(coroutine);
                                }
                                else
                                {
                                    method.Invoke(targetObject, parameterValues);
                                }
                            }
                        }
                    }

                    GUI.enabled = wasEnabled;  // Restore the previous GUI state
                }
            }
        }

        private static object[] GetParameterValues(int ID, Type type, MethodInfo method, ParameterInfo[] parameters)
        {
            object[] parameterValues = new object[parameters.Length];

            for (int i = 0; i < parameters.Length; i++)
            {
                var param = parameters[i];
                var key = $"{ID}.{type.Name}.{method.Name}.{param.Name}";

                if (param.ParameterType == typeof(int))
                {
                    parameterValues[i] = EditorPrefs.GetInt(key, 0);
                    parameterValues[i] = EditorGUILayout.IntField(param.Name, (int)(parameterValues[i]));
                    EditorPrefs.SetInt(key, (int)(parameterValues[i]));
                }
                else if (param.ParameterType == typeof(float))
                {
                    parameterValues[i] = EditorPrefs.GetFloat(key, 0.0f);
                    parameterValues[i] = EditorGUILayout.FloatField(param.Name, (float)(parameterValues[i]));
                    EditorPrefs.SetFloat(key, (float)(parameterValues[i]));
                }
                else if (param.ParameterType == typeof(string))
                {
                    parameterValues[i] = EditorPrefs.GetString(key, string.Empty);
                    parameterValues[i] = EditorGUILayout.TextField(param.Name, (string)(parameterValues[i]));
                    EditorPrefs.SetString(key, (string)(parameterValues[i]));
                }
                else if (param.ParameterType == typeof(bool))
                {
                    parameterValues[i] = EditorPrefs.GetBool(key, false);
                    parameterValues[i] = EditorGUILayout.Toggle(param.Name, (bool)(parameterValues[i]));
                    EditorPrefs.SetBool(key, (bool)(parameterValues[i]));
                }
                else
                {
                    EditorGUILayout.LabelField($"Unsupported parameter type: {param.ParameterType}");
                }
            }

            return parameterValues;
        }
    }
}