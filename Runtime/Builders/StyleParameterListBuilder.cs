using System.Linq;
using UnityEngine;
using Hackbox.Parameters;

namespace Hackbox.Builders
{
    /// <summary>
    /// Builder class for constructing a ParameterList with various style parameters.
    /// </summary>
    public sealed class StyleParameterListBuilder
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="StyleParameterListBuilder"/> class.
        /// </summary>
        public StyleParameterListBuilder()
        {
            ParameterList = new ParameterList();
        }

        /// <summary>
        /// Gets the ParameterList being built.
        /// </summary>
        public ParameterList ParameterList
        {
            get;
            private set;
        }

        #region Public Methods
        /// <summary>
        /// Creates a new instance of the <see cref="StyleParameterListBuilder"/> class.
        /// </summary>
        /// <returns>A new instance of <see cref="StyleParameterListBuilder"/>.</returns>
        public static StyleParameterListBuilder Create()
        {
            return new StyleParameterListBuilder();
        }

        /// <summary>
        /// Sets the 'align' parameter.
        /// </summary>
        /// <param name="alignment">The alignment to set.</param>
        /// <returns>The current instance of <see cref="StyleParameterListBuilder"/>.</returns>
        public StyleParameterListBuilder SetAlignment(string alignment)
        {
            ParameterList.SetParameterValue("align", alignment);
            return this;
        }

        /// <summary>
        /// Sets the 'background' parameter.
        /// </summary>
        /// <param name="background">The background to set.</param>
        /// <returns>The current instance of <see cref="StyleParameterListBuilder"/>.</returns>
        public StyleParameterListBuilder SetBackground(string background)
        {
            ParameterList.SetParameterValue("background", background);
            return this;
        }

        /// <summary>
        /// Sets the 'background' parameter using a solid color.
        /// </summary>
        /// <param name="backgroundColor">The background color to set.</param>
        /// <returns>The current instance of <see cref="StyleParameterListBuilder"/>.</returns>
        public StyleParameterListBuilder SetBackgroundColor(Color backgroundColor)
        {
            return SetBackground(backgroundColor.ToColorString());
        }

        /// <summary>
        /// Sets the 'background' parameter using a linear gradient.
        /// </summary>
        /// <param name="gradient">The gradient to set.</param>
        /// <param name="gradientAngle">The gradient angle to set.</param>
        /// <returns>The current instance of <see cref="StyleParameterListBuilder"/>.</returns>
        public StyleParameterListBuilder SetBackgroundLinearGradient(Gradient gradient, float gradientAngle = 0.0f)
        {
            return SetBackground(gradient.ToLinearGradientString(gradientAngle));
        }

        /// <summary>
        /// Sets the 'background' parameter using a radial gradient.
        /// </summary>
        /// <param name="gradient">The gradient to set.</param>
        /// <param name="positioning">The positioning to set.</param>
        /// <returns>The current instance of <see cref="StyleParameterListBuilder"/>.</returns>
        public StyleParameterListBuilder SetBackgroundRadialGradient(Gradient gradient, string positioning = "circle at center")
        {
            return SetBackground(gradient.ToRadialGradientString(positioning));
        }

        /// <summary>
        /// Sets the 'border' parameter.
        /// </summary>
        /// <param name="border">The border to set.</param>
        /// <returns>The current instance of <see cref="StyleParameterListBuilder"/>.</returns>
        public StyleParameterListBuilder SetBorder(string border)
        {
            ParameterList.SetParameterValue("border", border);
            return this;
        }

        /// <summary>
        /// Sets the 'border' parameter with color, width, and style.
        /// </summary>
        /// <param name="color">The color to set.</param>
        /// <param name="width">The width to set.</param>
        /// <param name="dimensionUnit">The dimension unit to set.</param>
        /// <param name="style">The style to set.</param>
        /// <returns>The current instance of <see cref="StyleParameterListBuilder"/>.</returns>
        public StyleParameterListBuilder SetBorder(Color color, float width = 2.0f, string dimensionUnit = "px", string style = "solid")
        {
            return SetBorder($"{width}{dimensionUnit} {style} {color.ToColorString()}");
        }

        /// <summary>
        /// Sets the 'borderRadius' parameter.
        /// </summary>
        /// <param name="borderRadius">The border radius to set.</param>
        /// <returns>The current instance of <see cref="StyleParameterListBuilder"/>.</returns>
        public StyleParameterListBuilder SetBorderRadius(string borderRadius)
        {
            ParameterList.SetParameterValue("radius", borderRadius);
            ParameterList.SetParameterValue("borderRadius", borderRadius);
            return this;
        }

