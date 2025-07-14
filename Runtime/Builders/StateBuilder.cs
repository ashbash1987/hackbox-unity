using System;
using System.Linq;
using UnityEngine;
using Hackbox.Parameters;
using Hackbox.UI;

namespace Hackbox.Builders
{
    /// <summary>
    /// A builder class for constructing a State object with various configurations.
    /// </summary>
    public sealed class StateBuilder
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="StateBuilder"/> class with the specified theme.
        /// </summary>
        /// <param name="theme">The theme to be used for the state.</param>
        public StateBuilder(Theme theme)
        {
            State = new State(theme);
        }

        /// <summary>
        /// Gets the current state being built.
        /// </summary>
        public State State
        {
            get;
            private set;
        }

        #region Public Methods
        /// <summary>
        /// Creates a new instance of the <see cref="StateBuilder"/> class with the specified theme.
        /// </summary>
        /// <param name="theme">The theme to be used for the state.</param>
        /// <returns>A new instance of the <see cref="StateBuilder"/> class.</returns>
        public static StateBuilder Create(Theme theme)
        {
            return new StateBuilder(theme);
        }

        #region Header
        /// <summary>
        /// Sets the header color of the state.
        /// </summary>
        /// <param name="color">The color to set for the header.</param>
        /// <returns>The current instance of <see cref="StateBuilder"/>.</returns>
        public StateBuilder SetHeaderColor(Color color)
        {
            State.HeaderColor = color;
            return this;
        }

        /// <summary>
        /// Sets the header background of the state.
        /// </summary>
        /// <param name="background">The background to set for the header.</param>
        /// <returns>The current instance of <see cref="StateBuilder"/>.</returns>
        public StateBuilder SetHeaderBackground(string background)
        {
            State.HeaderBackground = background;
            return this;
        }

        /// <summary>
        /// Sets the header background color of the state.
        /// </summary>
        /// <param name="backgroundColor">The background color to set for the header.</param>
        /// <returns>The current instance of <see cref="StateBuilder"/>.</returns>
        public StateBuilder SetHeaderBackgroundColor(Color backgroundColor)
        {
            return SetHeaderBackground(backgroundColor.ToColorString());
        }

        /// <summary>
        /// Sets the header background to a linear gradient.
        /// </summary>
        /// <param name="gradient">The gradient to set for the header background.</param>
        /// <param name="gradientAngle">The angle of the gradient.</param>
        /// <returns>The current instance of <see cref="StateBuilder"/>.</returns>
        public StateBuilder SetHeaderBackgroundLinearGradient(Gradient gradient, float gradientAngle = 0.0f)
        {
            return SetHeaderBackground(gradient.ToLinearGradientString(gradientAngle));
        }

        /// <summary>
        /// Sets the header background to a radial gradient.
        /// </summary>
        /// <param name="gradient">The gradient to set for the header background.</param>
        /// <param name="positioning">The positioning of the radial gradient.</param>
        /// <returns>The current instance of <see cref="StateBuilder"/>.</returns>
        public StateBuilder SetHeaderBackgroundRadialGradient(Gradient gradient, string positioning = "circle at center")
        {
            return SetHeaderBackground(gradient.ToRadialGradientString(positioning));
        }

        /// <summary>
        /// Sets the header background to an image.
        /// </summary>
        /// <param name="url">The URL of the image.</param>
        /// <param name="scalingAndPositioning">The scaling and positioning of the image.</param>
        /// <returns>The current instance of <see cref="StateBuilder"/>.</returns>
        public StateBuilder SetHeaderBackgroundImage(string url, string scalingAndPositioning = "no-repeat center / cover")
        {
            return SetHeaderBackground(url.ToImageString(scalingAndPositioning));
        }

        /// <summary>
        /// Sets the header text of the state.
        /// </summary>
        /// <param name="text">The text to set for the header.</param>
        /// <returns>The current instance of <see cref="StateBuilder"/>.</returns>
        public StateBuilder SetHeaderText(string text)
        {
            State.HeaderText = text;
            return this;
        }

        /// <summary>
        /// Sets the minimum height of the header.
        /// </summary>
        /// <param name="height">The minimum height to set for the header.</param>
        /// <returns>The current instance of <see cref="StateBuilder"/>.</returns>
        public StateBuilder SetHeaderMinimumHeight(string height = "50px")
        {
            State.SetHeaderParameter("minHeight", height);
            return this;
        }

        /// <summary>
        /// Sets the minimum height of the header.
        /// </summary>
        /// <param name="height">The minimum height to set for the header.</param>
        /// <param name="dimensionUnit">The unit of the height dimension.</param>
        /// <returns>The current instance of <see cref="StateBuilder"/>.</returns>
        public StateBuilder SetHeaderMinimumHeight(float height = 50, string dimensionUnit = "px")
        {
            return SetHeaderMinimumHeight($"{height}{dimensionUnit}");
        }

        /// <summary>
        /// Sets the maximum height of the header.
        /// </summary>
        /// <param name="height">The maximum height to set for the header.</param>
        /// <returns>The current instance of <see cref="StateBuilder"/>.</returns>
        public StateBuilder SetHeaderMaximumHeight(string height = "50px")
        {
            State.SetHeaderParameter("maxHeight", height);
            return this;
        }

        /// <summary>
        /// Sets the maximum height of the header.
        /// </summary>
        /// <param name="height">The maximum height to set for the header.</param>
        /// <param name="dimensionUnit">The unit of the height dimension.</param>
        /// <returns>The current instance of <see cref="StateBuilder"/>.</returns>
        public StateBuilder SetHeaderMaximumHeight(float height = 50, string dimensionUnit = "px")
        {
            return SetHeaderMaximumHeight($"{height}{dimensionUnit}");
        }
        #endregion

