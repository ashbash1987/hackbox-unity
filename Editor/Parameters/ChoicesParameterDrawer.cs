using UnityEngine;
using UnityEditor;

namespace Hackbox.Parameters
{
    [CustomPropertyDrawer(typeof(ChoicesParameter))]
    public class ChoicesParameterDrawer : PropertyDrawer
    {
        private ChoicesParameter _obj = null;

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return EditorGUI.GetPropertyHeight(property.FindPropertyRelative("_value"));
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            if (_obj == null)
            {
                _obj = (ChoicesParameter)PropertyDiscovery.GetValue(property);
            }

            SerializedObject serializedObject = property.serializedObject;

            EditorGUI.BeginChangeCheck();
            
            EditorGUI.BeginProperty(position, label, property);

            SerializedProperty name = property.FindPropertyRelative("Name");            
            SerializedProperty value = property.FindPropertyRelative("_value");
            EditorGUI.PropertyField(position, value, new GUIContent(name.stringValue), true);

            EditorGUI.EndProperty();

            if (EditorGUI.EndChangeCheck())
            {
                serializedObject.ApplyModifiedProperties();
            }
        }
    }
}