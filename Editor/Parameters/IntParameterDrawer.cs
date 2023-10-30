using UnityEngine;
using UnityEditor;

namespace Hackbox.Parameters
{
    [CustomPropertyDrawer(typeof(IntParameter))]
    public class IntParameterDrawer : BaseParameterDrawer
    {
        protected override void OnParameterGUI(Rect position, SerializedProperty property, string name, string tooltip, SerializedProperty value)
        {
            value.intValue = EditorGUI.IntField(position, new GUIContent(name, tooltip), value.intValue);
        }
    }
}
