using System.Linq;
using Hackbox.Parameters;
using Hackbox.UI;
using UnityEngine;

namespace Hackbox.Builders
{
    /// <summary>
    /// Builder class for creating and configuring Preset objects with various parameters and style parameters.
    /// </summary>
    public sealed class PresetBuilder
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PresetBuilder"/> class.
        /// </summary>
        /// <param name="name">The name of the preset.</param>
        /// <param name="presetType">The type of the preset.</param>
        public PresetBuilder(string name, Preset.PresetType presetType)
        {
            Preset = Preset.Create(name, presetType);
            Preset.ParameterList = ParameterListBuilder;
            Preset.StyleParameterList = StyleParameterListBuilder;
        }

        /// <summary>
        /// Gets the Preset object being built.
        /// </summary>
        public Preset Preset
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the type of the preset.
        /// </summary>
        private Preset.PresetType PresetType => Preset.Type;

        private readonly ParameterListBuilder ParameterListBuilder = new ParameterListBuilder();
        private readonly StyleParameterListBuilder StyleParameterListBuilder = new StyleParameterListBuilder();

        #region Public Methods
        /// <summary>
        /// Creates a new instance of the <see cref="PresetBuilder"/> class.
        /// </summary>
        /// <param name="name">The name of the preset.</param>
        /// <param name="presetType">The type of the preset.</param>
        /// <returns>A new instance of <see cref="PresetBuilder"/>.</returns>
        public static PresetBuilder Create(string name, Preset.PresetType presetType)
        {
            return new PresetBuilder(name, presetType);
        }

        #region Parameters
        /// <summary>
        /// Sets the choices for the preset.
        /// </summary>
        /// <param name="choices">An array of choice labels.</param>
        /// <param name="multiSelect">Indicates if multiple selections are allowed.</param>
        /// <param name="submitLabel">The label for the submit button.</param>
        /// <param name="hoverStyleParameterList">The style parameters for hover state.</param>
        /// <param name="submitStyleParameterList">The style parameters for submit button.</param>
        /// <returns>The current instance of <see cref="PresetBuilder"/>.</returns>
        public PresetBuilder SetChoices(string[] choices, bool multiSelect = false, string submitLabel = null, ParameterList hoverStyleParameterList = null, ParameterList submitStyleParameterList = null)
        {
            Debug.Assert(PresetType == Preset.PresetType.Choices || PresetType == Preset.PresetType.Sort);

            ParameterListBuilder.SetChoices(choices.Select(x => new ChoicesParameter.Choice() { Label = x, Value = x }).ToList());
            return SetChoices(multiSelect, submitLabel, hoverStyleParameterList, submitStyleParameterList);
        }

        /// <summary>
        /// Sets the choices for the preset.
        /// </summary>
        /// <param name="choices">An array of tuples containing choice labels and values.</param>
        /// <param name="multiSelect">Indicates if multiple selections are allowed.</param>
        /// <param name="submitLabel">The label for the submit button.</param>
        /// <param name="hoverStyleParameterList">The style parameters for hover state.</param>
        /// <param name="submitStyleParameterList">The style parameters for submit button.</param>
        /// <returns>The current instance of <see cref="PresetBuilder"/>.</returns>
        public PresetBuilder SetChoices((string label, string value)[] choices, bool multiSelect = false, string submitLabel = null, ParameterList hoverStyleParameterList = null, ParameterList submitStyleParameterList = null)
        {
            Debug.Assert(PresetType == Preset.PresetType.Choices || PresetType == Preset.PresetType.Sort);

            ParameterListBuilder.SetChoices(choices.Select(x => new ChoicesParameter.Choice() { Label = x.label, Value = x.value }).ToList());
            return SetChoices(multiSelect, submitLabel, hoverStyleParameterList, submitStyleParameterList);
        }

        /// <summary>
        /// Sets the choices for the preset.
        /// </summary>
        /// <param name="choiceLabels">An array of choice labels.</param>
        /// <param name="choiceValues">An array of choice values.</param>
        /// <param name="multiSelect">Indicates if multiple selections are allowed.</param>
        /// <param name="submitLabel">The label for the submit button.</param>
        /// <param name="hoverStyleParameterList">The style parameters for hover state.</param>
        /// <param name="submitStyleParameterList">The style parameters for submit button.</param>
        /// <returns>The current instance of <see cref="PresetBuilder"/>.</returns>
        public PresetBuilder SetChoices(string[] choiceLabels, string[] choiceValues, bool multiSelect = false, string submitLabel = null, ParameterList hoverStyleParameterList = null, ParameterList submitStyleParameterList = null)
        {
            return SetChoices(choiceLabels.Zip(choiceValues, (label, value) => (label, value)).ToArray(), multiSelect, submitLabel, hoverStyleParameterList, submitStyleParameterList);
        }

