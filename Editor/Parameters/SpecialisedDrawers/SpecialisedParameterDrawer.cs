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
            SerializedObject serializedObject = BeginProperty(position, property, label);
            OnParameterGUI(position, property, GetName(property), GetValue(property));
            EndProperty(serializedObject);
        }
    }
}