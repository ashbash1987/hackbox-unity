using System.Linq;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using UnityEngine;

namespace Hackbox
{
    public static class BackgroundHelpers
    {
        #region Solid Color
        public static bool TryParseColor(this string value, out Color color)
        {
            return ColorUtility.TryParseHtmlString(value, out color);
        }

        public static string ToColorString(this Color color)
        {
            return color.ToHTMLStringWithAlpha();
        }
        #endregion

        #region Linear Gradients
        public static bool TryParseLinearGradient(this string value, out Gradient gradient, out float gradientAngle)
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

        public static string ToLinearGradientString(this Gradient gradient, float gradientAngle = 0.0f)
        {
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

        #region Radial Gradients
        public static bool TryParseRadialGradient(this string value, out Gradient gradient, out string positioning)
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

        public static string ToRadialGradientString(this Gradient gradient, string positioning = "circle at center")
        {
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
    }
}