        /// <summary>
        /// Sets the choices for the preset.
        /// </summary>
        /// <param name="choices">An array of tuples containing choice labels, values, and keys.</param>
        /// <param name="multiSelect">Indicates if multiple selections are allowed.</param>
        /// <param name="submitLabel">The label for the submit button.</param>
        /// <param name="hoverStyleParameterList">The style parameters for hover state.</param>
        /// <param name="submitStyleParameterList">The style parameters for submit button.</param>
        /// <returns>The current instance of <see cref="PresetBuilder"/>.</returns>
        public PresetBuilder SetChoices((string label, string value, string[] keys)[] choices, bool multiSelect = false, string submitLabel = null, ParameterList hoverStyleParameterList = null, ParameterList submitStyleParameterList = null)
        {
            Debug.Assert(PresetType == Preset.PresetType.Choices || PresetType == Preset.PresetType.Sort);

            ParameterListBuilder.SetChoices(choices.Select(x => new ChoicesParameter.Choice() { Label = x.label, Value = x.value, Keys = x.keys }).ToList());
            return SetChoices(multiSelect, submitLabel, hoverStyleParameterList, submitStyleParameterList);
        }

        /// <summary>
        /// Sets the choices for the preset.
        /// </summary>
        /// <param name="choiceLabels">An array of choice labels.</param>
        /// <param name="choiceValues">An array of choice values.</param>
        /// <param name="choiceKeys">An array of choice keys.</param>
        /// <param name="multiSelect">Indicates if multiple selections are allowed.</param>
        /// <param name="submitLabel">The label for the submit button.</param>
        /// <param name="hoverStyleParameterList">The style parameters for hover state.</param>
        /// <param name="submitStyleParameterList">The style parameters for submit button.</param>
        /// <returns>The current instance of <see cref="PresetBuilder"/>.</returns>
        public PresetBuilder SetChoices(string[] choiceLabels, string[] choiceValues, string[][] choiceKeys, bool multiSelect = false, string submitLabel = null, ParameterList hoverStyleParameterList = null, ParameterList submitStyleParameterList = null)
        {
            return SetChoices(choiceLabels.Zip(choiceValues, (label, value) => (label, value)).Zip(choiceKeys, (labelValue, keys) => (labelValue.label, labelValue.value, keys)).ToArray(), multiSelect, submitLabel, hoverStyleParameterList, submitStyleParameterList);
        }

        /// <summary>
        /// Sets the sort options for the preset.
        /// </summary>
        /// <param name="sortOptions">An array of sort option labels.</param>
        /// <param name="submitLabel">The label for the submit button.</param>
        /// <param name="hoverStyleParameterList">The style parameters for hover state.</param>
        /// <param name="submitStyleParameterList">The style parameters for submit button.</param>
        /// <returns>The current instance of <see cref="PresetBuilder"/>.</returns>
        public PresetBuilder SetSortOptions(string[] sortOptions, string submitLabel = null, ParameterList hoverStyleParameterList = null, ParameterList submitStyleParameterList = null)
        {
            return SetChoices(sortOptions, false, submitLabel, hoverStyleParameterList, submitStyleParameterList);
        }

        /// <summary>
        /// Sets the sort options for the preset.
        /// </summary>
        /// <param name="sortOptions">An array of tuples containing sort option labels and values.</param>
        /// <param name="submitLabel">The label for the submit button.</param>
        /// <param name="hoverStyleParameterList">The style parameters for hover state.</param>
        /// <param name="submitStyleParameterList">The style parameters for submit button.</param>
        /// <returns>The current instance of <see cref="PresetBuilder"/>.</returns>
        public PresetBuilder SetSortOptions((string label, string value)[] sortOptions, string submitLabel = null, ParameterList hoverStyleParameterList = null, ParameterList submitStyleParameterList = null)
        {
            return SetChoices(sortOptions, false, submitLabel, hoverStyleParameterList, submitStyleParameterList);
        }

