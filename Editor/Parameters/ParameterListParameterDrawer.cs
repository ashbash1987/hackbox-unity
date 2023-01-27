using UnityEngine;
using UnityEditor;

namespace Hackbox.Parameters
{
    [CustomPropertyDrawer(typeof(ParameterListParameter))]
    public class ParameterListParameterDrawer : BaseParameterDrawer
    {
        private ParameterListParameter _obj = null;

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return EditorGUI.GetPropertyHeight(GetValue(property)) + EditorGUIUtility.singleLineHeight;
        }

        protected override void OnParameterGUI(Rect position, SerializedProperty property, string name, SerializedProperty value)
        {
            if (_obj == null)
            {
                _obj = (ParameterListParameter)PropertyDiscovery.GetValue(property);
            }

            EditorGUI.LabelField(new Rect(position.x, position.y, position.width, EditorGUIUtility.singleLineHeight), new GUIContent(name));
            EditorGUI.PropertyField(new Rect(position.x, position.y + EditorGUIUtility.singleLineHeight, position.width, position.height - EditorGUIUtility.singleLineHeight), value, new GUIContent(), true);
        }
    }
}
