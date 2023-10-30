using UnityEngine;
using UnityEditor;

namespace Hackbox.Parameters
{
    public abstract class SpecialisedParameterDrawer : BaseParameterDrawer
    {
        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return EditorGUIUtility.singleLineHeight;
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
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
    }
}