        /// <summary>
        /// Sets the 'borderRadius' parameter with radius and dimension unit.
        /// </summary>
        /// <param name="radius">The radius to set.</param>
        /// <param name="dimensionUnit">The dimension unit to set.</param>
        /// <returns>The current instance of <see cref="StyleParameterListBuilder"/>.</returns>
        public StyleParameterListBuilder SetBorderRadius(float radius, string dimensionUnit = "px")
        {
            return SetBorderRadius($"{radius}{dimensionUnit}");
        }

        /// <summary>
        /// Sets the 'color' parameter.
        /// </summary>
        /// <param name="color">The color to set.</param>
        /// <returns>The current instance of <see cref="StyleParameterListBuilder"/>.</returns>
        public StyleParameterListBuilder SetColor(Color color)
        {
            ParameterList.SetParameterValue("color", color);
            return this;
        }

        /// <summary>
        /// Sets the 'fontFamily' parameter.
        /// </summary>
        /// <param name="fontFamilies">The font families to set.</param>
        /// <returns>The current instance of <see cref="StyleParameterListBuilder"/>.</returns>
        public StyleParameterListBuilder SetFontFamily(params string[] fontFamilies)
        {
            ParameterList.SetParameterValue("fontFamily", string.Join(", ", fontFamilies));
            return this;
        }

        /// <summary>
        /// Sets the 'fontSize' parameter.
        /// </summary>
        /// <param name="fontSize">The font size to set.</param>
        /// <returns>The current instance of <see cref="StyleParameterListBuilder"/>.</returns>
        public StyleParameterListBuilder SetFontSize(string fontSize)
        {
            ParameterList.SetParameterValue("fontSize", fontSize);
            return this;
        }

        /// <summary>
        /// Sets the 'fontSize' parameter with size and dimension unit.
        /// </summary>
        /// <param name="size">The size to set.</param>
        /// <param name="dimensionUnit">The dimension unit to set.</param>
        /// <returns>The current instance of <see cref="StyleParameterListBuilder"/>.</returns>
        public StyleParameterListBuilder SetFontSize(float size, string dimensionUnit = "px")
        {
            return SetFontSize($"{size}{dimensionUnit}");
        }

        /// <summary>
        /// Sets the 'grid', 'gridColumns', 'gridGap', and'gridRowHeight' parameters.
        /// </summary>
        /// <param name="grid">The grid value to set.</param>
        /// <param name="gridColumns">The grid columns to set.</param>
        /// <param name="gridGap">The grid gap to set.</param>
        /// <param name="gridRowHeight">The grid row height to set.</param>
        /// <returns>The current instance of <see cref="StyleParameterListBuilder"/>.</returns>
        public StyleParameterListBuilder SetGrid(bool grid, int gridColumns = 1, string gridGap = "10px", string gridRowHeight = "1fr")
        {
            ParameterList.SetParameterValue("grid", grid);
            if (grid)
            {
                ParameterList.SetParameterValue("gridColumns", gridColumns);
                ParameterList.SetParameterValue("gridGap", gridGap);
                ParameterList.SetParameterValue("gridRowHeight", gridRowHeight);
            }
            return this;
        }

        /// <summary>
        /// Sets the 'grid', 'gridColumns', 'gridGap', and'gridRowHeight' parameters.
        /// </summary>
        /// <param name="grid">The grid value to set.</param>
        /// <param name="gridColumns">The grid columns to set.</param>
        /// <param name="gridGap">The grid gap to set.</param>
        /// <param name="gridGapDimensionUnit">The grid gap dimension unit to set.</param>
        /// <param name="gridRowHeight">The grid row height to set.</param>
        /// <param name="gridRowHeightDimensionUnit">The grid row height dimension unit to set.</param>
        /// <returns>The current instance of <see cref="StyleParameterListBuilder"/>.</returns>
        public StyleParameterListBuilder SetGrid(bool grid, int gridColumns = 1, float gridGap = 10.0f, string gridGapDimensionUnit = "px", float gridRowHeight = 1.0f, string gridRowHeightDimensionUnit = "fr")
        {
            return SetGrid(grid, gridColumns, $"{gridGap}{gridGapDimensionUnit}", $"{gridRowHeight}{gridRowHeightDimensionUnit}");
        }

