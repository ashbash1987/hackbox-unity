using UnityEngine;
using UnityEditor;

namespace Hackbox.Parameters
{
    [CustomPropertyDrawer(typeof(ChoicesParameter))]
    public class ChoicesParameterDrawer : BaseParameterDrawer
    {
        private ChoicesParameter _obj = null;

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return EditorGUI.GetPropertyHeight(GetValue(property));
        }

        protected override void OnParameterGUI(Rect position, SerializedProperty property, string name, SerializedProperty value)
        {
            if (_obj == null)
            {
                _obj = (ChoicesParameter)PropertyDiscovery.GetValue(property);
            }

            EditorGUI.indentLevel++;
            EditorGUI.PropertyField(position, value, new GUIContent(name), true);
            EditorGUI.indentLevel--;
        }
    }
}
