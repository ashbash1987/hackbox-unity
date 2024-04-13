using System.Linq;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEditor;

namespace Hackbox
{
    [CustomPropertyDrawer(typeof(BackgroundStringAttribute))]
    public class BackgroundStringDrawer : PropertyDrawer
    {
        internal enum Mode
        {
            Color,
            LinearGradient,
            RadialGradient,
            Custom
        }

        private Mode? _mode = null;

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return EditorGUIUtility.singleLineHeight * 2;
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            if (_mode == null)
            {
                _mode = DetermineMode(property.stringValue);
            }

            _mode = (Mode)EditorGUI.EnumPopup(new Rect(position.x, position.y, position.width, EditorGUIUtility.singleLineHeight), property.displayName, _mode);
            position.y += EditorGUIUtility.singleLineHeight;
            position.height -= EditorGUIUtility.singleLineHeight;

            position.x += EditorGUIUtility.labelWidth;
            position.width -= EditorGUIUtility.labelWidth;

            property.stringValue = DoModeGUI(position, _mode, property.stringValue);
        }

        internal static string DoModeGUI(Rect position, Mode? mode, string value)
        {
            if (!mode.HasValue)
            {
                return OnCustomModeGUI(position, value);
            }

            switch (mode.Value)
            {
                case Mode.Color:
                    return OnColorModeGUI(position, value);

                case Mode.LinearGradient:
                    return OnLinearGradientModeGUI(position, value);

                case Mode.RadialGradient:
                    return OnRadialGradientModeGUI(position, value);

                default:
                    return OnCustomModeGUI(position, value);
            }
        }

        internal static Mode DetermineMode(string value)
        {
            if (TryColor(value, out _))
            {
                return Mode.Color;
            }
            if (TryLinearGradient(value, out _, out _))
            {
                return Mode.LinearGradient;
            }
            if (TryRadialGradient(value, out _, out _))
            {
                return Mode.RadialGradient;
            }

            return Mode.Custom;
        }

        #region Color Mode
        internal static bool TryColor(string value, out Color color)
        {
            return ColorUtility.TryParseHtmlString(value, out color);
        }

        internal static string OnColorModeGUI(Rect position, string value)
        {
            if (!TryColor(value, out Color color))
            {
                if (TryLinearGradient(value, out Gradient gradient, out _))
                {
                    color = gradient.colorKeys[0].color;
                }
                else
                {
                    color = Color.black;
                }
            }

            color = EditorGUI.ColorField(position, color);
            return color.ToHTMLStringWithAlpha();
        }
        #endregion

        #region Linear Gradient Mode
        internal static bool TryLinearGradient(string value, out Gradient gradient, out float gradientAngle)
        {
            Regex regex = new Regex(@"linear-gradient\((?<angle>[\d.]+)deg, (?<step>(?<color>#[\dA-Fa-f]+)\s+(?<progress>[\d.]+)%,?\s*)*\)");
            Match match = regex.Match(value);

            if (match.Success)
            {
                gradientAngle = float.Parse(match.Groups["angle"].Value);
                gradient = new Gradient();

                List<Color> colors = new List<Color>();
                foreach (Capture capture in match.Groups["color"].Captures)
                {
                    if (!ColorUtility.TryParseHtmlString(capture.Value, out Color color))
                    {
                        color = Color.black;
                    }

                    colors.Add(color);
                }

                List<float> progresses = new List<float>();
                foreach (Capture capture in match.Groups["progress"].Captures)
                {
                    if (!float.TryParse(capture.Value, out float progress))
                    {
                        progress = 0.0f;
                    }

                    progresses.Add(progress);
                }

                gradient.SetKeys(colors.Select((x, i) => new GradientColorKey(x, progresses[i] * 0.01f)).ToArray(),
                                 colors.Select((x, i) => new GradientAlphaKey(x.a, progresses[i] * 0.01f)).ToArray());

                return true;
            }
            else
            {
                gradientAngle = 0.0f;
                gradient = new Gradient();

                return false;
            }
        }