        /// <summary>
        /// Sets the 'height' parameter.
        /// </summary>
        /// <param name="height">The height to set.</param>
        /// <returns>The current instance of <see cref="StyleParameterListBuilder"/>.</returns>
        public StyleParameterListBuilder SetHeight(string height)
        {
            ParameterList.SetParameterValue("height", height);
            return this;
        }

        /// <summary>
        /// Sets the 'height' parameter with height and dimension unit.
        /// </summary>
        /// <param name="height">The height to set.</param>
        /// <param name="dimensionUnit">The dimension unit to set.</param>
        /// <returns>The current instance of <see cref="StyleParameterListBuilder"/>.</returns>
        public StyleParameterListBuilder SetHeight(float height, string dimensionUnit = "px")
        {
            return SetHeight($"{height}{dimensionUnit}");
        }

        /// <summary>
        /// Sets the 'hover' parameter.
        /// </summary>
        /// <param name="parameterList">The hover parameter list to set.</param>
        /// <returns>The current instance of <see cref="StyleParameterListBuilder"/>.</returns>
        public StyleParameterListBuilder SetHover(ParameterList parameterList)
        {
            ParameterList.SetParameterValue("hover", parameterList);
            return this;
        }

        /// <summary>
        /// Sets the 'margin' parameter.
        /// </summary>
        /// <param name="margin">The margin to set.</param>
        /// <returns>The current instance of <see cref="StyleParameterListBuilder"/>.</returns>
        public StyleParameterListBuilder SetMargin(string margin)
        {
            ParameterList.SetParameterValue("margin", margin);
            return this;
        }

        /// <summary>
        /// Sets the 'margin' parameter with separate margins and dimension units.
        /// </summary>
        /// <param name="margins">The margins to set.</param>
        /// <returns>The current instance of <see cref="StyleParameterListBuilder"/>.</returns>
        public StyleParameterListBuilder SetMargin(params (float margin, string dimensionUnit)[] margins)
        {
            return SetMargin(string.Join(" ", margins.Select(x => $"{x.margin}{x.dimensionUnit}")));
        }

        /// <summary>
        /// Sets the 'padding' parameter.
        /// </summary>
        /// <param name="padding">The padding to set.</param>
        /// <returns>The current instance of <see cref="StyleParameterListBuilder"/>.</returns>
        public StyleParameterListBuilder SetPadding(string padding)
        {
            ParameterList.SetParameterValue("padding", padding);
            return this;
        }

        /// <summary>
        /// Sets the 'padding' parameter with separate paddings and dimension units.
        /// </summary>
        /// <param name="paddings">The paddings to set.</param>
        /// <returns>The current instance of <see cref="StyleParameterListBuilder"/>.</returns>
        public StyleParameterListBuilder SetPadding(params (float padding, string dimensionUnit)[] paddings)
        {
            return SetPadding(string.Join(" ", paddings.Select(x => $"{x.padding}{x.dimensionUnit}")));
        }

        /// <summary>
        /// Sets the 'shadow' parameter.
        /// </summary>
        /// <param name="shadow">The shadow to set.</param>
        /// <returns>The current instance of <see cref="StyleParameterListBuilder"/>.</returns>
        public StyleParameterListBuilder SetShadow(string shadow)
        {
            ParameterList.SetParameterValue("shadow", shadow);
            return this;
        }

        /// <summary>
        /// Sets the 'width' parameter.
        /// </summary>
        /// <param name="width">The width to set.</param>
        /// <returns>The current instance of <see cref="StyleParameterListBuilder"/>.</returns>
        public StyleParameterListBuilder SetWidth(string width)
        {
            ParameterList.SetParameterValue("width", width);
            return this;
        }

        /// <summary>
        /// Sets the 'width' parameter with width and dimension unit.
        /// </summary>
        /// <param name="width">The width to set.</param>
        /// <param name="dimensionUnit">The dimension unit to set.</param>
        /// <returns>The current instance of <see cref="StyleParameterListBuilder"/>.</returns>
        public StyleParameterListBuilder SetWidth(float width, string dimensionUnit = "%")
        {
            return SetWidth($"{width}{dimensionUnit}");
        }

        /// <summary>
        /// Implicitly converts a <see cref="StyleParameterListBuilder"/> to a <see cref="ParameterList"/>.
        /// </summary>
        /// <param name="builder">The builder to convert.</param>
        public static implicit operator ParameterList(StyleParameterListBuilder builder) => builder.ParameterList;
        #endregion
    }
}