        /// <summary>
        /// Sets the sort options for the preset.
        /// </summary>
        /// <param name="sortOptionLabels">An array of sort option labels.</param>
        /// <param name="sortOptionValues">An array of sort option values.</param>
        /// <param name="submitLabel">The label for the submit button.</param>
        /// <param name="hoverStyleParameterList">The style parameters for hover state.</param>
        /// <param name="submitStyleParameterList">The style parameters for submit button.</param>
        /// <returns>The current instance of <see cref="PresetBuilder"/>.</returns>
        public PresetBuilder SetSortOptions(string[] sortOptionLabels, string[] sortOptionValues, string submitLabel = null, ParameterList hoverStyleParameterList = null, ParameterList submitStyleParameterList = null)
        {
            return SetChoices(sortOptionLabels, sortOptionValues, false, submitLabel, hoverStyleParameterList, submitStyleParameterList);
        }

        /// <summary>
        /// Sets the event name for the preset.
        /// </summary>
        /// <param name="eventName">The name of the event.</param>
        /// <returns>The current instance of <see cref="PresetBuilder"/>.</returns>
        public PresetBuilder SetEvent(string eventName)
        {
            ParameterListBuilder.SetEvent(eventName);
            return this;
        }

        /// <summary>
        /// Sets the label for the preset.
        /// </summary>
        /// <param name="label">The label text.</param>
        /// <returns>The current instance of <see cref="PresetBuilder"/>.</returns>
        public PresetBuilder SetLabel(string label)
        {
            ParameterListBuilder.SetLabel(label);
            return this;
        }

        /// <summary>
        /// Sets the minimum value for the preset.
        /// </summary>
        /// <param name="minimumValue">The minimum value.</param>
        /// <returns>The current instance of <see cref="PresetBuilder"/>.</returns>
        public PresetBuilder SetMinimum(int minimumValue)
        {
            Debug.Assert(PresetType == Preset.PresetType.Range);

            ParameterListBuilder.SetMinimum(minimumValue);
            return this;
        }

        /// <summary>
        /// Sets the maximum value for the preset.
        /// </summary>
        /// <param name="maximumValue">The maximum value.</param>
        /// <returns>The current instance of <see cref="PresetBuilder"/>.</returns>
        public PresetBuilder SetMaximum(int maximumValue)
        {
            Debug.Assert(PresetType == Preset.PresetType.Range);

            ParameterListBuilder.SetMaximum(maximumValue);
            return this;
        }

        /// <summary>
        /// Sets whether multiple selections are allowed for the preset.
        /// </summary>
        /// <param name="multiSelect">Indicates if multiple selections are allowed.</param>
        /// <returns>The current instance of <see cref="PresetBuilder"/>.</returns>
        public PresetBuilder SetMultiSelect(bool multiSelect)
        {
            Debug.Assert(PresetType == Preset.PresetType.Choices);

            ParameterListBuilder.SetMultiSelect(multiSelect);
            return this;
        }

        /// <summary>
        /// Sets whether the preset is persistent.
        /// </summary>
        /// <param name="persistent">Indicates if the preset is persistent.</param>
        /// <returns>The current instance of <see cref="PresetBuilder"/>.</returns>
        public PresetBuilder SetPersistent(bool persistent)
        {
            Debug.Assert(PresetType == Preset.PresetType.TextInput);

            ParameterListBuilder.SetPersistent(persistent);
            return this;
        }

        /// <summary>
        /// Sets the minmum and maximum values (range) for the preset.
        /// </summary>
        /// <param name="minimumValue">The minimum value.</param>
        /// <param name="maximumValue">The maximum value.</param>
        /// <returns>The current instance of <see cref="PresetBuilder"/>.</returns>
        public PresetBuilder SetRange(int minimumValue, int maximumValue)
        {
            Debug.Assert(PresetType == Preset.PresetType.Range);

            ParameterListBuilder.SetRange(minimumValue, maximumValue);
            return this;
        }

        /// <summary>
        /// Sets the step value for the preset.
        /// </summary>
        /// <param name="step">The step value.</param>
        /// <returns>The current instance of <see cref="PresetBuilder"/>.</returns>
        public PresetBuilder SetStep(int step)
        {
            Debug.Assert(PresetType == Preset.PresetType.Range);

            ParameterListBuilder.SetStep(step);
            return this;
        }

