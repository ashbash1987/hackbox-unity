using UnityEngine;

namespace Hackbox
{
    public static class ColorExtensions
    {
        public static string ToHTMLString(this Color color)
        {
            return $"#{ColorUtility.ToHtmlStringRGB(color)}";
        }
    }
}
