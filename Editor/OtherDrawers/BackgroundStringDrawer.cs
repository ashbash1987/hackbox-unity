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
            Image,
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

                case Mode.Image:
                    return OnImageModeGUI(position, value);

                default:
                    return OnCustomModeGUI(position, value);
            }
        }

        internal static Mode DetermineMode(string value)
        {
            if (value.TryParseColor(out _))
            {
                return Mode.Color;
            }
            if (value.TryParseLinearGradient(out _, out _))
            {
                return Mode.LinearGradient;
            }
            if (value.TryParseLinearGradient(out _, out _))
            {
                return Mode.RadialGradient;
            }
            if (value.TryParseImage(out _, out _))
            {
                return Mode.Image;
            }

            return Mode.Custom;
        }

        #region Color Mode
        internal static string OnColorModeGUI(Rect position, string value)
        {
            if (!value.TryParseColor(out Color color))
            {
                if (value.TryParseLinearGradient(out Gradient gradient, out _))
                {
                    color = gradient.colorKeys[0].color;
                }
                else
                {
                    color = Color.black;
                }
            }

            color = EditorGUI.ColorField(position, color);
            return color.ToColorString();
        }
        #endregion

        #region Linear Gradient Mode
        internal static string OnLinearGradientModeGUI(Rect position, string value)
        {
            if (!value.TryParseLinearGradient(out Gradient gradient, out float gradientAngle))
            {
                gradientAngle = 0.0f;
                if (!value.TryParseRadialGradient(out gradient, out _))
                {
                    if (value.TryParseColor(out Color color))
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

            return gradient.ToLinearGradientString(gradientAngle);
        }
        #endregion

        #region Radial Gradient Mode
        internal static string OnRadialGradientModeGUI(Rect position, string value)
        {
            if (!value.TryParseRadialGradient(out Gradient gradient, out string positioning))
            {
                positioning = "circle at center";
                if (!value.TryParseLinearGradient(out gradient, out _))
                {
                    if (value.TryParseColor(out Color color))
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

            return gradient.ToRadialGradientString(positioning);
        }
        #endregion

        #region Image Mode
        internal static string OnImageModeGUI(Rect position, string value)
        {
            if (!value.TryParseImage(out string url, out string scalingAndPositioning))
            {
                url = "";
                scalingAndPositioning = "no-repeat center / cover";
            }

            Rect positioningPosition = position;
            positioningPosition.x += positioningPosition.width - 100;
            positioningPosition.width = 100;
            position.width -= 100;

            url = EditorGUI.TextField(position, url);
            scalingAndPositioning = EditorGUI.TextField(positioningPosition, scalingAndPositioning);

            return url.ToImageString(scalingAndPositioning);
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
