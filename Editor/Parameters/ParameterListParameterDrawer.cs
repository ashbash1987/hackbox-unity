using UnityEngine;
using UnityEditor;

namespace Hackbox.Parameters
{
    [CustomPropertyDrawer(typeof(ParameterListParameter))]
    public class ParameterListParameterDrawer : BaseParameterDrawer
    {
        private ParameterListParameter _obj = null;
        private bool _foldout = false;

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            if (_foldout)
            {
                return EditorGUI.GetPropertyHeight(GetValue(property)) + EditorGUIUtility.singleLineHeight;
            }

            return EditorGUIUtility.singleLineHeight;
        }

        protected override void OnParameterGUI(Rect position, SerializedProperty property, string name, string tooltip, SerializedProperty value)
        {
            if (_obj == null)
            {
                _obj = (ParameterListParameter)PropertyDiscovery.GetValue(property);
            }

            EditorGUI.indentLevel++;
            _foldout = EditorGUI.Foldout(new Rect(position.x, position.y, position.width, EditorGUIUtility.singleLineHeight), _foldout, new GUIContent(name, tooltip));
            if (_foldout)
            {
                EditorGUI.PropertyField(new Rect(position.x, position.y + EditorGUIUtility.singleLineHeight, position.width, position.height - EditorGUIUtility.singleLineHeight), value, new GUIContent("", tooltip), true);
            }
            EditorGUI.indentLevel--;
        }
    }
}
