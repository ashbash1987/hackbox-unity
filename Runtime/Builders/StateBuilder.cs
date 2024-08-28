using System;
using System.Linq;
using UnityEngine;
using Hackbox.Parameters;
using Hackbox.UI;

namespace Hackbox.Builders
{
    public sealed class StateBuilder
    {
        public StateBuilder(Theme theme)
        {
            State = new State(theme);
        }

        public State State
        {
            get;
            private set;
        }

        #region Public Methods
        public static StateBuilder Create(Theme theme)
        {
            return new StateBuilder(theme);
        }

        #region Header
        public StateBuilder SetHeaderColor(Color color)
        {
            State.HeaderColor = color;
            return this;
        }

        public StateBuilder SetHeaderBackground(string background)
        {
            State.HeaderBackground = background;
            return this;
        }

        public StateBuilder SetHeaderBackgroundColor(Color backgroundColor)
        {
            return SetHeaderBackground(backgroundColor.ToColorString());
        }

        public StateBuilder SetHeaderBackgroundLinearGradient(Gradient gradient, float gradientAngle = 0.0f)
        {
            return SetHeaderBackground(gradient.ToLinearGradientString(gradientAngle));
        }

        public StateBuilder SetHeaderBackgroundRadialGradient(Gradient gradient, string positioning = "circle at center")
        {
            return SetHeaderBackground(gradient.ToRadialGradientString(positioning));
        }

        public StateBuilder SetHeaderText(string text)
        {
            State.HeaderText = text;
            return this;
        }

        public StateBuilder SetHeaderMinimumHeight(string height = "50px")
        {
            State.SetHeaderParameter("minHeight", height);
            return this;
        }

        public StateBuilder SetHeaderMinimumHeight(float height = 50, string dimensionUnit = "px")
        {
            return SetHeaderMinimumHeight($"{height}{dimensionUnit}");
        }

        public StateBuilder SetHeaderMaximumHeight(string height = "50px")
        {
            State.SetHeaderParameter("maxHeight", height);
            return this;
        }

        public StateBuilder SetHeaderMaximumHeight(float height = 50, string dimensionUnit = "px")
        {
            return SetHeaderMaximumHeight($"{height}{dimensionUnit}");
        }
        #endregion

        #region Main
        public StateBuilder SetMainAlignment(string alignment = "start")
        {
            State.MainAlignment = alignment;
            return this;
        }

        public StateBuilder SetMainBackground(string background)
        {
            State.MainBackground = background;
            return this;
        }

        public StateBuilder SetMainBackgroundColor(Color backgroundColor)
        {
            return SetMainBackground(backgroundColor.ToColorString());
        }

        public StateBuilder SetMainBackgroundLinearGradient(Gradient gradient, float gradientAngle = 0.0f)
        {
            return SetMainBackground(gradient.ToLinearGradientString(gradientAngle));
        }

        public StateBuilder SetMainBackgroundRadialGradient(Gradient gradient, string positioning = "circle at center")
        {
            return SetMainBackground(gradient.ToRadialGradientString(positioning));
        }

        public StateBuilder SetMainMinimumWidth(string width = "300px")
        {
            State.SetMainParameter("minWidth", width);
            return this;
        }

        public StateBuilder SetMainMinimumWidth(float width = 300, string dimensionUnit = "px")
        {
            return SetMainMinimumWidth($"{width}{dimensionUnit}");
        }

        public StateBuilder SetMainMaximumWidth(string width = "350px")
        {
            State.SetMainParameter("maxWidth", width);
            return this;
        }

        public StateBuilder SetMainMaximumWidth(float width = 350, string dimensionUnit = "px")
        {
            return SetMainMaximumWidth($"{width}{dimensionUnit}");
        }
        #endregion

        #region Components
        public StateBuilder AddTextComponent(Preset preset, string text, string name = null, string key = null, ParameterList styleOverrides = null, Action<UIComponent> componentAction = null)
        {
            Debug.Assert(preset.Type == Preset.PresetType.Text);

            UIComponent newComponent = new UIComponent(name, preset)
            {
                ["text"] = new StringParameter(text)
            };

            return AddComponent(newComponent, key, styleOverrides, componentAction);
        }

        public StateBuilder AddTextInputComponent(Preset preset, string eventName, string name = null, string key = null, ParameterList styleOverrides = null, Action<UIComponent> componentAction = null)
        {
            Debug.Assert(preset.Type == Preset.PresetType.TextInput);

            UIComponent newComponent = new UIComponent(name, preset)
            {
                ["event"] = new StringParameter(eventName)
            };

            return AddComponent(newComponent, key, styleOverrides, componentAction);
        }

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

        public StateBuilder AddChoicesComponent(Preset preset, string[] choices, string name = null, string key = null, ParameterList styleOverrides = null, Action<UIComponent> componentAction = null)
        {
            Debug.Assert(preset.Type == Preset.PresetType.Choices);

            UIComponent newComponent = new UIComponent(name, preset)
            {
                ["choices"] = new ChoicesParameter(choices.Select(x => new ChoicesParameter.Choice() { Label = x, Value = x }).ToList())
            };

            return AddComponent(newComponent, key, styleOverrides, componentAction);
        }

        public StateBuilder AddChoicesComponent(Preset preset, string eventName, string[] choices, string name = null, string key = null, ParameterList styleOverrides = null, Action<UIComponent> componentAction = null)
        {
            Debug.Assert(preset.Type == Preset.PresetType.Choices);

            UIComponent newComponent = new UIComponent(name, preset)
            {
                ["choices"] = new ChoicesParameter(choices.Select(x => new ChoicesParameter.Choice() { Label = x, Value = x }).ToList()),
                ["event"] = new StringParameter(eventName)
            };

            return AddComponent(newComponent, key, styleOverrides, componentAction);
        }