        #region Main
        /// <summary>
        /// Sets the alignment of the main content.
        /// </summary>
        /// <param name="alignment">The alignment to set for the main content.</param>
        /// <returns>The current instance of <see cref="StateBuilder"/>.</returns>
        public StateBuilder SetMainAlignment(string alignment = "start")
        {
            State.MainAlignment = alignment;
            return this;
        }

        /// <summary>
        /// Sets the background of the main content.
        /// </summary>
        /// <param name="background">The background to set for the main content.</param>
        /// <returns>The current instance of <see cref="StateBuilder"/>.</returns>
        public StateBuilder SetMainBackground(string background)
        {
            State.MainBackground = background;
            return this;
        }

        /// <summary>
        /// Sets the background color of the main content.
        /// </summary>
        /// <param name="backgroundColor">The background color to set for the main content.</param>
        /// <returns>The current instance of <see cref="StateBuilder"/>.</returns>
        public StateBuilder SetMainBackgroundColor(Color backgroundColor)
        {
            return SetMainBackground(backgroundColor.ToColorString());
        }

        /// <summary>
        /// Sets the background of the main content to a linear gradient.
        /// </summary>
        /// <param name="gradient">The gradient to set for the main background.</param>
        /// <param name="gradientAngle">The angle of the gradient.</param>
        /// <returns>The current instance of <see cref="StateBuilder"/>.</returns>
        public StateBuilder SetMainBackgroundLinearGradient(Gradient gradient, float gradientAngle = 0.0f)
        {
            return SetMainBackground(gradient.ToLinearGradientString(gradientAngle));
        }

        /// <summary>
        /// Sets the background of the main content to a radial gradient.
        /// </summary>
        /// <param name="gradient">The gradient to set for the main background.</param>
        /// <param name="positioning">The positioning of the radial gradient.</param>
        /// <returns>The current instance of <see cref="StateBuilder"/>.</returns>
        public StateBuilder SetMainBackgroundRadialGradient(Gradient gradient, string positioning = "circle at center")
        {
            return SetMainBackground(gradient.ToRadialGradientString(positioning));
        }

        /// <summary>
        /// Sets the background of the main content to an image.
        /// </summary>
        /// <param name="url">The URL of the image.</param>
        /// <param name="scalingAndPositioning">The scaling and positioning of the image.</param>
        /// <returns>The current instance of <see cref="StateBuilder"/>.</returns>
        public StateBuilder SetMainBackgroundImage(string url, string scalingAndPositioning = "no-repeat center / cover")
        {
            return SetMainBackground(url.ToImageString(scalingAndPositioning));
        }

        /// <summary>
        /// Sets the minimum width of the main content.
        /// </summary>
        /// <param name="width">The minimum width to set for the main content.</param>
        /// <returns>The current instance of <see cref="StateBuilder"/>.</returns>
        public StateBuilder SetMainMinimumWidth(string width = "300px")
        {
            State.SetMainParameter("minWidth", width);
            return this;
        }

        /// <summary>
        /// Sets the minimum width of the main content.
        /// </summary>
        /// <param name="width">The minimum width to set for the main content.</param>
        /// <param name="dimensionUnit">The unit of the width dimension.</param>
        /// <returns>The current instance of <see cref="StateBuilder"/>.</returns>
        public StateBuilder SetMainMinimumWidth(float width = 300, string dimensionUnit = "px")
        {
            return SetMainMinimumWidth($"{width}{dimensionUnit}");
        }

        /// <summary>
        /// Sets the maximum width of the main content.
        /// </summary>
        /// <param name="width">The maximum width to set for the main content.</param>
        /// <returns>The current instance of <see cref="StateBuilder"/>.</returns>
        public StateBuilder SetMainMaximumWidth(string width = "350px")
        {
            State.SetMainParameter("maxWidth", width);
            return this;
        }

        /// <summary>
        /// Sets the maximum width of the main content.
        /// </summary>
        /// <param name="width">The maximum width to set for the main content.</param>
        /// <param name="dimensionUnit">The unit of the width dimension.</param>
        /// <returns>The current instance of <see cref="StateBuilder"/>.</returns>
        public StateBuilder SetMainMaximumWidth(float width = 350, string dimensionUnit = "px")
        {
            return SetMainMaximumWidth($"{width}{dimensionUnit}");
        }

        /// <summary>
        /// Adds a text component to the state.
        /// </summary>
        /// <param name="preset">The preset configuration for the text component.</param>
        /// <param name="text">The text to display in the component.</param>
        /// <param name="name">The name of the component.</param>
        /// <param name="key">The key for the component.</param>
        /// <param name="styleOverrides">Optional style overrides for the component.</param>
        /// <param name="componentAction">Optional action to perform on the component.</returns>
        /// <returns>The current instance of <see cref="StateBuilder"/>.</returns>
        public StateBuilder AddTextComponent(Preset preset, string text, string name = null, string key = null, ParameterList styleOverrides = null, Action<UIComponent> componentAction = null)
        {
            Debug.Assert(preset.Type == Preset.PresetType.Text);

            UIComponent newComponent = new UIComponent(name, preset)
            {
                ["text"] = new StringParameter(text)
            };

            return AddComponent(newComponent, key, styleOverrides, componentAction);
        }

