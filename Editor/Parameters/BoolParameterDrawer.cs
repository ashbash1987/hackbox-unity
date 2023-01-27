using UnityEngine;
using UnityEditor;

namespace Hackbox.Parameters
{
    [CustomPropertyDrawer(typeof(BoolParameter))]
    public class BoolParameterDrawer : BaseParameterDrawer
    {
        protected override void OnParameterGUI(Rect position, SerializedProperty property, string name, SerializedProperty value)
        {
            value.boolValue = EditorGUI.Toggle(position, name, value.boolValue);
        }
    }
}
