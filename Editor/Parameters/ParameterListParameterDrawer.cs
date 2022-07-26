using UnityEngine;
using UnityEditor;

namespace Hackbox.Parameters
{
    [CustomPropertyDrawer(typeof(ParameterListParameter))]
    public class ParameterListParameterDrawer : PropertyDrawer
    {
        private ParameterListParameter _obj = null;

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return EditorGUI.GetPropertyHeight(property.FindPropertyRelative("_value")) + EditorGUIUtility.singleLineHeight;
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            if (_obj == null)
            {
                _obj = (ParameterListParameter)PropertyDiscovery.GetValue(property);
            }

            SerializedObject serializedObject = property.serializedObject;

            EditorGUI.BeginChangeCheck();

            EditorGUI.BeginProperty(position, label, property);

            SerializedProperty name = property.FindPropertyRelative("Name");
            EditorGUI.LabelField(new Rect(position.x, position.y, position.width, EditorGUIUtility.singleLineHeight), new GUIContent(name.stringValue));

            SerializedProperty value = property.FindPropertyRelative("_value");
            EditorGUI.PropertyField(new Rect(position.x, position.y + EditorGUIUtility.singleLineHeight, position.width, position.height - EditorGUIUtility.singleLineHeight), value, new GUIContent(), true);

            EditorGUI.EndProperty();

            if (EditorGUI.EndChangeCheck())
            {
                serializedObject.ApplyModifiedProperties();
            }
        }
    }
}