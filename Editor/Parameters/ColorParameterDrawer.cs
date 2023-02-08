using UnityEngine;
using UnityEditor;

namespace Hackbox.Parameters
{
    [CustomPropertyDrawer(typeof(ColorParameter))]
    public class ColorParameterDrawer : BaseParameterDrawer
    {
        protected override void OnParameterGUI(Rect position, SerializedProperty property, string name, SerializedProperty value)
        {
            value.colorValue = EditorGUI.ColorField(position, name, value.colorValue);
        }
    }
}