        /// <summary>
        /// Adds a text input component to the state.
        /// </summary>
        /// <param name="preset">The preset configuration for the text input component.</param>
        /// <param name="eventName">The event name associated with the text input.</param>
        /// <param name="name">The name of the component.</param>
        /// <param name="key">The key for the component.</param>
        /// <param name="styleOverrides">Optional style overrides for the component.</param>
        /// <param name="componentAction">Optional action to perform on the component.</returns>
        /// <returns>The current instance of <see cref="StateBuilder"/>.</returns>
        public StateBuilder AddTextInputComponent(Preset preset, string eventName, string name = null, string key = null, ParameterList styleOverrides = null, Action<UIComponent> componentAction = null)
        {
            Debug.Assert(preset.Type == Preset.PresetType.TextInput);

            UIComponent newComponent = new UIComponent(name, preset)
            {
                ["event"] = new StringParameter(eventName)
            };

            return AddComponent(newComponent, key, styleOverrides, componentAction);
        }

        /// <summary>
        /// Adds a buzzer component to the state.
        /// </summary>
        /// <param name="preset">The preset configuration for the buzzer component.</param>
        /// <param name="eventName">The event name associated with the buzzer.</param>
        /// <param name="label">The label for the buzzer.</param>
        /// <param name="name">The name of the component.</param>
        /// <param name="key">The key for the component.</param>
        /// <param name="styleOverrides">Optional style overrides for the component.</param>
        /// <param name="componentAction">Optional action to perform on the component.</returns>
        /// <returns>The current instance of <see cref="StateBuilder"/>.</returns>
        public StateBuilder AddBuzzerComponent(Preset preset, string eventName, string label, string name = null, string key = null, ParameterList styleOverrides = null, Action<UIComponent> componentAction = null)
        {
            Debug.Assert(preset.Type == Preset.PresetType.Buzzer);

            UIComponent newComponent = new UIComponent(name, preset)
            {
                ["event"] = new StringParameter(eventName),
                ["label"] = new StringParameter(label)
            };

            return AddComponent(newComponent, key, styleOverrides, componentAction);
        }

        /// <summary>
        /// Adds a button component to the state.
        /// </summary>
        /// <param name="preset">The preset configuration for the button component.</param>
        /// <param name="eventName">The event name associated with the button.</param>
        /// <param name="label">The label for the button.</param>
        /// <param name="value">The value associated with the button.</param>
        /// <param name="name">The name of the component.</param>
        /// <param name="key">The key for the component.</param>
        /// <param name="styleOverrides">Optional style overrides for the component.</param>
        /// <param name="componentAction">Optional action to perform on the component.</returns>
        /// <returns>The current instance of <see cref="StateBuilder"/>.</returns>
        public StateBuilder AddButtonComponent(Preset preset, string eventName, string label, string value, string name = null, string key = null, ParameterList styleOverrides = null, Action<UIComponent> componentAction = null)
        {
            Debug.Assert(preset.Type == Preset.PresetType.Button);

            UIComponent newComponent = new UIComponent(name, preset)
            {
                ["event"] = new StringParameter(eventName),
                ["label"] = new StringParameter(label),
                ["value"] = new StringParameter(value)
            };

            return AddComponent(newComponent, key, styleOverrides, componentAction);
        }

        /// <summary>
        /// Adds a choices component to the state.
        /// </summary>
        /// <param name="preset">The preset configuration for the choices component.</param>
        /// <param name="eventName">The event name associated with the choices component.</param>
        /// <param name="name">The name of the component.</param>
        /// <param name="key">The key for the component.</param>
        /// <param name="multiSelect">Indicates if multiple selections are allowed.</param>
        /// <param name="submitLabel">The label for the submit button.</param>
        /// <param name="styleOverrides">Optional style overrides for the component.</param>
        /// <param name="hoverStyleOverrides">Optional hover style overrides for the component.</param>
        /// <param name="submitStyleOverrides">Optional submit style overrides for the component.</param>
        /// <param name="componentAction">Optional action to perform on the component.</param>
        /// <returns>The current instance of <see cref="StateBuilder"/>.</returns>
        public StateBuilder AddChoicesComponent(Preset preset, string eventName, string name = null, string key = null, bool multiSelect = false, string submitLabel = null, ParameterList styleOverrides = null, ParameterList hoverStyleOverrides = null, ParameterList submitStyleOverrides = null, Action<UIComponent> componentAction = null)
        {
            Debug.Assert(preset.Type == Preset.PresetType.Choices);

            UIComponent newComponent = new UIComponent(name, preset)
            {
                ["event"] = new StringParameter(eventName),
                ["multiSelect"] = new BoolParameter(multiSelect)
            };

            return AddChoicesComponent(newComponent, key, multiSelect, submitLabel, styleOverrides, hoverStyleOverrides, submitStyleOverrides, componentAction);
        }

