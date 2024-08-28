using System;
using System.Linq;
using UnityEngine;
using Hackbox;
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

        public static StateBuilder Create(Theme theme)
        {
            return new StateBuilder(theme);
        }

        public State State
        {
            get;
            private set;
        }

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

        public StateBuilder SetHeaderText(string text)
        {
            State.HeaderText = text;
            return this;
        }

        public StateBuilder AddTextComponent(Preset preset, string text, string name = null, ParameterList styleOverrides = null, Action<UIComponent> componentAction = null)
        {
            Debug.Assert(preset.Type == Preset.PresetType.Text);

            UIComponent newComponent = new UIComponent(name, preset)
            {
                ["text"] = new StringParameter(text)
            };

            FinishAddComponent(newComponent, styleOverrides, componentAction);
            return this;
        }

        public StateBuilder AddTextInputComponent(Preset preset, string eventName, string name = null, ParameterList styleOverrides = null, Action<UIComponent> componentAction = null)
        {
            Debug.Assert(preset.Type == Preset.PresetType.TextInput);

            UIComponent newComponent = new UIComponent(name, preset)
            {
                ["event"] = new StringParameter(eventName)
            };

            FinishAddComponent(newComponent, styleOverrides, componentAction);
            return this;
        }

        public StateBuilder AddBuzzerComponent(Preset preset, string eventName, string label, string name = null, ParameterList styleOverrides = null, Action<UIComponent> componentAction = null)
        {
            Debug.Assert(preset.Type == Preset.PresetType.Buzzer);

            UIComponent newComponent = new UIComponent(name, preset)
            {
                ["event"] = new StringParameter(eventName),
                ["label"] = new StringParameter(label)
            };

            FinishAddComponent(newComponent, styleOverrides, componentAction);
            return this;
        }

        public StateBuilder AddButtonComponent(Preset preset, string eventName, string label, string value, string name = null, ParameterList styleOverrides = null, Action<UIComponent> componentAction = null)
        {
            Debug.Assert(preset.Type == Preset.PresetType.Button);

            UIComponent newComponent = new UIComponent(name, preset)
            {
                ["event"] = new StringParameter(eventName),
                ["label"] = new StringParameter(label),
                ["value"] = new StringParameter(value)
            };

            FinishAddComponent(newComponent, styleOverrides, componentAction);
            return this;
        }

        public StateBuilder AddChoicesComponent(Preset preset, string eventName, string[] choices, string name = null, ParameterList styleOverrides = null, Action<UIComponent> componentAction = null)
        {
            Debug.Assert(preset.Type == Preset.PresetType.Choices);

            UIComponent newComponent = new UIComponent(name, preset)
            {
                ["choices"] = new ChoicesParameter(choices.Select(x => new ChoicesParameter.Choice() { Label = x, Value = x }).ToList()),
                ["event"] = new StringParameter(eventName)
            };

            FinishAddComponent(newComponent, styleOverrides, componentAction);
            return this;
        }

        public StateBuilder AddChoicesComponent(Preset preset, string eventName, (string label, string value)[] choices, string name = null, ParameterList styleOverrides = null, Action<UIComponent> componentAction = null)
        {
            Debug.Assert(preset.Type == Preset.PresetType.Choices);

            UIComponent newComponent = new UIComponent(name, preset)
            {
                ["choices"] = new ChoicesParameter(choices.Select(x => new ChoicesParameter.Choice() { Label = x.label, Value = x.value }).ToList()),
                ["event"] = new StringParameter(eventName)
            };

            FinishAddComponent(newComponent, styleOverrides, componentAction);
            return this;
        }

        public StateBuilder AddRangeComponent(Preset preset, string eventName, int min, int max, int step, string name = null, ParameterList styleOverrides = null, Action<UIComponent> componentAction = null)
        {
            Debug.Assert(preset.Type == Preset.PresetType.Range);

            UIComponent newComponent = new UIComponent(name, preset)
            {
                ["min"] = new IntParameter(min),
                ["max"] = new IntParameter(max),
                ["step"] = new IntParameter(step),
                ["event"] = new StringParameter(eventName)
            };

            FinishAddComponent(newComponent, styleOverrides, componentAction);
            return this;
        }

        public StateBuilder AddTextComponent(string text, string name, ParameterList styleOverrides = null, Action<UIComponent> componentAction = null)
        {
            return AddTextComponent(Preset.Create(name, Preset.PresetType.Text), text, name, styleOverrides, componentAction);
        }

        public StateBuilder AddTextInputComponent(string eventName, string name, ParameterList styleOverrides = null, Action<UIComponent> componentAction = null)
        {
            return AddTextInputComponent(Preset.Create(name, Preset.PresetType.TextInput), eventName, name, styleOverrides, componentAction);
        }

        public StateBuilder AddBuzzerComponent(string eventName, string label, string name, ParameterList styleOverrides = null, Action<UIComponent> componentAction = null)
        {
            return AddBuzzerComponent(Preset.Create(name, Preset.PresetType.Buzzer), eventName, label, name, styleOverrides, componentAction);
        }

        public StateBuilder AddButtonComponent(string eventName, string label, string value, string name, ParameterList styleOverrides = null, Action<UIComponent> componentAction = null)
        {
            return AddButtonComponent(Preset.Create(name, Preset.PresetType.Button), eventName, label, value, name, styleOverrides, componentAction);
        }

        public StateBuilder AddChoicesComponent(string eventName, string[] choices, string name, ParameterList styleOverrides = null, Action<UIComponent> componentAction = null)
        {
            return AddChoicesComponent(Preset.Create(name, Preset.PresetType.Choices), eventName, choices, name, styleOverrides, componentAction);
        }

        public StateBuilder AddChoicesComponent(string eventName, (string label, string value)[] choices, string name, ParameterList styleOverrides = null, Action<UIComponent> componentAction = null)
        {
            return AddChoicesComponent(Preset.Create(name, Preset.PresetType.Choices), eventName, choices, name, styleOverrides, componentAction);
        }

        public StateBuilder AddRangeComponent(string eventName, int min, int max, int step, string name, ParameterList styleOverrides = null, Action<UIComponent> componentAction = null)
        {
            return AddRangeComponent(Preset.Create(name, Preset.PresetType.Range), eventName, min, max, step, name, styleOverrides, componentAction);
        }

        private void FinishAddComponent(UIComponent component, ParameterList styleOverrides, Action<UIComponent> componentAction)
        {
            if (styleOverrides != null)
            {
                component.StyleParameterList = new ParameterList(styleOverrides);
            }

            componentAction?.Invoke(component);
            State.Add(component);
        }

        public static implicit operator State(StateBuilder builder)
        {
            return builder.State;
        }
    }
}
