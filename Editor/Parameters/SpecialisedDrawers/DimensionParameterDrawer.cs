using System.Linq;
using System.Collections.Generic;
using System.Text;
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
            "rem",
            "vw",
            "vh",
            "vmin",
            "vmax"
        };

        protected override void OnParameterGUI(Rect position, SerializedProperty property, string name, SerializedProperty value)
        {
            Regex regex = new Regex(@"(?<value>[\d.]+)\s*(?<unit>%|px|em|rem|vw|vh|vmin|vmax)");
            Match match = regex.Match(value.stringValue);

            if (match.Success)
            {
                float dimensionValue = float.Parse(match.Groups["value"].Value);
                string dimensionUnit = match.Groups["unit"].Value;
                int dimensionUnitIndex = ArrayUtility.IndexOf(UNITS, dimensionUnit);

                Rect unitPosition = position;
                unitPosition.x += unitPosition.width - 40;
                unitPosition.width = 40;
                position.width -= 40;

                dimensionValue = EditorGUI.FloatField(position, name, dimensionValue);
                dimensionUnit = UNITS[EditorGUI.Popup(unitPosition, dimensionUnitIndex, UNITS)];

                value.stringValue = $"{dimensionValue}{dimensionUnit}";
            }
            else
            {
                value.stringValue = EditorGUI.TextField(position, value.stringValue);
            }
        }
    }
}