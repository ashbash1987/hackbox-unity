using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;
using Hackbox.UI;
using Hackbox.Parameters;

namespace Hackbox
{
    [Serializable]
    public class State : IEnumerable
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
            set
            {
                int existingIndex = Components.FindIndex(x => x.Name.Equals(componentName));
                value.Name = componentName;
                if (existingIndex >= 0)
                {
                    Components[existingIndex] = value;
                }
                else
                {
                    Components.Add(value);
                }
            }
        }

        public Parameter this[int componentIndex, string parameterName]
        {
            get => this[componentIndex][parameterName];
            set => this[componentIndex].Add(parameterName, value);
        }

        public Parameter this[string componentName, string parameterName]
        {
            get => this[componentName][parameterName];
            set => this[componentName].Add(parameterName, value);
        }

        private JObject _obj = new JObject();

        #region IEnumerable Interface & Collection Initialiser Implementation
        public IEnumerator GetEnumerator()
        {
            return Components.GetEnumerator();
        }

        public void Add(UIComponent component)
        {
            Components.Add(component);
        }

        public void Add(string componentName, UIComponent component)
        {
            this[componentName] = component;
        }
        #endregion

        #region Public Methods
        public Parameter<ValueT> GetGenericHeaderParameter<ValueT>(string parameterName)
        {
            return HeaderParameterList.GetGenericParameter<ValueT>(parameterName);
        }

        public ParamT GetHeaderParameter<ParamT>(string parameterName) where ParamT : Parameter, new()
        {
            return HeaderParameterList.GetParameter<ParamT>(parameterName);
        }

        public ValueT GetHeaderParameterValue<ValueT>(string parameterName)
        {
            return GetGenericHeaderParameter<ValueT>(parameterName).Value;
        }

        public void SetHeaderParameter<ValueT>(string parameterName, ValueT value)
        {
            HeaderParameterList.SetParameterValue<ValueT>(parameterName, value);
        }

        public string GetHeaderText()
        {
            return GetHeaderParameterValue<string>("text");
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

        public Parameter<ValueT> GetComponentGenericParameter<ValueT>(int componentIndex, string parameterName)
        {
            return this[componentIndex].GetGenericParameter<ValueT>(parameterName);
        }

        public Parameter<ValueT> GetComponentGenericParameter<ValueT>(string componentName, string parameterName)
        {
            return this[componentName].GetGenericParameter<ValueT>(parameterName);
        }

        public ParamT GetComponentParameter<ParamT>(int componentIndex, string parameterName) where ParamT: Parameter, new()
        {
            return this[componentIndex].GetParameter<ParamT>(parameterName);
        }

        public ParamT GetComponentParameter<ParamT>(string componentName, string parameterName) where ParamT : Parameter, new()
        {
            return this[componentName].GetParameter<ParamT>(parameterName);
        }

        public ValueT GetComponentParameterValue<ValueT>(int componentIndex, string parameterName)
        {
            return this[componentIndex].GetParameterValue<ValueT>(parameterName);
        }

        public ValueT GetComponentParameterValue<ValueT>(string componentName, string parameterName)
        {
            return this[componentName].GetParameterValue<ValueT>(parameterName);
        }

        public void SetComponentParameterValue<ValueT>(int componentIndex, string parameterName, ValueT value)
        {
            this[componentIndex].SetParameterValue<ValueT>(parameterName, value);
        }

        public void SetComponentParameterValue<ValueT>(string componentName, string parameterName, ValueT value)
        {
            this[componentName].SetParameterValue<ValueT>(parameterName, value);
        }

        public Parameter<ValueT> GetComponentGenericStyleParameter<ValueT>(int componentIndex, string parameterName)
        {
            return this[componentIndex].GetGenericStyleParameter<ValueT>(parameterName);
        }

        public Parameter<ValueT> GetComponentGenericStyleParameter<ValueT>(string componentName, string parameterName)
        {
            return this[componentName].GetGenericStyleParameter<ValueT>(parameterName);
        }

        public ParamT GetComponentStyleParameter<ParamT>(int componentIndex, string parameterName) where ParamT : Parameter, new()
        {
            return this[componentIndex].GetStyleParameter<ParamT>(parameterName);
        }

        public ParamT GetComponentStyleParameter<ParamT>(string componentName, string parameterName) where ParamT : Parameter, new()
        {
            return this[componentName].GetStyleParameter<ParamT>(parameterName);
        }

        public ValueT GetComponentStyleParameterValue<ValueT>(int componentIndex, string parameterName)
        {
            return this[componentIndex].GetStyleParameterValue<ValueT>(parameterName);
        }

        public ValueT GetComponentStyleParameterValue<ValueT>(string componentName, string parameterName)
        {
            return this[componentName].GetStyleParameterValue<ValueT>(parameterName);
        }

        public void SetComponentStyleParameterValue<ValueT>(int componentIndex, string parameterName, ValueT value)
        {
            this[componentIndex].SetStyleParameterValue<ValueT>(parameterName, value);
        }

        public void SetComponentStyleParameterValue<ValueT>(string componentName, string parameterName, ValueT value)
        {
            this[componentName].SetStyleParameterValue<ValueT>(parameterName, value);
        }

        public string GetComponentText(int componentIndex)
        {
            return GetComponentParameterValue<string>(componentIndex, "text");
        }

        public string GetComponentText(string componentName)
        {
            return GetComponentParameterValue<string>(componentName, "text");
        }

        public void SetComponentText(int componentIndex, string text)
        {
            SetComponentParameterValue(componentIndex, "text", text);
        }

        public void SetComponentText(string componentName, string text)
        {
            SetComponentParameterValue(componentName, "text", text);
        }

        public string GetComponentLabel(int componentIndex)
        {
            return GetComponentParameterValue<string>(componentIndex, "label");
        }

        public string GetComponentLabel(string componentName)
        {
            return GetComponentParameterValue<string>(componentName, "label");
        }

        public void SetComponentLabel(int componentIndex, string label)
        {
            SetComponentParameterValue(componentIndex, "label", label);
        }

        public void SetComponentLabel(string componentName, string label)
        {
            SetComponentParameterValue(componentName, "label", label);
        }

        public string GetComponentValue(int componentIndex)
        {
            return GetComponentParameterValue<string>(componentIndex, "value");
        }

        public string GetComponentValue(string componentName)
        {
            return GetComponentParameterValue<string>(componentName, "value");
        }

        public void SetComponentValue(int componentIndex, string value)
        {
            SetComponentParameterValue(componentIndex, "value", value);
        }

        public void SetComponentValue(string componentName, string value)
        {
            SetComponentParameterValue(componentName, "value", value);
        }
        #endregion

        public JObject GenerateJSON(int version)
        {
            if (Theme == null)
            {
                return _obj;
            }

            _obj = new JObject();
            _obj["version"] = version;
            _obj["theme"] = Theme.GenerateJSON(version);
            _obj["presets"] = GeneratePresets(version);

            JObject ui = new JObject();
            _obj["ui"] = ui;

            JObject headerObj = new JObject();
            _obj["ui"]["header"] = headerObj;

            JObject mainObj = new JObject();
            mainObj["components"] = new JArray(Components.Select(x => x.GenerateJSON(version)));
            _obj["ui"]["main"] = mainObj;

            foreach (Parameter parameter in HeaderParameterList.Parameters)
            {
                parameter.ApplyValueToJObject(headerObj, version);
            }

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