        public StateBuilder AddChoicesComponent(Preset preset, (string label, string value)[] choices, string name = null, string key = null, ParameterList styleOverrides = null, Action<UIComponent> componentAction = null)
        {
            Debug.Assert(preset.Type == Preset.PresetType.Choices);

            UIComponent newComponent = new UIComponent(name, preset)
            {
                ["choices"] = new ChoicesParameter(choices.Select(x => new ChoicesParameter.Choice() { Label = x.label, Value = x.value }).ToList())
            };

            return AddComponent(newComponent, key, styleOverrides, componentAction);
        }

        public StateBuilder AddChoicesComponent(Preset preset, string eventName, (string label, string value)[] choices, string name = null, string key = null, ParameterList styleOverrides = null, Action<UIComponent> componentAction = null)
        {
            Debug.Assert(preset.Type == Preset.PresetType.Choices);

            UIComponent newComponent = new UIComponent(name, preset)
            {
                ["choices"] = new ChoicesParameter(choices.Select(x => new ChoicesParameter.Choice() { Label = x.label, Value = x.value }).ToList()),
                ["event"] = new StringParameter(eventName)
            };

            return AddComponent(newComponent, key, styleOverrides, componentAction);
        }

        public StateBuilder AddChoicesComponent(Preset preset, (string label, string value, string[] keys)[] choices, string name = null, string key = null, ParameterList styleOverrides = null, Action<UIComponent> componentAction = null)
        {
            Debug.Assert(preset.Type == Preset.PresetType.Choices);

            UIComponent newComponent = new UIComponent(name, preset)
            {
                ["choices"] = new ChoicesParameter(choices.Select(x => new ChoicesParameter.Choice() { Label = x.label, Value = x.value, Keys = x.keys }).ToList())
            };

            return AddComponent(newComponent, key, styleOverrides, componentAction);
        }

        public StateBuilder AddChoicesComponent(Preset preset, string eventName, (string label, string value, string[] keys)[] choices, string name = null, string key = null, ParameterList styleOverrides = null, Action<UIComponent> componentAction = null)
        {
            Debug.Assert(preset.Type == Preset.PresetType.Choices);

            UIComponent newComponent = new UIComponent(name, preset)
            {
                ["choices"] = new ChoicesParameter(choices.Select(x => new ChoicesParameter.Choice() { Label = x.label, Value = x.value, Keys = x.keys }).ToList()),
                ["event"] = new StringParameter(eventName)
            };

            return AddComponent(newComponent, key, styleOverrides, componentAction);
        }

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

        public StateBuilder AddTextComponent(string text, string name, string key = null, ParameterList styleOverrides = null, Action<UIComponent> componentAction = null)
        {
            return AddTextComponent(Preset.Create(name, Preset.PresetType.Text), text, name, key, styleOverrides, componentAction);
        }

        public StateBuilder AddTextInputComponent(string eventName, string name, string key = null, ParameterList styleOverrides = null, Action<UIComponent> componentAction = null)
        {
            return AddTextInputComponent(Preset.Create(name, Preset.PresetType.TextInput), eventName, name, key, styleOverrides, componentAction);
        }

        public StateBuilder AddBuzzerComponent(string eventName, string label, string name, string key = null, ParameterList styleOverrides = null, Action<UIComponent> componentAction = null)
        {
            return AddBuzzerComponent(Preset.Create(name, Preset.PresetType.Buzzer), eventName, label, name, key, styleOverrides, componentAction);
        }

        public StateBuilder AddButtonComponent(string eventName, string label, string value, string name, string key = null, ParameterList styleOverrides = null, Action<UIComponent> componentAction = null)
        {
            return AddButtonComponent(Preset.Create(name, Preset.PresetType.Button), eventName, label, value, name, key, styleOverrides, componentAction);
        }

        public StateBuilder AddChoicesComponent(string eventName, string[] choices, string name, string key = null, ParameterList styleOverrides = null, Action<UIComponent> componentAction = null)
        {
            return AddChoicesComponent(Preset.Create(name, Preset.PresetType.Choices), eventName, choices, name, key, styleOverrides, componentAction);
        }

        public StateBuilder AddChoicesComponent(string eventName, (string label, string value)[] choices, string name, string key = null, ParameterList styleOverrides = null, Action<UIComponent> componentAction = null)
        {
            return AddChoicesComponent(Preset.Create(name, Preset.PresetType.Choices), eventName, choices, name, key, styleOverrides, componentAction);
        }

        public StateBuilder AddRangeComponent(string eventName, int min, int max, int step, string name, string key = null, ParameterList styleOverrides = null, Action<UIComponent> componentAction = null)
        {
            return AddRangeComponent(Preset.Create(name, Preset.PresetType.Range), eventName, min, max, step, name, key, styleOverrides, componentAction);
        }
        #endregion

        public static implicit operator State(StateBuilder builder) => builder.State;
        #endregion

        #region Private Methods
        private StateBuilder AddComponent(UIComponent component, string key, ParameterList styleOverrides, Action<UIComponent> componentAction)
        {
            if (!string.IsNullOrEmpty(key))
            {
                component["key"] = new StringParameter(key);
            }

            if (styleOverrides != null)
            {
                component.StyleParameterList = new ParameterList(styleOverrides);
            }

            componentAction?.Invoke(component);
            State.Add(component);

            return this;
        }
        #endregion
    }
}