        /// <summary>
        /// Sets the submit parameters for the preset.
        /// </summary>
        /// <param name="parameterList">The parameter list for the submit action.</param>
        /// <returns>The current instance of <see cref="PresetBuilder"/>.</returns>
        public PresetBuilder SetSubmit(ParameterList parameterList)
        {
            Debug.Assert(PresetType == Preset.PresetType.Choices);

            ParameterListBuilder.SetSubmit(parameterList);
            return this;
        }

        /// <summary>
        /// Sets the text for the preset.
        /// </summary>
        /// <param name="text">The text value.</param>
        /// <returns>The current instance of <see cref="PresetBuilder"/>.</returns>
        public PresetBuilder SetText(string text)
        {
            ParameterListBuilder.SetText(text);
            return this;
        }

        /// <summary>
        /// Sets the type for the preset.
        /// </summary>
        /// <param name="type">The type value.</param>
        /// <returns>The current instance of <see cref="PresetBuilder"/>.</returns>
        public PresetBuilder SetType(string type)
        {
            Debug.Assert(PresetType == Preset.PresetType.TextInput);

            ParameterListBuilder.SetType(type);
            return this;
        }
        #endregion

        #region Style Parameters
        /// Sets the alignment for the preset.
        /// </summary>
        /// <param name="alignment">The alignment value.</param>
        /// <returns>The current instance of <see cref="PresetBuilder"/>.</returns>
        public PresetBuilder SetAlignment(string alignment)
        {
            StyleParameterListBuilder.SetAlignment(alignment);
            return this;
        }

        /// <summary>
        /// Sets the background for the preset.
        /// </summary>
        /// <param name="background">The background value.</param>
        /// <returns>The current instance of <see cref="PresetBuilder"/>.</returns>
        public PresetBuilder SetBackground(string background)
        {
            StyleParameterListBuilder.SetBackground(background);
            return this;
        }

        /// <summary>
        /// Sets the background for the preset using a solid color.
        /// </summary>
        /// <param name="backgroundColor">The background color value.</param>
        /// <returns>The current instance of <see cref="PresetBuilder"/>.</returns>
        public PresetBuilder SetBackgroundColor(Color backgroundColor)
        {
            StyleParameterListBuilder.SetBackgroundColor(backgroundColor);
            return this;
        }

        /// <summary>
        /// Sets the background for the preset using a linear gradient.
        /// </summary>
        /// <param name="gradient">The gradient value.</param>
        /// <param name="gradientAngle">The gradient angle value.</param>
        /// <returns>The current instance of <see cref="PresetBuilder"/>.</returns>
        public PresetBuilder SetBackgroundLinearGradient(Gradient gradient, float gradientAngle = 0.0f)
        {
            StyleParameterListBuilder.SetBackgroundLinearGradient(gradient, gradientAngle);
            return this;
        }

        /// <summary>
        /// Sets the background for the preset using a radial gradient.
        /// </summary>
        /// <param name="gradient">The gradient value.</param>
        /// <param name="positioning">The positioning value.</param>
        /// <returns>The current instance of <see cref="PresetBuilder"/>.</returns>
        public PresetBuilder SetBackgroundRadialGradient(Gradient gradient, string positioning = "circle at center")
        {
            StyleParameterListBuilder.SetBackgroundRadialGradient(gradient, positioning);
            return this;
        }

        /// <summary>
        /// Sets the border for the preset.
        /// </summary>
        /// <param name="border">The border value.</param>
        /// <returns>The current instance of <see cref="PresetBuilder"/>.</returns>
        public PresetBuilder SetBorder(string border)
        {
            StyleParameterListBuilder.SetBorder(border);
            return this;
        }

        /// <summary>
        /// Sets the border for the preset.
        /// </summary>
        /// <param name="color">The border color value.</param>
        /// <param name="width">The border width value.</param>
        /// <param name="dimensionUnit">The dimension unit for the border width.</param>
        /// <param name="style">The border style value.</param>
        /// <returns>The current instance of <see cref="PresetBuilder"/>.</returns>
        public PresetBuilder SetBorder(Color color, float width = 2.0f, string dimensionUnit = "px", string style = "solid")
        {
            StyleParameterListBuilder.SetBorder(color, width, dimensionUnit, style);
            return this;
        }

