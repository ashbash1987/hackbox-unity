using System.Linq;
using UnityEngine;
using Hackbox.UI;

namespace Hackbox.Builders
{
    /// <summary>  
    /// A builder class for creating and configuring a <see cref="Theme"/>.  
    /// </summary>  
    public sealed class ThemeBuilder
    {
        /// <summary>  
        /// Initializes a new instance of the <see cref="ThemeBuilder"/> class with the specified name.  
        /// </summary>  
        /// <param name="name">The name of the theme.</param>  
        public ThemeBuilder(string name)
        {
            Theme = Theme.Create(name);
        }

        /// <summary>  
        /// Gets the <see cref="Theme"/> instance being built.  
        /// </summary>  
        public Theme Theme
        {
            get;
            private set;
        }

        #region Public Methods  
        /// <summary>  
        /// Creates a new <see cref="ThemeBuilder"/> instance with the specified name.  
        /// </summary>  
        /// <param name="name">The name of the theme.</param>  
        /// <returns>A new <see cref="ThemeBuilder"/> instance.</returns>  
        public static ThemeBuilder Create(string name)
        {
            return new ThemeBuilder(name);
        }

        #region Header  
        /// <summary>  
        /// Sets the header color of the theme.  
        /// </summary>  
        /// <param name="color">The color to set.</param>  
        /// <returns>The current <see cref="ThemeBuilder"/> instance.</returns>  
        public ThemeBuilder SetHeaderColor(Color color)
        {
            Theme.HeaderColor = color;
            return this;
        }

        /// <summary>  
        /// Sets the header background of the theme.  
        /// </summary>  
        /// <param name="background">The background to set.</param>  
        /// <returns>The current <see cref="ThemeBuilder"/> instance.</returns>  
        public ThemeBuilder SetHeaderBackground(string background)
        {
            Theme.HeaderBackground = background;
            return this;
        }

        /// <summary>  
        /// Sets the header background using a solid color.  
        /// </summary>  
        /// <param name="backgroundColor">The background color to set.</param>  
        /// <returns>The current <see cref="ThemeBuilder"/> instance.</returns>  
        public ThemeBuilder SetHeaderBackgroundColor(Color backgroundColor)
        {
            return SetHeaderBackground(backgroundColor.ToColorString());
        }

        /// <summary>  
        /// Sets the header background using a linear gradient.  
        /// </summary>  
        /// <param name="gradient">The gradient to set.</param>  
        /// <param name="gradientAngle">The angle of the gradient.</param>  
        /// <returns>The current <see cref="ThemeBuilder"/> instance.</returns>  
        public ThemeBuilder SetHeaderBackgroundLinearGradient(Gradient gradient, float gradientAngle = 0.0f)
        {
            return SetHeaderBackground(gradient.ToLinearGradientString(gradientAngle));
        }

        /// <summary>  
        /// Sets the header background using a radial gradient.  
        /// </summary>  
        /// <param name="gradient">The gradient to set.</param>  
        /// <param name="positioning">The positioning of the gradient.</param>  
        /// <returns>The current <see cref="ThemeBuilder"/> instance.</returns>  
        public ThemeBuilder SetHeaderBackgroundRadialGradient(Gradient gradient, string positioning = "circle at center")
        {
            return SetHeaderBackground(gradient.ToRadialGradientString(positioning));
        }

        /// <summary>
        /// Sets the header background using an image.
        /// </summary>
        /// <param name="url">The URL of the image.</param>
        /// <param name="scalingAndPositioning">The scaling and positioning of the image.</param>
        /// <returns>The current instance of <see cref="ThemeBuilder"/>.</returns>
        public ThemeBuilder SetHeaderBackgroundImage(string url, string scalingAndPositioning = "no-repeat center / cover")
        {
            return SetHeaderBackground(url.ToImageString(scalingAndPositioning));
        }

        /// <summary>  
        /// Sets the minimum height of the header.  
        /// </summary>  
        /// <param name="height">The minimum height to set.</param>  
        /// <returns>The current <see cref="ThemeBuilder"/> instance.</returns>  
        public ThemeBuilder SetHeaderMinimumHeight(string height = "50px")
        {
            Theme.HeaderMinHeight = height;
            return this;
        }

        /// <summary>  
        /// Sets the minimum height of the header.  
        /// </summary>  
        /// <param name="height">The minimum height to set.</param>  
        /// <param name="dimensionUnit">The unit of the height.</param>  
        /// <returns>The current <see cref="ThemeBuilder"/> instance.</returns>  
        public ThemeBuilder SetHeaderMinimumHeight(float height = 50, string dimensionUnit = "px")
        {
            return SetHeaderMinimumHeight($"{height}{dimensionUnit}");
        }

        /// <summary>  
        /// Sets the maximum height of the header.  
        /// </summary>  
        /// <param name="height">The maximum height to set.</param>  
        /// <returns>The current <see cref="ThemeBuilder"/> instance.</returns>  
        public ThemeBuilder SetHeaderMaximumHeight(string height = "50px")
        {
            Theme.HeaderMaxHeight = height;
            return this;
        }

        /// <summary>  
        /// Sets the maximum height of the header.  
        /// </summary>  
        /// <param name="height">The maximum height to set.</param>  
        /// <param name="dimensionUnit">The unit of the height.</param>  
        /// <returns>The current <see cref="ThemeBuilder"/> instance.</returns>  
        public ThemeBuilder SetHeaderMaximumHeight(float height = 50, string dimensionUnit = "px")
        {
            return SetHeaderMaximumHeight($"{height}{dimensionUnit}");
        }

