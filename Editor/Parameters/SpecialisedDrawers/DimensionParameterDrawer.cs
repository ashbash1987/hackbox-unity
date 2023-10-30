using System.Text.RegularExpressions;
using UnityEngine;
using UnityEditor;

namespace Hackbox.Parameters
{
    public class DimensionParameterDrawer : SpecialisedParameterDrawer
    {
        private static readonly string[] UNITS = new[]
        {
            "%",
            "px",
            "em",
            "fr",
            "rem",
            "vw",
            "vh",
            "vmin",
            "vmax",
            "custom"
        };

        protected override void OnParameterGUI(Rect position, SerializedProperty property, string name, string tooltip, SerializedProperty value)
        {
            Regex regex = new Regex(@"(?<value>[\d.]+)\s*(?<unit>%|px|em|rem|vw|vh|vmin|vmax)");
            Match match = regex.Match(value.stringValue);

            Rect unitPosition = position;
            unitPosition.x += unitPosition.width - 40;
            unitPosition.width = 40;
            position.width -= 40;

            if (match.Success)
            {
                float dimensionValue = float.Parse(match.Groups["value"].Value);
                string dimensionUnit = match.Groups["unit"].Value;
                int dimensionUnitIndex = ArrayUtility.IndexOf(UNITS, dimensionUnit);

                dimensionValue = EditorGUI.FloatField(position, new GUIContent(name, tooltip), dimensionValue);
                dimensionUnit = UNITS[EditorGUI.Popup(unitPosition, dimensionUnitIndex, UNITS)];

                value.stringValue = dimensionUnit != "custom" ? $"{dimensionValue}{dimensionUnit}" : dimensionValue.ToString();
            }
            else
            {
                string dimensionValue = EditorGUI.TextField(position, new GUIContent(name, tooltip), value.stringValue);
                int dimensionUnitIndex = EditorGUI.Popup(unitPosition, ArrayUtility.IndexOf(UNITS, "custom"), UNITS);
                string dimensionUnit = UNITS[EditorGUI.Popup(unitPosition, dimensionUnitIndex, UNITS)];

                value.stringValue = dimensionUnit != "custom" ? $"{dimensionValue}{dimensionUnit}" : dimensionValue.ToString();
            }
        }
    }
}