        /// <summary>
        /// Sets the border radius for the preset.
        /// </summary>
        /// <param name="borderRadius">The border radius value.</param>
        /// <returns>The current instance of <see cref="PresetBuilder"/>.</returns>
        public PresetBuilder SetBorderRadius(string borderRadius)
        {
            StyleParameterListBuilder.SetBorderRadius(borderRadius);
            return this;
        }

        /// <summary>
        /// Sets the border radius for the preset.
        /// </summary>
        /// <param name="radius">The border radius value.</param>
        /// <param name="dimensionUnit">The dimension unit for the border radius.</param>
        /// <returns>The current instance of <see cref="PresetBuilder"/>.</returns>
        public PresetBuilder SetBorderRadius(float radius, string dimensionUnit = "px")
        {
            StyleParameterListBuilder.SetBorderRadius(radius, dimensionUnit);
            return this;
        }

        /// <summary>
        /// Sets the color for the preset.
        /// </summary>
        /// <param name="color">The color value.</param>
        /// <returns>The current instance of <see cref="PresetBuilder"/>.</returns>
        public PresetBuilder SetColor(Color color)
        {
            StyleParameterListBuilder.SetColor(color);
            return this;
        }

        /// <summary>
        /// Sets the font family for the preset.
        /// </summary>
        /// <param name="fontFamilies">The font family values.</param>
        /// <returns>The current instance of <see cref="PresetBuilder"/>.</returns>
        public PresetBuilder SetFontFamily(params string[] fontFamilies)
        {
            StyleParameterListBuilder.SetFontFamily(fontFamilies);
            return this;
        }

        /// <summary>
        /// Sets the font size for the preset.
        /// </summary>
        /// <param name="fontSize">The font size value.</param>
        /// <returns>The current instance of <see cref="PresetBuilder"/>.</returns>
        public PresetBuilder SetFontSize(string fontSize)
        {
            StyleParameterListBuilder.SetFontSize(fontSize);
            return this;
        }

        /// <summary>
        /// Sets the font size for the preset.
        /// </summary>
        /// <param name="size">The font size value.</param>
        /// <param name="dimensionUnit">The dimension unit for the font size.</param>
        /// <returns>The current instance of <see cref="PresetBuilder"/>.</returns>
        public PresetBuilder SetFontSize(float size, string dimensionUnit = "px")
        {
            StyleParameterListBuilder.SetFontSize(size, dimensionUnit);
            return this;
        }

        /// <summary>
        /// Sets the grid properties for the preset.
        /// </summary>
        /// <param name="grid">Indicates if grid is enabled.</param>
        /// <param name="gridColumns">The number of grid columns.</param>
        /// <param name="gridGap">The grid gap value.</param>
        /// <param name="gridRowHeight">The grid row height value.</param>
        /// <returns>The current instance of <see cref="PresetBuilder"/>.</returns>
        public PresetBuilder SetGrid(bool grid, int gridColumns = 1, string gridGap = "10px", string gridRowHeight = "1fr")
        {
            Debug.Assert(PresetType == Preset.PresetType.Choices);

            StyleParameterListBuilder.SetGrid(grid, gridColumns, gridGap, gridRowHeight);
            return this;
        }

        /// <summary>
        /// Sets the grid properties for the preset.
        /// </summary>
        /// <param name="grid">Indicates if grid is enabled.</param>
        /// <param name="gridColumns">The number of grid columns.</param>
        /// <param name="gridGap">The grid gap value.</param>
        /// <param name="gridGapDimensionUnit">The dimension unit for the grid gap.</param>
        /// <param name="gridRowHeight">The grid row height value.</param>
        /// <param name="gridRowHeightDimensionUnit">The dimension unit for the grid row height.</param>
        /// <returns>The current instance of <see cref="PresetBuilder"/>.</returns>
        public PresetBuilder SetGrid(bool grid, int gridColumns = 1, float gridGap = 10.0f, string gridGapDimensionUnit = "px", float gridRowHeight = 1.0f, string gridRowHeightDimensionUnit = "fr")
        {
            Debug.Assert(PresetType == Preset.PresetType.Choices);

            StyleParameterListBuilder.SetGrid(grid, gridColumns, gridGap, gridGapDimensionUnit, gridRowHeight, gridRowHeightDimensionUnit);
            return this;
        }