        /// <summary>  
        /// Sets the font family of the header.  
        /// </summary>  
        /// <param name="fontFamilies">The font families to set.</param>  
        /// <returns>The current <see cref="ThemeBuilder"/> instance.</returns>  
        public ThemeBuilder SetHeaderFontFamily(params string[] fontFamilies)
        {
            Theme.HeaderFontFamily = string.Join(", ", fontFamilies);
            return this;
        }
        #endregion

        #region Main  
        /// <summary>  
        /// Sets the main color of the theme.  
        /// </summary>  
        /// <param name="color">The color to set.</param>  
        /// <returns>The current <see cref="ThemeBuilder"/> instance.</returns>  
        public ThemeBuilder SetMainColor(Color color)
        {
            Theme.MainColor = color;
            return this;
        }

        /// <summary>  
        /// Sets the main background of the theme.  
        /// </summary>  
        /// <param name="background">The background to set.</param>  
        /// <returns>The current <see cref="ThemeBuilder"/> instance.</returns>  
        public ThemeBuilder SetMainBackground(string background)
        {
            Theme.MainBackground = background;
            return this;
        }

        /// <summary>  
        /// Sets the main background using a solid color.
        /// </summary>  
        /// <param name="backgroundColor">The background color to set.</param>  
        /// <returns>The current <see cref="ThemeBuilder"/> instance.</returns>  
        public ThemeBuilder SetMainBackgroundColor(Color backgroundColor)
        {
            return SetMainBackground(backgroundColor.ToColorString());
        }

        /// <summary>  
        /// Sets the main background using a linear gradient.
        /// </summary>  
        /// <param name="gradient">The gradient to set.</param>  
        /// <param name="gradientAngle">The angle of the gradient.</param>  
        /// <returns>The current <see cref="ThemeBuilder"/> instance.</returns>  
        public ThemeBuilder SetMainBackgroundLinearGradient(Gradient gradient, float gradientAngle = 0.0f)
        {
            return SetMainBackground(gradient.ToLinearGradientString(gradientAngle));
        }

        /// <summary>  
        /// Sets the main background using a radial gradient.
        /// </summary>  
        /// <param name="gradient">The gradient to set.</param>  
        /// <param name="positioning">The positioning of the gradient.</param>  
        /// <returns>The current <see cref="ThemeBuilder"/> instance.</returns>  
        public ThemeBuilder SetMainBackgroundRadialGradient(Gradient gradient, string positioning = "circle at center")
        {
            return SetMainBackground(gradient.ToRadialGradientString(positioning));
        }

        /// <summary>
        /// Sets the main background using an image.
        /// </summary>
        /// <param name="url">The URL of the image.</param>
        /// <param name="scalingAndPositioning">The scaling and positioning of the image.</param>
        /// <returns>The current instance of <see cref="ThemeBuilder"/>.</returns>
        public ThemeBuilder SetMainBackgroundImage(string url, string scalingAndPositioning = "no-repeat center / cover")
        {
            return SetMainBackground(url.ToImageString(scalingAndPositioning));
        }

        /// <summary>  
        /// Sets the minimum width of the main section.  
        /// </summary>  
        /// <param name="width">The minimum width to set.</param>  
        /// <returns>The current <see cref="ThemeBuilder"/> instance.</returns>  
        public ThemeBuilder SetMainMinimumWidth(string width = "300px")
        {
            Theme.MainMinWidth = width;
            return this;
        }

        /// <summary>  
        /// Sets the minimum width of the main section.  
        /// </summary>  
        /// <param name="width">The minimum width to set.</param>  
        /// <param name="dimensionUnit">The unit of the width.</param>  
        /// <returns>The current <see cref="ThemeBuilder"/> instance.</returns>  
        public ThemeBuilder SetMainMinimumWidth(float width = 300, string dimensionUnit = "px")
        {
            return SetMainMinimumWidth($"{width}{dimensionUnit}");
        }

        /// <summary>  
        /// Sets the maximum width of the main section.  
        /// </summary>  
        /// <param name="width">The maximum width to set.</param>  
        /// <returns>The current <see cref="ThemeBuilder"/> instance.</returns>  
        public ThemeBuilder SetMainMaximumWidth(string width = "300px")
        {
            Theme.MainMaxWidth = width;
            return this;
        }

        /// <summary>  
        /// Sets the maximum width of the main section.  
        /// </summary>  
        /// <param name="width">The maximum width to set.</param>  
        /// <param name="dimensionUnit">The unit of the width.</param>  
        /// <returns>The current <see cref="ThemeBuilder"/> instance.</returns>  
        public ThemeBuilder SetMainMaximumWidth(float width = 350, string dimensionUnit = "px")
        {
            return SetMainMaximumWidth($"{width}{dimensionUnit}");
        }
        #endregion

        /// <summary>  
        /// Implicitly converts a <see cref="ThemeBuilder"/> to a <see cref="Theme"/>.  
        /// </summary>  
        /// <param name="builder">The <see cref="ThemeBuilder"/> instance to convert.</param>  
        /// <returns>The <see cref="Theme"/> instance.</returns>  
        public static implicit operator Theme(ThemeBuilder builder) => builder.Theme;
        #endregion
    }
}
