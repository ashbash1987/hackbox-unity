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

        [HeaderParameterList]
        public ParameterList HeaderParameterList = new ParameterList();

        [MainParameterList]
        public ParameterList MainParameterList = new ParameterList();

        public List<UIComponent> Components = new List<UIComponent>();

        public UIComponent this[int componentIndex]
        {
            get => Components[componentIndex];
            set => Components[componentIndex] = value;
        }

        public UIComponent this[string componentName]
        {
            get => Components.Find(x => x.Name.Equals(componentName));
        }

        private JObject _obj = new JObject();

        public void SetHeaderParameter<T>(string parameterName, T value)
        {
            HeaderParameterList.SetParameterValue<T>(parameterName, value);
        }

        public void SetHeaderText(string text)
        {
            SetHeaderParameter<string>("text", text);
        }

        public UIComponent GetComponent(int componentIndex)
        {
            return this[componentIndex];
        }

        public UIComponent GetComponent(string componentName)
        {
            return this[componentName];
        }

        public Parameter<T> GetComponentGenericParameter<T>(int componentIndex, string parameterName)
        {
            return this[componentIndex].GetGenericParameter<T>(parameterName);
        }

        public Parameter<T> GetComponentGenericParameter<T>(string componentName, string parameterName)
        {
            return this[componentName].GetGenericParameter<T>(parameterName);
        }

        public T GetComponentParameter<T>(int componentIndex, string parameterName) where T: Parameter, new()
        {
            return this[componentIndex].GetParameter<T>(parameterName);
        }

        public T GetComponentParameter<T>(string componentName, string parameterName) where T : Parameter, new()
        {
            return this[componentName].GetParameter<T>(parameterName);
        }

        public void SetComponentParameterValue<T>(int componentIndex, string parameterName, T value)
        {
            this[componentIndex].SetParameterValue<T>(parameterName, value);
        }

        public void SetComponentParameterValue<T>(string componentName, string parameterName, T value)
        {
            this[componentName].SetParameterValue<T>(parameterName, value);
        }

        public void SetComponentText(int componentIndex, string text)
        {
            SetComponentParameterValue(componentIndex, "text", text);
        }

        public void SetComponentText(string componentName, string text)
        {
            SetComponentParameterValue(componentName, "text", text);
        }

        public void SetComponentLabel(int componentIndex, string label)
        {
            SetComponentParameterValue(componentIndex, "label", label);
        }

        public void SetComponentLabel(string componentName, string label)
        {
            SetComponentParameterValue(componentName, "label", label);
        }

        public void SetComponentValue(int componentIndex, string value)
        {
            SetComponentParameterValue(componentIndex, "value", value);
        }

        public void SetComponentValue(string componentName, string value)
        {
            SetComponentParameterValue(componentName, "value", value);
        }

        public JObject GenerateJSON(int version)
        {
            if (Theme == null)
            {
                return _obj;
            }

            _obj = JObject.FromObject(new
            {
                theme = Theme.GenerateJSON(version),
                presets = GeneratePresets(version),
                ui = new
                {
                    header = new {},
                    main = new
                    {
                        components = new JArray(Components.Select(x => x.GenerateJSON(version)))
                    }
                }                
            });

            JObject headerObj = (JObject)_obj["ui"]["header"];
            foreach (Parameter parameter in HeaderParameterList.Parameters)
            {
                parameter.ApplyValueToJObject(headerObj, version);
            }

            JObject mainObj = (JObject)_obj["ui"]["main"];
            foreach (Parameter parameter in MainParameterList.Parameters)
            {
                parameter.ApplyValueToJObject(mainObj, version);
            }

            return _obj;
        }

        private JObject GeneratePresets(int version)
        {
            JObject presets = new JObject();
            foreach (Preset preset in Components.Select(x => x.Preset).Distinct())
            {
                presets[preset.name] = preset.GenerateJSON(version);
            }

            return presets;
        }
    }
}
