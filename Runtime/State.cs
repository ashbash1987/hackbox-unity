using System;
using System.Linq;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;
using Hackbox.UI;
using Hackbox.Parameters;

namespace Hackbox
{
    [Serializable]
    public class State
    {
        public State()
        {
        }

        public State(State from)
        {
            Theme = from.Theme;
            HeaderParameterList = new ParameterList(from.HeaderParameterList);
            MainParameterList = new ParameterList(from.MainParameterList);
            Components = new List<UIComponent>(from.Components.Select(x => new UIComponent(x)));
        }

        public Theme Theme = null;
        public ParameterList HeaderParameterList = new ParameterList();
        public ParameterList MainParameterList = new ParameterList();
        public List<UIComponent> Components = new List<UIComponent>();

        public UIComponent this[string componentName]
        {
            get => Components.Find(x => x.Name.Equals(componentName));
        }

        public UIComponent this[int componentIndex]
        {
            get => Components[componentIndex];
            set => Components[componentIndex] = value;
        }

        private JObject _obj = new JObject();

        public void SetHeaderParameter<S, T>(string parameterName, T value) where S : Parameter<T>, new()
        {
            SetParameter(HeaderParameterList.GetParameter<S>(parameterName), value);
        }

        public void SetHeaderText(string text)
        {
            SetHeaderParameter<StringParameter, string>("text", text);
        }

        public void SetComponentParameter<S, T>(int componentIndex, string parameterName, T value) where S: Parameter<T>, new()
        {
            SetParameter(this[componentIndex].GetParameter<S>(parameterName), value);
        }

        public void SetComponentParameter<S, T>(string componentName, string parameterName, T value) where S : Parameter<T>, new()
        {
            SetParameter(this[componentName].GetParameter<S>(parameterName), value);
        }

        public void SetComponentText(int componentIndex, string text)
        {
            SetComponentParameter<StringParameter, string>(componentIndex, "text", text);
        }

        public void SetComponentText(string componentName, string text)
        {
            SetComponentParameter<StringParameter, string>(componentName, "text", text);
        }

        public void SetComponentLabel(int componentIndex, string label)
        {
            SetComponentParameter<StringParameter, string>(componentIndex, "label", label);
        }

        public void SetComponentLabel(string componentName, string label)
        {
            SetComponentParameter<StringParameter, string>(componentName, "label", label);
        }

        public void SetComponentValue(int componentIndex, string value)
        {
            SetComponentParameter<StringParameter, string>(componentIndex, "value", value);
        }

        public void SetComponentValue(string componentName, string value)
        {
            SetComponentParameter<StringParameter, string>(componentName, "value", value);
        }

        public JObject GenerateJSON()
        {
            if (Theme == null)
            {
                return _obj;
            }

            _obj = JObject.FromObject(new
            {
                theme = Theme.GenerateJSON(),
                presets = GeneratePresets(),
                ui = new
                {
                    header = new {},
                    main = new
                    {
                        components = new JArray(Components.Select(x => x.GenerateJSON()))
                    }
                }                
            });

            JObject headerObj = (JObject)_obj["ui"]["header"];
            foreach (Parameter parameter in HeaderParameterList.Parameters)
            {
                parameter.ApplyValueToJObject(headerObj);
            }

            JObject mainObj = (JObject)_obj["ui"]["main"];
            foreach (Parameter parameter in MainParameterList.Parameters)
            {
                parameter.ApplyValueToJObject(mainObj);
            }

            return _obj;
        }

        private JObject GeneratePresets()
        {
            JObject presets = new JObject();
            foreach (Preset preset in Components.Select(x => x.Preset).Distinct())
            {
                presets[preset.name] = preset.GenerateJSON();
            }

            return presets;
        }

        private void SetParameter<T>(Parameter<T> parameter, T value)
        {
            parameter.Value = value;
        }
    }
}