        /// <summary>
        /// Adds a choices component to the state.
        /// </summary>
        /// <param name="preset">The preset configuration for the choices component.</param>
        /// <param name="eventName">The event name associated with the choices component.</param>
        /// <param name="choices">The array of choices for the component.</param>
        /// <param name="name">The name of the component.</param>
        /// <param name="key">The key for the component.</param>
        /// <param name="multiSelect">Indicates if multiple selections are allowed.</param>
        /// <param name="submitLabel">The label for the submit button.</param>
        /// <param name="styleOverrides">Optional style overrides for the component.</param>
        /// <param name="hoverStyleOverrides">Optional hover style overrides for the component.</param>
        /// <param name="submitStyleOverrides">Optional submit style overrides for the component.</param>
        /// <param name="componentAction">Optional action to perform on the component.</param>
        /// <returns>The current instance of <see cref="StateBuilder"/>.</returns>
        public StateBuilder AddChoicesComponent(Preset preset, string eventName, string[] choices, string name = null, string key = null, bool multiSelect = false, string submitLabel = null, ParameterList styleOverrides = null, ParameterList hoverStyleOverrides = null, ParameterList submitStyleOverrides = null, Action<UIComponent> componentAction = null)
        {
            Debug.Assert(preset.Type == Preset.PresetType.Choices);

            UIComponent newComponent = new UIComponent(name, preset)
            {
                ["choices"] = new ChoicesParameter(choices.Select(x => new ChoicesParameter.Choice() { Label = x, Value = x }).ToList()),
                ["event"] = new StringParameter(eventName),
                ["multiSelect"] = new BoolParameter(multiSelect)
            };

            return AddChoicesComponent(newComponent, key, multiSelect, submitLabel, styleOverrides, hoverStyleOverrides, submitStyleOverrides, componentAction);
        }

        /// <summary>
        /// Adds a choices component to the state.
        /// </summary>
        /// <param name="preset">The preset configuration for the choices component.</param>
        /// <param name="eventName">The event name associated with the choices component.</param>
        /// <param name="choices">The array of choices for the component, each containing a label and value.</param>
        /// <param name="name">The name of the component.</param>
        /// <param name="key">The key for the component.</param>
        /// <param name="multiSelect">Indicates if multiple selections are allowed.</param>
        /// <param name="submitLabel">The label for the submit button.</param>
        /// <param name="styleOverrides">Optional style overrides for the component.</param>
        /// <param name="hoverStyleOverrides">Optional hover style overrides for the component.</param>
        /// <param name="submitStyleOverrides">Optional submit style overrides for the component.</param>
        /// <param name="componentAction">Optional action to perform on the component.</param>
        /// <returns>The current instance of <see cref="StateBuilder"/>.</returns>
        public StateBuilder AddChoicesComponent(Preset preset, string eventName, (string label, string value)[] choices, string name = null, string key = null, bool multiSelect = false, string submitLabel = null, ParameterList styleOverrides = null, ParameterList hoverStyleOverrides = null, ParameterList submitStyleOverrides = null, Action<UIComponent> componentAction = null)
        {
            Debug.Assert(preset.Type == Preset.PresetType.Choices);

            UIComponent newComponent = new UIComponent(name, preset)
            {
                ["choices"] = new ChoicesParameter(choices.Select(x => new ChoicesParameter.Choice() { Label = x.label, Value = x.value }).ToList()),
                ["event"] = new StringParameter(eventName)
            };

            return AddChoicesComponent(newComponent, key, multiSelect, submitLabel, styleOverrides, hoverStyleOverrides, submitStyleOverrides, componentAction);
        }

        /// <summary>
        /// Adds a choices component to the state.
        /// </summary>
        /// <param name="preset">The preset configuration for the choices component.</param>
        /// <param name="eventName">The event name associated with the choices component.</param>
        /// <param name="choiceLabels">The array of choice labels for the component.</param>
        /// <param name="choiceValues">The array of choice values for the component.</param>
        /// <param name="name">The name of the component.</param>
        /// <param name="key">The key for the component.</param>
        /// <param name="multiSelect">Indicates if multiple selections are allowed.</param>
        /// <param name="submitLabel">The label for the submit button.</param>
        /// <param name="styleOverrides">Optional style overrides for the component.</param>
        /// <param name="hoverStyleOverrides">Optional hover style overrides for the component.</param>
        /// <param name="submitStyleOverrides">Optional submit style overrides for the component.</param>
        /// <param name="componentAction">Optional action to perform on the component.</param>
        /// <returns>The current instance of <see cref="StateBuilder"/>.</returns>
        public StateBuilder AddChoicesComponent(Preset preset, string eventName, string[] choiceLabels, string[] choiceValues, string name = null, string key = null, bool multiSelect = false, string submitLabel = null, ParameterList styleOverrides = null, ParameterList hoverStyleOverrides = null, ParameterList submitStyleOverrides = null, Action<UIComponent> componentAction = null)
        {
            return AddChoicesComponent(preset, eventName, choiceLabels.Zip(choiceValues, (label, value) => (label, value)).ToArray(), name, key, multiSelect, submitLabel, styleOverrides, hoverStyleOverrides, submitStyleOverrides, componentAction);
        }

        /// <summary>
        /// Adds a choices component to the state.
        /// </summary>
        /// <param name="preset">The preset configuration for the choices component.</param>
        /// <param name="eventName">The event name associated with the choices component.</param>
        /// <param name="choices">The array of choices for the component, each containing a label, value, and keys.</param>
        /// <param name="name">The name of the component.</param>
        /// <param name="key">The key for the component.</param>
        /// <param name="multiSelect">Indicates if multiple selections are allowed.</param>
        /// <param name="submitLabel">The label for the submit button.</param>
        /// <param name="styleOverrides">Optional style overrides for the component.</param>
        /// <param name="hoverStyleOverrides">Optional hover style overrides for the component.</param>
        /// <param name="submitStyleOverrides">Optional submit style overrides for the component.</param>
        /// <param name="componentAction">Optional action to perform on the component.</param>
        /// <returns>The current instance of <see cref="StateBuilder"/>.</returns>
        public StateBuilder AddChoicesComponent(Preset preset, string eventName, (string label, string value, string[] keys)[] choices, string name = null, string key = null, bool multiSelect = false, string submitLabel = null, ParameterList styleOverrides = null, ParameterList hoverStyleOverrides = null, ParameterList submitStyleOverrides = null, Action<UIComponent> componentAction = null)
        {
            Debug.Assert(preset.Type == Preset.PresetType.Choices);

            UIComponent newComponent = new UIComponent(name, preset)
            {
                ["choices"] = new ChoicesParameter(choices.Select(x => new ChoicesParameter.Choice() { Label = x.label, Value = x.value, Keys = x.keys }).ToList()),
                ["event"] = new StringParameter(eventName)
            };

            return AddChoicesComponent(newComponent, key, multiSelect, submitLabel, styleOverrides, hoverStyleOverrides, submitStyleOverrides, componentAction);
        }

