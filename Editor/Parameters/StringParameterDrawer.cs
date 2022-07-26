using UnityEngine;
using UnityEditor;

namespace Hackbox.Parameters
{
    [CustomPropertyDrawer(typeof(StringParameter))]
    public class StringParameterDrawer : PropertyDrawer
    {
        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return EditorGUIUtility.singleLineHeight;
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            SerializedObject serializedObject = property.serializedObject;

            EditorGUI.BeginChangeCheck();

            EditorGUI.BeginProperty(position, label, property);

            SerializedProperty name = property.FindPropertyRelative("Name");
            SerializedProperty value = property.FindPropertyRelative("_value");
            value.stringValue = EditorGUI.TextField(position, name.stringValue, value.stringValue);

            EditorGUI.EndProperty();

            if (EditorGUI.EndChangeCheck())
            {
                serializedObject.ApplyModifiedProperties();
            }
        }
    }
}