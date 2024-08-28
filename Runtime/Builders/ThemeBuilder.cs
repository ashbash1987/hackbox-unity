using System.Linq;
using UnityEngine;
using Hackbox.UI;

namespace Hackbox.Builders
{
    public sealed class ThemeBuilder
    {
        public ThemeBuilder(string name)
        {
            Theme = Theme.Create(name);
        }

        public Theme Theme
        {
            get;
            private set;
        }

        #region Public Methods
        public static ThemeBuilder Create(string name)
        {
            return new ThemeBuilder(name);
        }

        #region Header
        public ThemeBuilder SetHeaderColor(Color color)
        {
            Theme.HeaderColor = color;
            return this;
        }

        public ThemeBuilder SetHeaderBackground(string background)
        {
            Theme.HeaderBackground = background;
            return this;
        }

        public ThemeBuilder SetHeaderBackgroundColor(Color backgroundColor)
        {
            return SetHeaderBackground(backgroundColor.ToColorString());
        }

        public ThemeBuilder SetHeaderBackgroundLinearGradient(Gradient gradient, float gradientAngle = 0.0f)
        {
            return SetHeaderBackground(gradient.ToLinearGradientString(gradientAngle));
        }

        public ThemeBuilder SetHeaderBackgroundRadialGradient(Gradient gradient, string positioning = "circle at center")
        {
            return SetHeaderBackground(gradient.ToRadialGradientString(positioning));
        }

        public ThemeBuilder SetHeaderMinimumHeight(string height = "50px")
        {
            Theme.HeaderMinHeight = height;
            return this;
        }

        public ThemeBuilder SetHeaderMinimumHeight(float height = 50, string dimensionUnit = "px")
        {
            return SetHeaderMinimumHeight($"{height}{dimensionUnit}");
        }

        public ThemeBuilder SetHeaderMaximumHeight(string height = "50px")
        {
            Theme.HeaderMaxHeight = height;
            return this;
        }

        public ThemeBuilder SetHeaderMaximumHeight(float height = 50, string dimensionUnit = "px")
        {
            return SetHeaderMaximumHeight($"{height}{dimensionUnit}");
        }

        public ThemeBuilder SetHeaderFontFamily(params string[] fontFamilies)
        {
            Theme.HeaderFontFamily = string.Join(", ", fontFamilies);
            return this;
        }
        #endregion

        #region Main
        public ThemeBuilder SetMainColor(Color color)
        {
            Theme.MainColor = color;
            return this;
        }

        public ThemeBuilder SetMainBackground(string background)
        {
            Theme.MainBackground = background;
            return this;
        }

        public ThemeBuilder SetMainBackgroundColor(Color backgroundColor)
        {
            return SetMainBackground(backgroundColor.ToColorString());
        }

        public ThemeBuilder SetMainBackgroundLinearGradient(Gradient gradient, float gradientAngle = 0.0f)
        {
            return SetMainBackground(gradient.ToLinearGradientString(gradientAngle));
        }

        public ThemeBuilder SetMainBackgroundRadialGradient(Gradient gradient, string positioning = "circle at center")
        {
            return SetMainBackground(gradient.ToRadialGradientString(positioning));
        }

        public ThemeBuilder SetMainMinimumWidth(string width = "300px")
        {
            Theme.MainMinWidth = width;
            return this;
        }

        public ThemeBuilder SetMainMinimumWidth(float width = 300, string dimensionUnit = "px")
        {
            return SetMainMinimumWidth($"{width}{dimensionUnit}");
        }

        public ThemeBuilder SetMainMaximumWidth(string width = "300px")
        {
            Theme.MainMaxWidth = width;
            return this;
        }

        public ThemeBuilder SetMainMaximumWidth(float width = 350, string dimensionUnit = "px")
        {
            return SetMainMaximumWidth($"{width}{dimensionUnit}");
        }
        #endregion

        public ThemeBuilder SetFonts(params string[] fonts)
        {
            Theme.Fonts = fonts.ToList();
            return this;
        }
       
        public static implicit operator Theme(ThemeBuilder builder) => builder.Theme;
        #endregion
    }
}