        /// <summary>
        /// Adds a choices component to the state.
        /// </summary>
        /// <param name="preset">The preset configuration for the choices component.</param>
        /// <param name="eventName">The event name associated with the choices component.</param>
        /// <param name="choiceLabels">The array of choice labels for the component.</param>
        /// <param name="choiceValues">The array of choice values for the component.</param>
        /// <param name="choiceKeys">The array of choice keys for the component.</param>
        /// <param name="name">The name of the component.</param>
        /// <param name="key">The key for the component.</param>
        /// <param name="multiSelect">Indicates if multiple selections are allowed.</param>
        /// <param name="submitLabel">The label for the submit button.</param>
        /// <param name="styleOverrides">Optional style overrides for the component.</param>
        /// <param name="hoverStyleOverrides">Optional hover style overrides for the component.</param>
        /// <param name="submitStyleOverrides">Optional submit style overrides for the component.</param>
        /// <param name="componentAction">Optional action to perform on the component.</param>
        /// <returns>The current instance of <see cref="StateBuilder"/>.</returns>
        public StateBuilder AddChoicesComponent(Preset preset, string eventName, string[] choiceLabels, string[] choiceValues, string[][] choiceKeys, string name = null, string key = null, bool multiSelect = false, string submitLabel = null, ParameterList styleOverrides = null, ParameterList hoverStyleOverrides = null, ParameterList submitStyleOverrides = null, Action<UIComponent> componentAction = null)
        {
            return AddChoicesComponent(preset, eventName, choiceLabels.Zip(choiceValues, (label, value) => (label, value)).Zip(choiceKeys, (labelValue, keys) => (labelValue.label, labelValue.value, keys)).ToArray(), name, key, multiSelect, submitLabel, styleOverrides, hoverStyleOverrides, submitStyleOverrides, componentAction);
        }

        /// <summary>
        /// Adds a sort component to the state.
        /// </summary>
        /// <param name="preset">The preset configuration for the sort component.</param>
        /// <param name="eventName">The event name associated with the sort component.</param>
        /// <param name="choices">The array of sort options for the component.</param>
        /// <param name="name">The name of the component.</param>
        /// <param name="key">The key for the component.</param>
        /// <param name="submitLabel">The label for the submit button.</param>
        /// <param name="styleOverrides">Optional style overrides for the component.</param>
        /// <param name="hoverStyleOverrides">Optional hover style overrides for the component.</param>
        /// <param name="submitStyleOverrides">Optional submit style overrides for the component.</param>
        /// <param name="componentAction">Optional action to perform on the component.</param>
        /// <returns>The current instance of <see cref="StateBuilder"/>.</returns>
        public StateBuilder AddSortComponent(Preset preset, string eventName, string[] choices, string name = null, string key = null, string submitLabel = null, ParameterList styleOverrides = null, ParameterList hoverStyleOverrides = null, ParameterList submitStyleOverrides = null, Action<UIComponent> componentAction = null)
        {
            Debug.Assert(preset.Type == Preset.PresetType.Sort);

            UIComponent newComponent = new UIComponent(name, preset)
            {
                ["choices"] = new ChoicesParameter(choices.Select(x => new ChoicesParameter.Choice() { Label = x, Value = x }).ToList()),
                ["event"] = new StringParameter(eventName)
            };

            return AddSortComponent(newComponent, key, submitLabel, styleOverrides, hoverStyleOverrides, submitStyleOverrides, componentAction);
        }

        /// <summary>
        /// Adds a sort component to the state.
        /// </summary>
        /// <param name="preset">The preset configuration for the sort component.</param>
        /// <param name="eventName">The event name associated with the sort component.</param>
        /// <param name="choices">The array of sort options for the component, each containing a label and value.</param>
        /// <param name="name">The name of the component.</param>
        /// <param name="key">The key for the component.</param>
        /// <param name="submitLabel">The label for the submit button.</param>
        /// <param name="styleOverrides">Optional style overrides for the component.</param>
        /// <param name="hoverStyleOverrides">Optional hover style overrides for the component.</param>
        /// <param name="submitStyleOverrides">Optional submit style overrides for the component.</param>
        /// <param name="componentAction">Optional action to perform on the component.</param>
        /// <returns>The current instance of <see cref="StateBuilder"/>.</returns>
        public StateBuilder AddSortComponent(Preset preset, string eventName, (string label, string value)[] choices, string name = null, string key = null, string submitLabel = null, ParameterList styleOverrides = null, ParameterList hoverStyleOverrides = null, ParameterList submitStyleOverrides = null, Action<UIComponent> componentAction = null)
        {
            Debug.Assert(preset.Type == Preset.PresetType.Sort);

            UIComponent newComponent = new UIComponent(name, preset)
            {
                ["choices"] = new ChoicesParameter(choices.Select(x => new ChoicesParameter.Choice() { Label = x.label, Value = x.value }).ToList()),
                ["event"] = new StringParameter(eventName)
            };

            return AddSortComponent(newComponent, key, submitLabel, styleOverrides, hoverStyleOverrides, submitStyleOverrides, componentAction);
        }

