using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace Hackbox.Parameters
{
    public abstract class BaseParameterDrawer : PropertyDrawer
    {
        private readonly Dictionary<string, BaseParameterDrawer> _specialisedDrawers = new Dictionary<string, BaseParameterDrawer>();

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            BaseParameterDrawer specialisedDrawer = GetSpecialisedDrawer(property);
            if (specialisedDrawer != null)
            {
                return specialisedDrawer.GetPropertyHeight(property, label);
            }

            return EditorGUIUtility.singleLineHeight;
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            BaseParameterDrawer specialisedDrawer = GetSpecialisedDrawer(property);
            if (specialisedDrawer != null)
            {
                specialisedDrawer.OnGUI(position, property, label);
                return;
            }

            string name = GetName(property);
            SerializedObject serializedObject = BeginProperty(position, property, label, name);

            string tooltip = "";
            if (DefaultParameters.AllParameterInfo.TryGetValue(name, out DefaultParameters.ParameterInfoEntry parameterInfo))
            {
                tooltip = parameterInfo.HelpText;
            }

            OnParameterGUI(position, property, name, tooltip, GetValue(property));
            EndProperty(serializedObject);
        }

        protected abstract void OnParameterGUI(Rect position, SerializedProperty property, string name, string tooltip, SerializedProperty value);

        protected string GetName(SerializedProperty property)
        {
            return property.FindPropertyRelative("Name").stringValue;
        }

        protected SerializedProperty GetValue(SerializedProperty property)
        {
            return property.FindPropertyRelative("_value");
        }

        protected SerializedObject BeginProperty(Rect position, SerializedProperty property, GUIContent label, string name)
        {
            SerializedObject serializedObject = property.serializedObject;
            EditorGUI.BeginChangeCheck();
            EditorGUI.BeginProperty(position, label, property);

            return serializedObject;
        }

        protected void EndProperty(SerializedObject serializedObject)
        {
            EditorGUI.EndProperty();
            if (EditorGUI.EndChangeCheck())
            {
                serializedObject.ApplyModifiedProperties();
            }
        }

        private BaseParameterDrawer GetSpecialisedDrawer(SerializedProperty property)
        {
            if (!_specialisedDrawers.ContainsKey(property.propertyPath))
            {
                _specialisedDrawers[property.propertyPath] = SpecialisedParameterDrawerLookup.CreateSpecialisedParameterDrawer(GetName(property));
            }

            return _specialisedDrawers[property.propertyPath];
        }
    }
}
