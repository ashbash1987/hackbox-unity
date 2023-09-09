using UnityEngine;
using UnityEditor;

namespace Hackbox.Parameters
{
    public class BackgroundParameterDrawer : SpecialisedParameterDrawer
    {
        private BackgroundStringDrawer.Mode? _mode = null;

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return EditorGUIUtility.singleLineHeight * 2;
        }

        protected override void OnParameterGUI(Rect position, SerializedProperty property, string name, SerializedProperty value)
        {
            if (_mode == null)
            {
                _mode = BackgroundStringDrawer.DetermineMode(value.stringValue);
            }

            _mode = (BackgroundStringDrawer.Mode)EditorGUI.EnumPopup(new Rect(position.x, position.y, position.width, EditorGUIUtility.singleLineHeight), name, _mode);
            position.y += EditorGUIUtility.singleLineHeight;
            position.height -= EditorGUIUtility.singleLineHeight;

            position.x += EditorGUIUtility.labelWidth;
            position.width -= EditorGUIUtility.labelWidth;

            value.stringValue = BackgroundStringDrawer.DoModeGUI(position, _mode, value.stringValue);
        }
    }
}