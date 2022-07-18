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
            return EditorGUI.GetPropertyHeight(property.FindPropertyRelative("_value"));
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            if (_obj == null)
            {
                _obj = (ParameterListParameter)PropertyDiscovery.GetValue(property);
            }

            SerializedObject serializedObject = property.serializedObject;

            EditorGUI.BeginChangeCheck();
            serializedObject.Update();

            EditorGUI.BeginProperty(position, label, property);

            SerializedProperty name = property.FindPropertyRelative("Name");            
            SerializedProperty value = property.FindPropertyRelative("_value");
            EditorGUI.PropertyField(position, value, new GUIContent(name.stringValue), true);

            EditorGUI.EndProperty();

            serializedObject.ApplyModifiedProperties();
            EditorGUI.EndChangeCheck();
        }
    }
}