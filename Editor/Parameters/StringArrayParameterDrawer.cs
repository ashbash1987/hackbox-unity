using UnityEngine;
using UnityEditor;

namespace Hackbox.Parameters
{
    [CustomPropertyDrawer(typeof(StringArrayParameter))]
    public class StringArrayParameterDrawer : BaseParameterDrawer
    {
        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return EditorGUI.GetPropertyHeight(GetValue(property));
        }

        protected override void OnParameterGUI(Rect position, SerializedProperty property, string name, SerializedProperty value)
        {
            EditorGUI.indentLevel++;
            EditorGUI.PropertyField(position, value, new GUIContent(name), true);
            EditorGUI.indentLevel--;
        }
    }
}
