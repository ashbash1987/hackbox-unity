using UnityEngine;
using UnityEditor;

namespace Hackbox.Parameters
{
    [CustomPropertyDrawer(typeof(IntParameter))]
    public class IntParameterDrawer : PropertyDrawer
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
            value.intValue = EditorGUI.IntField(position, name.stringValue, value.intValue);

            EditorGUI.EndProperty();

            if (EditorGUI.EndChangeCheck())
            {
                serializedObject.ApplyModifiedProperties();
            }
        }
    }
}