        internal static string OnLinearGradientModeGUI(Rect position, string value)
        {
            if (!TryLinearGradient(value, out Gradient gradient, out float gradientAngle))
            {
                gradientAngle = 0.0f;
                if (!TryRadialGradient(value, out gradient, out _))
                {
                    if (TryColor(value, out Color color))
                    {
                        gradient.SetKeys(new GradientColorKey[] { new GradientColorKey(color, 0), new GradientColorKey(color, 1) },
                                         new GradientAlphaKey[] { new GradientAlphaKey(color.a, 0), new GradientAlphaKey(color.a, 1) });
                    }
                    else
                    {
                        gradient.SetKeys(new GradientColorKey[] { new GradientColorKey(Color.black, 0), new GradientColorKey(Color.black, 1) },
                                         new GradientAlphaKey[] { new GradientAlphaKey(1, 0), new GradientAlphaKey(1, 1) });
                    }
                }
            }

            Rect angleLabelPosition = position;
            angleLabelPosition.x += angleLabelPosition.width - 10;
            angleLabelPosition.width = 10;

            Rect anglePosition = position;
            anglePosition.x += anglePosition.width - 40;
            anglePosition.width = 40;
            position.width -= 40;

            gradient = EditorGUI.GradientField(position, gradient);
            gradientAngle = EditorGUI.FloatField(anglePosition, gradientAngle);
            EditorGUI.LabelField(angleLabelPosition, "°");

            StringBuilder outputBuilder = new StringBuilder();
            outputBuilder.Append($"linear-gradient({gradientAngle}deg, ");
            outputBuilder.Append(string.Join(", ", gradient.colorKeys.Select(x => x.time).Concat(gradient.alphaKeys.Select(x => x.time)).Distinct().OrderBy(x => x).Select(x =>
            {
                Color keyColor = gradient.Evaluate(x);
                return $"{keyColor.ToHTMLStringWithAlpha()} {x * 100}%";
            })));
            outputBuilder.Append(")");

            return outputBuilder.ToString();
        }
        #endregion

        #region Radial Gradient Mode
        internal static bool TryRadialGradient(string value, out Gradient gradient, out string positioning)
        {
            Regex regex = new Regex(@"radial-gradient\((?<positioning>[^,]+), (?<step>(?<color>#[\dA-Fa-f]+)\s+(?<progress>[\d.]+)%,?\s*)*\)");
            Match match = regex.Match(value);

            if (match.Success)
            {
                positioning = match.Groups["positioning"].Value;
                gradient = new Gradient();

                List<Color> colors = new List<Color>();
                foreach (Capture capture in match.Groups["color"].Captures)
                {
                    if (!ColorUtility.TryParseHtmlString(capture.Value, out Color color))
                    {
                        color = Color.black;
                    }

                    colors.Add(color);
                }

                List<float> progresses = new List<float>();
                foreach (Capture capture in match.Groups["progress"].Captures)
                {
                    if (!float.TryParse(capture.Value, out float progress))
                    {
                        progress = 0.0f;
                    }

                    progresses.Add(progress);
                }

                gradient.SetKeys(colors.Select((x, i) => new GradientColorKey(x, progresses[i] * 0.01f)).ToArray(),
                                 colors.Select((x, i) => new GradientAlphaKey(x.a, progresses[i] * 0.01f)).ToArray());

                return true;
            }
            else
            {
                positioning = "circle at center";
                gradient = new Gradient();

                return false;
            }
        }

        internal static string OnRadialGradientModeGUI(Rect position, string value)
        {
            if (!TryRadialGradient(value, out Gradient gradient, out string positioning))
            {
                positioning = "circle at center";
                if (!TryLinearGradient(value, out gradient, out _))
                {
                    if (TryColor(value, out Color color))
                    {
                        gradient.SetKeys(new GradientColorKey[] { new GradientColorKey(color, 0), new GradientColorKey(color, 1) },
                                         new GradientAlphaKey[] { new GradientAlphaKey(color.a, 0), new GradientAlphaKey(color.a, 1) });
                    }
                    else
                    {
                        gradient.SetKeys(new GradientColorKey[] { new GradientColorKey(Color.black, 0), new GradientColorKey(Color.black, 1) },
                                         new GradientAlphaKey[] { new GradientAlphaKey(1, 0), new GradientAlphaKey(1, 1) });
                    }
                }
            }

            Rect positioningPosition = position;
            positioningPosition.x += positioningPosition.width - 100;
            positioningPosition.width = 100;
            position.width -= 100;

            gradient = EditorGUI.GradientField(position, gradient);
            positioning = EditorGUI.TextField(positioningPosition, positioning);

            StringBuilder outputBuilder = new StringBuilder();
            outputBuilder.Append($"radial-gradient({positioning}, ");
            outputBuilder.Append(string.Join(", ", gradient.colorKeys.Select(x => x.time).Concat(gradient.alphaKeys.Select(x => x.time)).Distinct().OrderBy(x => x).Select(x =>
            {
                Color keyColor = gradient.Evaluate(x);
                return $"{keyColor.ToHTMLStringWithAlpha()} {x * 100}%";
            })));
            outputBuilder.Append(")");

            return outputBuilder.ToString();
        }
        #endregion

        #region Custom Mode
        internal static string OnCustomModeGUI(Rect position, string value)
        {
            return EditorGUI.TextField(position, value);
        }
        #endregion
    }
}