        /// <summary>
        /// Adds a sort component to the state.
        /// </summary>
        /// <param name="preset">The preset configuration for the sort component.</param>
        /// <param name="eventName">The event name associated with the sort component.</param>
        /// <param name="choiceLabels">The array of sort option labels for the component.</param>
        /// <param name="choiceValues">The array of sort option values for the component.</param>
        /// <param name="name">The name of the component.</param>
        /// <param name="key">The key for the component.</param>
        /// <param name="submitLabel">The label for the submit button.</param>
        /// <param name="styleOverrides">Optional style overrides for the component.</param>
        /// <param name="hoverStyleOverrides">Optional hover style overrides for the component.</param>
        /// <param name="submitStyleOverrides">Optional submit style overrides for the component.</param>
        /// <param name="componentAction">Optional action to perform on the component.</param>
        /// <returns>The current instance of <see cref="StateBuilder"/>.</returns>
        public StateBuilder AddSortComponent(Preset preset, string eventName, string[] choiceLabels, string[] choiceValues, string name = null, string key = null, string submitLabel = null, ParameterList styleOverrides = null, ParameterList hoverStyleOverrides = null, ParameterList submitStyleOverrides = null, Action<UIComponent> componentAction = null)
        {
            return AddSortComponent(preset, eventName, choiceLabels.Zip(choiceValues, (label, value) => (label, value)).ToArray(), name, key, submitLabel, styleOverrides, hoverStyleOverrides, submitStyleOverrides, componentAction);
        }

        /// <summary>
        /// Adds a range component to the state.
        /// </summary>
        /// <param name="preset">The preset configuration for the range component.</param>
        /// <param name="min">The minimum value for the range.</param>
        /// <param name="max">The maximum value for the range.</param>
        /// <param name="step">The step value for the range.</param>
        /// <param name="name">The name of the component.</param>
        /// <param name="key">The key for the component.</param>
        /// <param name="styleOverrides">Optional style overrides for the component.</param>
        /// <param name="componentAction">Optional action to perform on the component.</param>
        /// <returns>The current instance of <see cref="StateBuilder"/>.</returns>
        public StateBuilder AddRangeComponent(Preset preset, int min, int max, int step, string name = null, string key = null, ParameterList styleOverrides = null, Action<UIComponent> componentAction = null)
        {
            Debug.Assert(preset.Type == Preset.PresetType.Range);

            UIComponent newComponent = new UIComponent(name, preset)
            {
                ["min"] = new IntParameter(min),
                ["max"] = new IntParameter(max),
                ["step"] = new IntParameter(step)
            };

            return AddComponent(newComponent, key, styleOverrides, componentAction);
        }

        /// <summary>
        /// Adds a range component to the state.
        /// </summary>
        /// <param name="preset">The preset configuration for the range component.</param>
        /// <param name="eventName">The event name associated with the range component.</param>
        /// <param name="min">The minimum value for the range.</param>
        /// <param name="max">The maximum value for the range.</param>
        /// <param name="step">The step value for the range.</param>
        /// <param name="name">The name of the component.</param>
        /// <param name="key">The key for the component.</param>
        /// <param name="styleOverrides">Optional style overrides for the component.</param>
        /// <param name="componentAction">Optional action to perform on the component.</param>
        /// <returns>The current instance of <see cref="StateBuilder"/>.</returns>
        public StateBuilder AddRangeComponent(Preset preset, string eventName, int min, int max, int step, string name = null, string key = null, ParameterList styleOverrides = null, Action<UIComponent> componentAction = null)
        {
            Debug.Assert(preset.Type == Preset.PresetType.Range);

            UIComponent newComponent = new UIComponent(name, preset)
            {
                ["min"] = new IntParameter(min),
                ["max"] = new IntParameter(max),
                ["step"] = new IntParameter(step),
                ["event"] = new StringParameter(eventName)
            };

            return AddComponent(newComponent, key, styleOverrides, componentAction);
        }

        /// <summary>
        /// Adds a text component to the state with an inline defined preset.
        /// </summary>
        /// <param name="text">The text content of the component.</param>
        /// <param name="name">The name of the component.</param>
        /// <param name="key">The key for the component.</param>
        /// <param name="styleOverrides">Optional style overrides for the component.</param>
        /// <param name="componentAction">Optional action to perform on the component.</param>
        /// <returns>The current instance of <see cref="StateBuilder"/>.</returns>
        public StateBuilder AddTextComponent(string text, string name, string key = null, ParameterList styleOverrides = null, Action<UIComponent> componentAction = null)
        {
            return AddTextComponent(Preset.Create(name, Preset.PresetType.Text), text, name, key, styleOverrides, componentAction);
        }

        /// <summary>
        /// Adds a text input component to the state with an inline defined preset.
        /// </summary>
        /// <param name="eventName">The event name associated with the text input component.</param>
        /// <param name="name">The name of the component.</param>
        /// <param name="key">The key for the component.</param>
        /// <param name="styleOverrides">Optional style overrides for the component.</param>
        /// <param name="componentAction">Optional action to perform on the component.</param>
        /// <returns>The current instance of <see cref="StateBuilder"/>.</returns>
        public StateBuilder AddTextInputComponent(string eventName, string name, string key = null, ParameterList styleOverrides = null, Action<UIComponent> componentAction = null)
        {
            return AddTextInputComponent(Preset.Create(name, Preset.PresetType.TextInput), eventName, name, key, styleOverrides, componentAction);
        }

