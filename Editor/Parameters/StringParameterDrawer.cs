using UnityEngine;
using UnityEditor;

namespace Hackbox.Parameters
{
    [CustomPropertyDrawer(typeof(StringParameter))]
    public class StringParameterDrawer : BaseParameterDrawer
    {
        protected override void OnParameterGUI(Rect position, SerializedProperty property, string name, string tooltip, SerializedProperty value)
        {
            value.stringValue = EditorGUI.TextField(position, new GUIContent(name, tooltip), value.stringValue);
        }
    }
}
