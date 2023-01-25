using UnityEngine;
using UnityEditor;

namespace Hackbox.Parameters
{
    [CustomPropertyDrawer(typeof(StringArrayParameter))]
    public class StringArrayParameterDrawer : PropertyDrawer
    {
        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return EditorGUI.GetPropertyHeight(property.FindPropertyRelative("_value"));
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            SerializedObject serializedObject = property.serializedObject;

            EditorGUI.BeginChangeCheck();

            EditorGUI.BeginProperty(position, label, property);

            EditorGUI.indentLevel++;

            SerializedProperty name = property.FindPropertyRelative("Name");
            SerializedProperty value = property.FindPropertyRelative("_value");
            EditorGUI.PropertyField(position, value, new GUIContent(name.stringValue), true);

            EditorGUI.indentLevel--;

            EditorGUI.EndProperty();

            if (EditorGUI.EndChangeCheck())
            {
                serializedObject.ApplyModifiedProperties();
            }
        }
    }
}