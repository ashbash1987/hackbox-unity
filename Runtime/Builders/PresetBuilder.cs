using Hackbox.UI;
using UnityEngine;

namespace Hackbox.Builders
{
    public sealed class PresetBuilder
    {
        public PresetBuilder(string name, Preset.PresetType presetType)
        {
            Preset = Preset.Create(name, presetType);
            Preset.ParameterList = ParameterListBuilder;
            Preset.StyleParameterList = StyleParameterListBuilder;
        }

        public Preset Preset
        {
            get;
            private set;
        }

        private readonly ParameterListBuilder ParameterListBuilder = new ParameterListBuilder();
        private readonly StyleParameterListBuilder StyleParameterListBuilder = new StyleParameterListBuilder();

        #region Public Methods
        public static PresetBuilder Create(string name, Preset.PresetType presetType)
        {
            return new PresetBuilder(name, presetType);
        }

        #region Parameters
        public PresetBuilder SetEvent(string eventName = "event")
        {
            ParameterListBuilder.SetEvent(eventName);
            return this;
        }

        public PresetBuilder SetKey(string key = "key")
        {
            ParameterListBuilder.SetKey(key);
            return this;
        }

        public PresetBuilder SetLabel(string label = "Sample text")
        {
            ParameterListBuilder.SetLabel(label);
            return this;
        }

        public PresetBuilder SetPersistent(bool persistent = false)
        {
            ParameterListBuilder.SetPersistent(persistent);
            return this;
        }

        public PresetBuilder SetText(string text = "Sample text")
        {
            ParameterListBuilder.SetText(text);
            return this;
        }
        #endregion

        #region Style Parameters
        public PresetBuilder SetAlignment(string alignment = "start")
        {
            StyleParameterListBuilder.SetAlignment(alignment);
            return this;
        }

        public PresetBuilder SetBackground(string background)
        {
            StyleParameterListBuilder.SetBackground(background);
            return this;
        }

        public PresetBuilder SetBackgroundColor(Color backgroundColor)
        {
            StyleParameterListBuilder.SetBackgroundColor(backgroundColor);
            return this;
        }

        public PresetBuilder SetBackgroundLinearGradient(Gradient gradient, float gradientAngle = 0.0f)
        {
            StyleParameterListBuilder.SetBackgroundLinearGradient(gradient, gradientAngle);
            return this;
        }

        public PresetBuilder SetBackgroundRadialGradient(Gradient gradient, string positioning = "circle at center")
        {
            StyleParameterListBuilder.SetBackgroundRadialGradient(gradient, positioning);
            return this;
        }

        public PresetBuilder SetBorder(string border = "2px solid black")
        {
            StyleParameterListBuilder.SetBorder(border);
            return this;
        }

        public PresetBuilder SetBorder(Color color, float width = 2.0f, string dimensionUnit = "px", string style = "solid")
        {
            StyleParameterListBuilder.SetBorder(color, width, dimensionUnit, style);
            return this;
        }

        public PresetBuilder SetBorderRadius(string borderRadius = "10px")
        {
            StyleParameterListBuilder.SetBorderRadius(borderRadius);
            return this;
        }

        public PresetBuilder SetBorderRadius(float radius = 10.0f, string dimensionUnit = "px")
        {
            StyleParameterListBuilder.SetBorderRadius(radius, dimensionUnit);
            return this;
        }

        public PresetBuilder SetColor(Color color)
        {
            StyleParameterListBuilder.SetColor(color);
            return this;
        }

        public PresetBuilder SetFontFamily(params string[] fontFamilies)
        {
            StyleParameterListBuilder.SetFontFamily(fontFamilies);
            return this;
        }

        public PresetBuilder SetFontSize(string fontSize = "20px")
        {
            StyleParameterListBuilder.SetFontSize(fontSize);
            return this;
        }

        public PresetBuilder SetFontSize(float size = 20.0f, string dimensionUnit = "px")
        {
            StyleParameterListBuilder.SetFontSize(size, dimensionUnit);
            return this;
        }

        public PresetBuilder SetGrid(bool grid = false, int gridColumns = 1, string gridGap = "10px", string gridRowHeight = "1fr")
        {
            StyleParameterListBuilder.SetGrid(grid, gridColumns, gridGap, gridRowHeight);
            return this;
        }

        public PresetBuilder SetGrid(bool grid = false, int gridColumns = 1, float gridGap = 10.0f, string gridGapDimensionUnit = "px", float gridRowHeight = 1.0f, string gridRowHeightDimensionUnit = "fr")
        {
            StyleParameterListBuilder.SetGrid(grid, gridColumns, gridGap, gridGapDimensionUnit, gridRowHeight, gridRowHeightDimensionUnit);
            return this;
        }

        public PresetBuilder SetHeight(string height = "100px")
        {
            StyleParameterListBuilder.SetHeight(height);
            return this;
        }

        public PresetBuilder SetHeight(float height = 100.0f, string dimensionUnit = "px")
        {
            StyleParameterListBuilder.SetHeight(height, dimensionUnit);
            return this;
        }

        public PresetBuilder SetMargin(string margin = "10px 0px")
        {
            StyleParameterListBuilder.SetMargin(margin);
            return this;
        }

        public PresetBuilder SetMargin(params (float margin, string dimensionUnit)[] margins)
        {
            StyleParameterListBuilder.SetMargin(margins);
            return this;
        }

        public PresetBuilder SetPadding(string padding = "0px 10px")
        {
            StyleParameterListBuilder.SetPadding(padding);
            return this;
        }

        public PresetBuilder SetPadding(params (float padding, string dimensionUnit)[] paddings)
        {
            StyleParameterListBuilder.SetPadding(paddings);
            return this;
        }

        public PresetBuilder SetWidth(string width = "100%")
        {
            StyleParameterListBuilder.SetWidth(width);
            return this;
        }

        public PresetBuilder SetWidth(float width = 100.0f, string dimensionUnit = "%")
        {
            StyleParameterListBuilder.SetWidth(width, dimensionUnit);
            return this;
        }
        #endregion

        public static implicit operator Preset(PresetBuilder builder) => builder.Preset;
        #endregion
    }
}