        /// <summary>
        /// Sets the height for the preset.
        /// </summary>
        /// <param name="height">The height value.</param>
        /// <returns>The current instance of <see cref="PresetBuilder"/>.</returns>
        public PresetBuilder SetHeight(string height)
        {
            StyleParameterListBuilder.SetHeight(height);
            return this;
        }

        /// <summary>
        /// Sets the height for the preset.
        /// </summary>
        /// <param name="height">The height value.</param>
        /// <param name="dimensionUnit">The dimension unit for the height.</param>
        /// <returns>The current instance of <see cref="PresetBuilder"/>.</returns>
        public PresetBuilder SetHeight(float height, string dimensionUnit = "px")
        {
            StyleParameterListBuilder.SetHeight(height, dimensionUnit);
            return this;
        }

        /// <summary>
        /// Sets the hover style parameters for the preset.
        /// </summary>
        /// <param name="parameterList">The parameter list for the hover state.</param>
        /// <returns>The current instance of <see cref="PresetBuilder"/>.</returns>
        public PresetBuilder SetHover(ParameterList parameterList)
        {
            Debug.Assert(PresetType == Preset.PresetType.Choices);

            StyleParameterListBuilder.SetHover(parameterList);
            return this;
        }

        /// <summary>
        /// Sets the margin for the preset.
        /// </summary>
        /// <param name="margin">The margin value.</param>
        /// <returns>The current instance of <see cref="PresetBuilder"/>.</returns>
        public PresetBuilder SetMargin(string margin)
        {
            StyleParameterListBuilder.SetMargin(margin);
            return this;
        }

        /// <summary>
        /// Sets the margin for the preset.
        /// </summary>
        /// <param name="margins">The margin values and their dimension units.</param>
        /// <returns>The current instance of <see cref="PresetBuilder"/>.</returns>
        public PresetBuilder SetMargin(params (float margin, string dimensionUnit)[] margins)
        {
            StyleParameterListBuilder.SetMargin(margins);
            return this;
        }

        /// <summary>
        /// Sets the padding for the preset.
        /// </summary>
        /// <param name="padding">The padding value.</param>
        /// <returns>The current instance of <see cref="PresetBuilder"/>.</returns>
        public PresetBuilder SetPadding(string padding)
        {
            StyleParameterListBuilder.SetPadding(padding);
            return this;
        }

        /// <summary>
        /// Sets the padding for the preset.
        /// </summary>
        /// <param name="paddings">The padding values and their dimension units.</param>
        /// <returns>The current instance of <see cref="PresetBuilder"/>.</returns>
        public PresetBuilder SetPadding(params (float padding, string dimensionUnit)[] paddings)
        {
            StyleParameterListBuilder.SetPadding(paddings);
            return this;
        }

        /// <summary>
        /// Sets the width for the preset.
        /// </summary>
        /// <param name="width">The width value.</param>
        /// <returns>The current instance of <see cref="PresetBuilder"/>.</returns>
        public PresetBuilder SetWidth(string width)
        {
            StyleParameterListBuilder.SetWidth(width);
            return this;
        }

        /// <summary>
        /// Sets the width for the preset.
        /// </summary>
        /// <param name="width">The width value.</param>
        /// <param name="dimensionUnit">The dimension unit for the width.</param>
        /// <returns>The current instance of <see cref="PresetBuilder"/>.</returns>
        public PresetBuilder SetWidth(float width, string dimensionUnit = "%")
        {
            StyleParameterListBuilder.SetWidth(width, dimensionUnit);
            return this;
        }

        /// <summary>
        /// Implicitly converts a <see cref="PresetBuilder"/> to a <see cref="Preset"/>.
        /// </summary>
        /// <param name="builder">The <see cref="PresetBuilder"/> instance.</param>
        public static implicit operator Preset(PresetBuilder builder)
        {
            return builder.Preset;
        }
        #endregion
        #endregion

        #region Private Methods
        /// <summary>
        /// Sets the choices for the preset.
        /// </summary>
        /// <param name="multiSelect">Indicates if multiple selections are allowed.</param>
        /// <param name="submitLabel">The label for the submit button.</param>
        /// <param name="hoverStyleParameterList">The style parameters for hover state.</param>
        /// <param name="submitStyleParameterList">The style parameters for submit button.</param>
        /// <returns>The current instance of <see cref="PresetBuilder"/>.</returns>
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
