using System.Linq;
using Hackbox.Parameters;
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

        private Preset.PresetType PresetType => Preset.Type;

        private readonly ParameterListBuilder ParameterListBuilder = new ParameterListBuilder();
        private readonly StyleParameterListBuilder StyleParameterListBuilder = new StyleParameterListBuilder();

        #region Public Methods
        public static PresetBuilder Create(string name, Preset.PresetType presetType)
        {
            return new PresetBuilder(name, presetType);
        }

        #region Parameters
        public PresetBuilder SetChoices(string[] choices, bool multiSelect = false, string submitLabel = null, ParameterList hoverStyleParameterList = null, ParameterList submitStyleParameterList = null)
        {
            Debug.Assert(PresetType == Preset.PresetType.Choices);

            ParameterListBuilder.SetChoices(choices.Select(x => new ChoicesParameter.Choice() { Label = x, Value = x }).ToList());
            return SetChoices(multiSelect, submitLabel, hoverStyleParameterList, submitStyleParameterList);
        }

        public PresetBuilder SetChoices((string label, string value)[] choices, bool multiSelect = false, string submitLabel = null, ParameterList hoverStyleParameterList = null, ParameterList submitStyleParameterList = null)
        {
            Debug.Assert(PresetType == Preset.PresetType.Choices);

            ParameterListBuilder.SetChoices(choices.Select(x => new ChoicesParameter.Choice() { Label = x.label, Value = x.value }).ToList());
            return SetChoices(multiSelect, submitLabel, hoverStyleParameterList, submitStyleParameterList);
        }

        public PresetBuilder SetChoices(string[] choiceLabels, string[] choiceValues, bool multiSelect = false, string submitLabel = null, ParameterList hoverStyleParameterList = null, ParameterList submitStyleParameterList = null)
        {
            return SetChoices(choiceLabels.Zip(choiceValues, (label, value) => (label, value)).ToArray(), multiSelect, submitLabel, hoverStyleParameterList, submitStyleParameterList);
        }

        public PresetBuilder SetChoices((string label, string value, string[] keys)[] choices, bool multiSelect = false, string submitLabel = null, ParameterList hoverStyleParameterList = null, ParameterList submitStyleParameterList = null)
        {
            Debug.Assert(PresetType == Preset.PresetType.Choices);

            ParameterListBuilder.SetChoices(choices.Select(x => new ChoicesParameter.Choice() { Label = x.label, Value = x.value, Keys = x.keys }).ToList());
            return SetChoices(multiSelect, submitLabel, hoverStyleParameterList, submitStyleParameterList);
        }

        public PresetBuilder SetChoices(string[] choiceLabels, string[] choiceValues, string[][] choiceKeys, bool multiSelect = false, string submitLabel = null, ParameterList hoverStyleParameterList = null, ParameterList submitStyleParameterList = null)
        {
            return SetChoices(choiceLabels.Zip(choiceValues, (label, value) => (label, value)).Zip(choiceKeys, (labelValue, keys) => (labelValue.label, labelValue.value, keys)).ToArray(), multiSelect, submitLabel, hoverStyleParameterList, submitStyleParameterList);
        }

        public PresetBuilder SetEvent(string eventName)
        {
            ParameterListBuilder.SetEvent(eventName);
            return this;
        }

        public PresetBuilder SetLabel(string label)
        {
            ParameterListBuilder.SetLabel(label);
            return this;
        }

        public PresetBuilder SetMinimum(int minimumValue)
        {
            Debug.Assert(PresetType == Preset.PresetType.Range);

            ParameterListBuilder.SetMinimum(minimumValue);
            return this;
        }

        public PresetBuilder SetMaximum(int maximumValue)
        {
            Debug.Assert(PresetType == Preset.PresetType.Range);

            ParameterListBuilder.SetMaximum(maximumValue);
            return this;
        }

        public PresetBuilder SetMultiSelect(bool multiSelect)
        {
            Debug.Assert(PresetType == Preset.PresetType.Choices);

            ParameterListBuilder.SetMultiSelect(multiSelect);
            return this;
        }

        public PresetBuilder SetPersistent(bool persistent)
        {
            Debug.Assert(PresetType == Preset.PresetType.TextInput);

            ParameterListBuilder.SetPersistent(persistent);
            return this;
        }

        public PresetBuilder SetRange(int minimumValue, int maximumValue)
        {
            Debug.Assert(PresetType == Preset.PresetType.Range);

            ParameterListBuilder.SetRange(minimumValue, maximumValue);
            return this;
        }

        public PresetBuilder SetStep(int step)
        {
            Debug.Assert(PresetType == Preset.PresetType.Range);

            ParameterListBuilder.SetStep(step);
            return this;
        }

        public PresetBuilder SetSubmit(ParameterList parameterList)
        {
            Debug.Assert(PresetType == Preset.PresetType.Choices);

            ParameterListBuilder.SetSubmit(parameterList);
            return this;
        }

        public PresetBuilder SetText(string text)
        {
            ParameterListBuilder.SetText(text);
            return this;
        }

        public PresetBuilder SetType(string type)
        {
            Debug.Assert(PresetType == Preset.PresetType.TextInput);

            ParameterListBuilder.SetType(type);
            return this;
        }
        #endregion

        #region Style Parameters
        public PresetBuilder SetAlignment(string alignment)
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

        public PresetBuilder SetBorder(string border)
        {
            StyleParameterListBuilder.SetBorder(border);
            return this;
        }

        public PresetBuilder SetBorder(Color color, float width = 2.0f, string dimensionUnit = "px", string style = "solid")
        {
            StyleParameterListBuilder.SetBorder(color, width, dimensionUnit, style);
            return this;
        }

        public PresetBuilder SetBorderRadius(string borderRadius)
        {
            StyleParameterListBuilder.SetBorderRadius(borderRadius);
            return this;
        }

        public PresetBuilder SetBorderRadius(float radius, string dimensionUnit = "px")
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

        public PresetBuilder SetFontSize(string fontSize)
        {
            StyleParameterListBuilder.SetFontSize(fontSize);
            return this;
        }

        public PresetBuilder SetFontSize(float size, string dimensionUnit = "px")
        {
            StyleParameterListBuilder.SetFontSize(size, dimensionUnit);
            return this;
        }

        public PresetBuilder SetGrid(bool grid, int gridColumns = 1, string gridGap = "10px", string gridRowHeight = "1fr")
        {
            Debug.Assert(PresetType == Preset.PresetType.Choices);

            StyleParameterListBuilder.SetGrid(grid, gridColumns, gridGap, gridRowHeight);
            return this;
        }

        public PresetBuilder SetGrid(bool grid, int gridColumns = 1, float gridGap = 10.0f, string gridGapDimensionUnit = "px", float gridRowHeight = 1.0f, string gridRowHeightDimensionUnit = "fr")
        {
            Debug.Assert(PresetType == Preset.PresetType.Choices);

            StyleParameterListBuilder.SetGrid(grid, gridColumns, gridGap, gridGapDimensionUnit, gridRowHeight, gridRowHeightDimensionUnit);
            return this;
        }

        public PresetBuilder SetHeight(string height)
        {
            StyleParameterListBuilder.SetHeight(height);
            return this;
        }

        public PresetBuilder SetHeight(float height, string dimensionUnit = "px")
        {
            StyleParameterListBuilder.SetHeight(height, dimensionUnit);
            return this;
        }

        public PresetBuilder SetHover(ParameterList parameterList)
        {
            Debug.Assert(PresetType == Preset.PresetType.Choices);

            StyleParameterListBuilder.SetHover(parameterList);
            return this;
        }

        public PresetBuilder SetMargin(string margin)
        {
            StyleParameterListBuilder.SetMargin(margin);
            return this;
        }

        public PresetBuilder SetMargin(params (float margin, string dimensionUnit)[] margins)
        {
            StyleParameterListBuilder.SetMargin(margins);
            return this;
        }

        public PresetBuilder SetPadding(string padding)
        {
            StyleParameterListBuilder.SetPadding(padding);
            return this;
        }

        public PresetBuilder SetPadding(params (float padding, string dimensionUnit)[] paddings)
        {
            StyleParameterListBuilder.SetPadding(paddings);
            return this;
        }

        public PresetBuilder SetWidth(string width)
        {
            StyleParameterListBuilder.SetWidth(width);
            return this;
        }

        public PresetBuilder SetWidth(float width, string dimensionUnit = "%")
        {
            StyleParameterListBuilder.SetWidth(width, dimensionUnit);
            return this;
        }
        #endregion

        public static implicit operator Preset(PresetBuilder builder)
        {
            return builder.Preset;
        }
        #endregion

        #region Private Methods
        private PresetBuilder SetChoices(bool multiSelect, string submitLabel, ParameterList hoverStyleParameterList, ParameterList submitStyleParameterList)
        {
            if (multiSelect)
            {
                ParameterListBuilder.SetMultiSelect(multiSelect);

                if (!string.IsNullOrEmpty(submitLabel) || submitStyleParameterList != null)
                {
                    ParameterListBuilder.SetSubmit(ParameterListBuilder.Create().SetLabel(submitLabel).SetStyle(submitStyleParameterList));
                }
            }

            if (hoverStyleParameterList != null)
            {
                StyleParameterListBuilder.SetHover(hoverStyleParameterList);
            }

            return this;
        }
        #endregion
    }
}
