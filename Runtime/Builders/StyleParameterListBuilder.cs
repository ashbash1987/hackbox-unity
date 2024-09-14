using System.Linq;
using UnityEngine;
using Hackbox.Parameters;

namespace Hackbox.Builders
{
    public sealed class StyleParameterListBuilder
    {
        public StyleParameterListBuilder()
        {
            ParameterList = new ParameterList();
        }

        public ParameterList ParameterList
        {
            get;
            private set;
        }

        #region Public Methods
        public static StyleParameterListBuilder Create()
        {
            return new StyleParameterListBuilder();
        }

        public StyleParameterListBuilder SetAlignment(string alignment = "start")
        {
            ParameterList.SetParameterValue("align", alignment);
            return this;
        }

        public StyleParameterListBuilder SetBackground(string background)
        {
            ParameterList.SetParameterValue("background", background);
            return this;
        }

        public StyleParameterListBuilder SetBackgroundColor(Color backgroundColor)
        {
            return SetBackground(backgroundColor.ToColorString());
        }

        public StyleParameterListBuilder SetBackgroundLinearGradient(Gradient gradient, float gradientAngle = 0.0f)
        {
            return SetBackground(gradient.ToLinearGradientString(gradientAngle));
        }

        public StyleParameterListBuilder SetBackgroundRadialGradient(Gradient gradient, string positioning = "circle at center")
        {
            return SetBackground(gradient.ToRadialGradientString(positioning));
        }

        public StyleParameterListBuilder SetBorder(string border = "2px solid black")
        {
            ParameterList.SetParameterValue("border", border);
            return this;
        }

        public StyleParameterListBuilder SetBorder(Color color, float width = 2.0f, string dimensionUnit = "px", string style = "solid")
        {
            return SetBorder($"{width}{dimensionUnit} {style} {color.ToColorString()}");
        }

        public StyleParameterListBuilder SetBorderRadius(string borderRadius = "10px")
        {
            ParameterList.SetParameterValue("borderRadius", borderRadius);
            return this;
        }

        public StyleParameterListBuilder SetBorderRadius(float radius = 10.0f, string dimensionUnit = "px")
        {
            return SetBorderRadius($"{radius}{dimensionUnit}");
        }

        public StyleParameterListBuilder SetColor(Color color)
        {
            ParameterList.SetParameterValue("color", color);
            return this;
        }

        public StyleParameterListBuilder SetFontFamily(params string[] fontFamilies)
        {
            ParameterList.SetParameterValue("fontFamily", string.Join(", ", fontFamilies));
            return this;
        }

        public StyleParameterListBuilder SetFontSize(string fontSize = "20px")
        {
            ParameterList.SetParameterValue("fontSize", fontSize);
            return this;
        }

        public StyleParameterListBuilder SetFontSize(float size = 20.0f, string dimensionUnit = "px")
        {
            return SetFontSize($"{size}{dimensionUnit}");
        }

        public StyleParameterListBuilder SetGrid(bool grid = false, int gridColumns = 1, string gridGap = "10px", string gridRowHeight = "1fr")
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

        public StyleParameterListBuilder SetGrid(bool grid = false, int gridColumns = 1, float gridGap = 10.0f, string gridGapDimensionUnit = "px", float gridRowHeight = 1.0f, string gridRowHeightDimensionUnit = "fr")
        {
            return SetGrid(grid, gridColumns, $"{gridGap}{gridGapDimensionUnit}", $"{gridRowHeight}{gridRowHeightDimensionUnit}");
        }

        public StyleParameterListBuilder SetHeight(string height = "100px")
        {
            ParameterList.SetParameterValue("height", height);
            return this;
        }

        public StyleParameterListBuilder SetHeight(float height = 100.0f, string dimensionUnit = "px")
        {
            return SetHeight($"{height}{dimensionUnit}");
        }

        public StyleParameterListBuilder SetHover(ParameterList parameterList)
        {
            ParameterList.SetParameterValue("hover", parameterList);
            return this;
        }

        public StyleParameterListBuilder SetMargin(string margin = "10px 0px")
        {
            ParameterList.SetParameterValue("margin", margin);
            return this;
        }

        public StyleParameterListBuilder SetMargin(params (float margin, string dimensionUnit)[] margins)
        {
            return SetMargin(string.Join(" ", margins.Select(x => $"{x.margin}{x.dimensionUnit}")));
        }

        public StyleParameterListBuilder SetPadding(string padding = "0px 10px")
        {
            ParameterList.SetParameterValue("padding", padding);
            return this;
        }

        public StyleParameterListBuilder SetPadding(params (float padding, string dimensionUnit)[] paddings)
        {
            return SetPadding(string.Join(" ", paddings.Select(x => $"{x.padding}{x.dimensionUnit}")));
        }

        public StyleParameterListBuilder SetWidth(string width = "100%")
        {
            ParameterList.SetParameterValue("width", width);
            return this;
        }

        public StyleParameterListBuilder SetWidth(float width = 100.0f, string dimensionUnit = "%")
        {
            return SetWidth($"{width}{dimensionUnit}");
        }

        public static implicit operator ParameterList(StyleParameterListBuilder builder) => builder.ParameterList;
        #endregion
    }
}