        /// <summary>
        /// Adds a buzzer component to the state with an inline defined preset.
        /// </summary>
        /// <param name="eventName">The event name associated with the buzzer component.</param>
        /// <param name="label">The label for the buzzer component.</param>
        /// <param name="name">The name of the component.</param>
        /// <param name="key">The key for the component.</param>
        /// <param name="styleOverrides">Optional style overrides for the component.</param>
        /// <param name="componentAction">Optional action to perform on the component.</param>
        /// <returns>The current instance of <see cref="StateBuilder"/>.</returns>
        public StateBuilder AddBuzzerComponent(string eventName, string label, string name, string key = null, ParameterList styleOverrides = null, Action<UIComponent> componentAction = null)
        {
            return AddBuzzerComponent(Preset.Create(name, Preset.PresetType.Buzzer), eventName, label, name, key, styleOverrides, componentAction);
        }

        /// <summary>
        /// Adds a button component to the state with an inline defined preset.
        /// </summary>
        /// <param name="eventName">The event name associated with the button component.</param>
        /// <param name="label">The label for the button component.</param>
        /// <param name="value">The value for the button component.</param>
        /// <param name="name">The name of the component.</param>
        /// <param name="key">The key for the component.</param>
        /// <param name="styleOverrides">Optional style overrides for the component.</param>
        /// <param name="componentAction">Optional action to perform on the component.</param>
        /// <returns>The current instance of <see cref="StateBuilder"/>.</returns>
        public StateBuilder AddButtonComponent(string eventName, string label, string value, string name, string key = null, ParameterList styleOverrides = null, Action<UIComponent> componentAction = null)
        {
            return AddButtonComponent(Preset.Create(name, Preset.PresetType.Button), eventName, label, value, name, key, styleOverrides, componentAction);
        }

        /// <summary>
        /// Adds a choices component to the state with an inline defined preset.
        /// </summary>
        /// <param name="eventName">The event name associated with the choices component.</param>
        /// <param name="choices">The array of choices for the component.</param>
        /// <param name="name">The name of the component.</param>
        /// <param name="key">The key for the component.</param>
        /// <param name="multiSelect">Indicates if multiple selections are allowed.</param>
        /// <param name="submitLabel">The label for the submit button.</param>
        /// <param name="styleOverrides">Optional style overrides for the component.</param>
        /// <param name="hoverStyleOverrides">Optional hover style overrides for the component.</param>
        /// <param name="submitStyleOverrides">Optional submit style overrides for the component.</param>
        /// <param name="componentAction">Optional action to perform on the component.</param>
        /// <returns>The current instance of <see cref="StateBuilder"/>.</returns>
        public StateBuilder AddChoicesComponent(string eventName, string[] choices, string name, string key = null, bool multiSelect = false, string submitLabel = null, ParameterList styleOverrides = null, ParameterList hoverStyleOverrides = null, ParameterList submitStyleOverrides = null, Action<UIComponent> componentAction = null)
        {
            return AddChoicesComponent(Preset.Create(name, Preset.PresetType.Choices), eventName, choices, name, key, multiSelect, submitLabel, styleOverrides, hoverStyleOverrides, submitStyleOverrides, componentAction);
        }

        /// <summary>
        /// Adds a choices component to the state with an inline defined preset.
        /// </summary>
        /// <param name="eventName">The event name associated with the choices component.</param>
        /// <param name="choices">The array of choices for the component, each containing a label and value.</param>
        /// <param name="name">The name of the component.</param>
        /// <param name="key">The key for the component.</param>
        /// <param name="multiSelect">Indicates if multiple selections are allowed.</param>
        /// <param name="submitLabel">The label for the submit button.</param>
        /// <param name="styleOverrides">Optional style overrides for the component.</param>
        /// <param name="hoverStyleOverrides">Optional hover style overrides for the component.</param>
        /// <param name="submitStyleOverrides">Optional submit style overrides for the component.</param>
        /// <param name="componentAction">Optional action to perform on the component.</param>
        /// <returns>The current instance of <see cref="StateBuilder"/>.</returns>
        public StateBuilder AddChoicesComponent(string eventName, (string label, string value)[] choices, string name, string key = null, bool multiSelect = false, string submitLabel = null, ParameterList styleOverrides = null, ParameterList hoverStyleOverrides = null, ParameterList submitStyleOverrides = null, Action<UIComponent> componentAction = null)
        {
            return AddChoicesComponent(Preset.Create(name, Preset.PresetType.Choices), eventName, choices, name, key, multiSelect, submitLabel, styleOverrides, hoverStyleOverrides, submitStyleOverrides, componentAction);
        }

        /// <summary>
        /// Adds a choices component to the state with an inline defined preset.
        /// </summary>
        /// <param name="eventName">The event name associated with the choices component.</param>
        /// <param name="choices">The array of choices for the component, each containing a label, value, and keys.</param>
        /// <param name="name">The name of the component.</param>
        /// <param name="key">The key for the component.</param>
        /// <param name="multiSelect">Indicates if multiple selections are allowed.</param>
        /// <param name="submitLabel">The label for the submit button.</param>
        /// <param name="styleOverrides">Optional style overrides for the component.</param>
        /// <param name="hoverStyleOverrides">Optional hover style overrides for the component.</param>
        /// <param name="submitStyleOverrides">Optional submit style overrides for the component.</param>
        /// <param name="componentAction">Optional action to perform on the component.</param>
        /// <returns>The current instance of <see cref="StateBuilder"/>.</returns>
        public StateBuilder AddChoicesComponent(string eventName, (string label, string value, string[] keys)[] choices, string name, string key = null, bool multiSelect = false, string submitLabel = null, ParameterList styleOverrides = null, ParameterList hoverStyleOverrides = null, ParameterList submitStyleOverrides = null, Action<UIComponent> componentAction = null)
        {
            return AddChoicesComponent(Preset.Create(name, Preset.PresetType.Choices), eventName, choices, name, key, multiSelect, submitLabel, styleOverrides, hoverStyleOverrides, submitStyleOverrides, componentAction);
        }

