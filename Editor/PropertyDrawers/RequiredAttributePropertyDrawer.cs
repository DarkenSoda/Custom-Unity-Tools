using DarkenSoda.CustomTools.Scripts.Attributes;
using UnityEditor;
using UnityEngine;

namespace DarkenSoda.CustomTools.EditorScripts.PropertyDrawers
{
    [CustomPropertyDrawer(typeof(RequiredAttribute))]
    public class RequiredAttributePropertyDrawer : PropertyDrawer
    {
        private readonly Color errorColor = new Color(1f, .2f, .2f, 0.1f);
        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            if (IsFieldEmpty(property))
            {
                return EditorGUIUtility.singleLineHeight * 3 + base.GetPropertyHeight(property, label);
            }

            return base.GetPropertyHeight(property, label);
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            if (!IsFieldSupported(property))
            {
                Debug.LogError("Required Attribute is placed on an unsupported field type.");
            }

            if (IsFieldSupported(property) && IsFieldEmpty(property))
            {
                position.y += EditorGUIUtility.singleLineHeight;
                position.height = EditorGUIUtility.singleLineHeight;
                position.height += base.GetPropertyHeight(property, label);

                EditorGUI.HelpBox(position, "Required", MessageType.Error);
                EditorGUI.DrawRect(position, errorColor);

                position.height = base.GetPropertyHeight(property, label);
                position.y += EditorGUIUtility.singleLineHeight * 2.2f;
            }

            EditorGUI.PropertyField(position, property, label, true);
        }

        private bool IsFieldEmpty(SerializedProperty property)
        {
            switch (property.propertyType)
            {
                case SerializedPropertyType.ObjectReference:
                    return property.objectReferenceValue == null;
                case SerializedPropertyType.ExposedReference:
                    return property.exposedReferenceValue == null;
                case SerializedPropertyType.ManagedReference:
                    return property.managedReferenceValue == null;
                case SerializedPropertyType.Generic:
                    return property.exposedReferenceValue == null || property.arraySize == 0 || property.minArraySize == 0;
                case SerializedPropertyType.String:
                    return string.IsNullOrEmpty(property.stringValue);
                default:
                    return false;
            }
        }

        private bool IsFieldSupported(SerializedProperty property)
        {
            switch (property.propertyType)
            {
                case SerializedPropertyType.ObjectReference:
                case SerializedPropertyType.ExposedReference:
                case SerializedPropertyType.ManagedReference:
                case SerializedPropertyType.Generic:
                case SerializedPropertyType.String:
                    return true;
                default:
                    return false;
            }
        }
    }
}

