using UnityEngine;

namespace Hackbox
{
    public static class ColorExtensions
    {
        public static string ToHTMLString(this Color color)
        {
            return $"#{ColorUtility.ToHtmlStringRGB(color)}";
        }

        public static string ToHTMLStringWithAlpha(this Color color)
        {
            return $"#{ColorUtility.ToHtmlStringRGBA(color)}";
        }
    }
}