        /// <summary>
        /// Adds a range component to the state with an inline defined preset.
        /// </summary>
        /// <param name="eventName">The event name associated with the range component.</param>
        /// <param name="min">The minimum value for the range.</param>
        /// <param name="max">The maximum value for the range.</param>
        /// <param name="step">The step value for the range.</param>
        /// <param name="name">The name of the component.</param>
        /// <param name="key">The key for the component.</param>
        /// <param name="styleOverrides">Optional style overrides for the component.</param>
        /// <param name="componentAction">Optional action to perform on the component.</param>
        /// <returns>The current instance of <see cref="StateBuilder"/>.</returns>
        public StateBuilder AddRangeComponent(string eventName, int min, int max, int step, string name, string key = null, ParameterList styleOverrides = null, Action<UIComponent> componentAction = null)
        {
            return AddRangeComponent(Preset.Create(name, Preset.PresetType.Range), eventName, min, max, step, name, key, styleOverrides, componentAction);
        }
        #endregion

        /// <summary>
        /// Implicitly converts a <see cref="StateBuilder"/> to a <see cref="State"/>.
        /// </summary>
        /// <param name="builder">The <see cref="StateBuilder"/> instance to convert.</param>
        /// <returns>The <see cref="State"/> instance.</returns>
        public static implicit operator State(StateBuilder builder) => builder.State;
        #endregion

        #region Private Methods
        /// <summary>
        /// Adds a partially-constructed component to the state.
        /// </summary>
        /// <param name="component">The component to add.</param>
        /// <param name="key">The key for the component.</param>
        /// <param name="styleOverrides">Style overrides for the component.</param>
        /// <param name="componentAction">An action to perform on the component.</param>
        /// <returns>The current <see cref="StateBuilder"/> instance.</returns>
        private StateBuilder AddComponent(UIComponent component, string key, ParameterList styleOverrides, Action<UIComponent> componentAction)
        {
            if (!string.IsNullOrEmpty(key))
            {
                component.Key = key;
            }

            if (styleOverrides != null)
            {
                component.StyleParameterList = new ParameterList(styleOverrides);
            }

            componentAction?.Invoke(component);
            State.Add(component);

            return this;
        }

        /// <summary>
        /// Adds a partially-constructed choices component to the state.
        /// </summary>
        /// <param name="component">The choices component to add.</param>
        /// <param name="key">The key for the component.</param>
        /// <param name="multiSelect">Indicates if multiple selections are allowed.</param>
        /// <param name="submitLabel">The label for the submit button.</param>
        /// <param name="styleOverrides">Style overrides for the component.</param>
        /// <param name="hoverStyleOverrides">Style overrides for the hover state.</param>
        /// <param name="submitStyleOverrides">Style overrides for the submit button.</param>
        /// <param name="componentAction">An action to perform on the component.</param>
        /// <returns>The current <see cref="StateBuilder"/> instance.</returns>
        private StateBuilder AddChoicesComponent(UIComponent component, string key, bool multiSelect, string submitLabel, ParameterList styleOverrides, ParameterList hoverStyleOverrides, ParameterList submitStyleOverrides, Action<UIComponent> componentAction)
        {
            if (multiSelect)
            {
                if (!string.IsNullOrEmpty(submitLabel))
                {
                    component["submit"] = new ParameterListParameter(ParameterListBuilder.Create().SetLabel("submitLabel").SetStyle(submitStyleOverrides));
                }
            }

            if (hoverStyleOverrides != null)
            {
                component["hover"] = new ParameterListParameter(hoverStyleOverrides);
            }

            return AddComponent(component, key, styleOverrides, componentAction);
        }


        /// <summary>
        /// Adds a partially-constructed sort component to the state.
        /// </summary>
        /// <param name="component">The sort component to add.</param>
        /// <param name="key">The key for the component.</param>
        /// <param name="submitLabel">The label for the submit button.</param>
        /// <param name="styleOverrides">Style overrides for the component.</param>
        /// <param name="hoverStyleOverrides">Style overrides for the hover state.</param>
        /// <param name="submitStyleOverrides">Style overrides for the submit button.</param>
        /// <param name="componentAction">An action to perform on the component.</param>
        /// <returns>The current <see cref="StateBuilder"/> instance.</returns>
        private StateBuilder AddSortComponent(UIComponent component, string key, string submitLabel, ParameterList styleOverrides, ParameterList hoverStyleOverrides, ParameterList submitStyleOverrides, Action<UIComponent> componentAction)
        {
            if (!string.IsNullOrEmpty(submitLabel))
            {
                component["submit"] = new ParameterListParameter(ParameterListBuilder.Create().SetLabel("submitLabel").SetStyle(submitStyleOverrides));
            }

            if (hoverStyleOverrides != null)
            {
                component["hover"] = new ParameterListParameter(hoverStyleOverrides);
            }

            return AddComponent(component, key, styleOverrides, componentAction);
        }
        #endregion
    }
}
