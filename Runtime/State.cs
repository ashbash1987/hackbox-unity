using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json.Linq;
using Hackbox.UI;
using Hackbox.Parameters;
using System.IO;
using Newtonsoft.Json;

namespace Hackbox
{
    [Serializable]
    public class State : IEnumerable
    {
        public State()
        {
        }

        public State(Theme theme)
        {
            Theme = theme;
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

        public string HeaderText
        {
            get => GetHeaderParameterValue<string>("text");
            set => SetHeaderParameter("text", value);
        }

        public Color HeaderColor
        {
            get => GetHeaderParameterValue<Color>("color");
            set => SetHeaderParameter("color", value);
        }

        public string HeaderBackground
        {
            get => GetHeaderParameterValue<string>("background");
            set => SetHeaderParameter("background", value);
        }

        public string MainAlignment
        {
            get => GetMainParameterValue<string>("align");
            set => SetMainParameter("align", value);
        }

        public string MainBackground
        {
            get => GetMainParameterValue<string>("background");
            set => SetMainParameter("background", value);
        }

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

        public bool Remove(UIComponent component)
        {
            return Components.Remove(component);
        }

        public bool Remove(string componentName)
        {
            return Components.Remove(this[componentName]);
        }

        public void RemoveAt(int componentIndex)
        {
            Components.RemoveAt(componentIndex);
        }
        #endregion

        #region Public Methods
        public static State Create(Theme theme)
        {
            return new State() { Theme = theme };
        }

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
            return HeaderText;
        }

        public void SetHeaderText(string text)
        {
            HeaderText = text;
        }

        public Parameter<ValueT> GetGenericMainParameter<ValueT>(string parameterName)
        {
            return MainParameterList.GetGenericParameter<ValueT>(parameterName);
        }

        public ParamT GetMainParameter<ParamT>(string parameterName) where ParamT : Parameter, new()
        {
            return MainParameterList.GetParameter<ParamT>(parameterName);
        }

        public ValueT GetMainParameterValue<ValueT>(string parameterName)
        {
            return GetGenericMainParameter<ValueT>(parameterName).Value;
        }

        public void SetMainParameter<ValueT>(string parameterName, ValueT value)
        {
            MainParameterList.SetParameterValue<ValueT>(parameterName, value);
        }

        public UIComponent GetComponent(int componentIndex)
        {
            return this[componentIndex];
        }

        public UIComponent GetComponent(string componentName)
        {
            return this[componentName];
        }

        public bool TryGetComponent(string componentName, out UIComponent component)
        {
            component = Components.Find(x => x.Name.Equals(componentName));
            return component != null;
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

        public string GenerateJSON()
        {
            StringWriter stringWriter = new StringWriter();
            JsonTextWriter json = new JsonTextWriter(stringWriter);
            WriteJSON(json);
            return stringWriter.ToString();
        }

        public void WriteJSON(JsonTextWriter json)
        {
            if (Theme == null)
            {
                return;
            }

            json.WriteStartObject();
            {
                json.WritePropertyName("theme"); Theme.WriteJSON(json);
                json.WritePropertyName("presets"); WritePresets(json);

                json.WritePropertyName("ui");
                json.WriteStartObject();
                {
                    json.WritePropertyName("header");
                    json.WriteStartObject();
                    {
                        foreach (Parameter parameter in HeaderParameterList.Parameters)
                        {
                            parameter.WriteProp(json);
                        }
                    }
                    json.WriteEndObject();

                    json.WritePropertyName("main");
                    json.WriteStartObject();
                    {
                        foreach (Parameter parameter in MainParameterList.Parameters)
                        {
                            parameter.WriteProp(json);
                        }

                        json.WritePropertyName("components");
                        json.WriteStartArray();
                        foreach (UIComponent component in Components)
                        {
                            component.WriteJSON(json);
                        }
                        json.WriteEndArray();
                    }
                    json.WriteEndObject();
                }
                json.WriteEndObject();
            }
            json.WriteEndObject();
        }
        #endregion

        #region Private Methods
        private void WritePresets(JsonTextWriter json)
        {
            json.WriteStartObject();
            {
                foreach (Preset preset in Components.Select(x => x.Preset).Distinct())
                {
                    json.WritePropertyName(preset.name); preset.WriteJSON(json);
                }
            }
            json.WriteEndObject();
        }
        #endregion
    }
}
