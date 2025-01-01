using System;
using System.Collections;
using System.Linq;
using Newtonsoft.Json;
using Hackbox.Parameters;
using UnityEngine;

namespace Hackbox.UI
{
    [Serializable]
    public class UIComponent: IUIElement
    {
        public UIComponent()
        {
        }

        public UIComponent(Preset preset)
        {
            Preset = preset;
        }

        public UIComponent(string name, Preset preset)
        {
            Name = name;
            Preset = preset;
        }

        public UIComponent(UIComponent from)
        {
            Name = from.Name;
            Key = from.Key;
            Preset = from.Preset;
            StyleParameterList = new ParameterList(from.StyleParameterList);
            ParameterList = new ParameterList(from.ParameterList);
        }

        [Tooltip("Name of the component. Only used in Unity.")]
        public string Name = "";
        [Tooltip("Key of the component. Use this to persist the state of a component between state changes.")]
        public string Key = "";
        [Tooltip("The preset this component is based on.")]
        public Preset Preset = null;

        [StyleParameterList]
        public ParameterList StyleParameterList = new ParameterList();

        [NormalParameterList]
        public ParameterList ParameterList = new ParameterList();

        public Parameter this[string parameterName]
        {
            get => ParameterList[parameterName] ?? StyleParameterList[parameterName];
            set
            {
                Add(parameterName, value);
            }
        }

        private void OnValidate()
        {
            //This is somewhat temporary to get style-based parameters into a specific parameter collection
            for (int parameterIndex = 0; parameterIndex < ParameterList.Parameters.Count; ++parameterIndex)
            {
                Parameter parameter = ParameterList[parameterIndex];

                if (!DefaultParameters.GetDefaultParameters(this).ContainsKey(parameter.Name) &&
                    DefaultParameters.GetDefaultStyleParameters(this).ContainsKey(parameter.Name))
                {
                    StyleParameterList.Parameters.Add(parameter);
                    ParameterList.Parameters.Remove(parameter);
                    parameterIndex--;
                }
            }
        }

        #region IEnumerable Interface & Collection Initialiser Implementation
        public IEnumerator GetEnumerator()
        {
            yield return StyleParameterList.GetEnumerator();
            yield return ParameterList.GetEnumerator();
        }

        public void Add(Parameter parameter)
        {
            Add(parameter.Name, parameter);
        }

        public void Add<T>(string parameterName, T value)
        {
            if (DefaultParameters.GetDefaultStyleParameters(this, null).ContainsKey(parameterName))
            {
                StyleParameterList.Add<T>(parameterName, value);
            }
            else
            {
                ParameterList.Add<T>(parameterName, value);
            }
        }
        #endregion

        #region Public Methods
        public static UIComponent Create(string name, Preset preset, string key)
        {
            return new UIComponent()
            {
                Name = name,
                Preset = preset,
                Key = key
            };
        }

        public static UIComponent Create(string name, Preset preset)
        {
            return new UIComponent()
            {
                Name = name,
                Preset = preset
            };
        }

        public static UIComponent Create(Preset preset, string key)
        {
            return new UIComponent()
            {
                Preset = preset,
                Key = key
            };
        }

        public static UIComponent Create(Preset preset)
        {
            return new UIComponent()
            {
                Preset = preset
            };
        }

        public Parameter<ValueT> GetGenericParameter<ValueT>(string parameterName)
        {
            return ParameterList.GetGenericParameter<ValueT>(parameterName);
        }

        public ParamT GetParameter<ParamT>(string parameterName) where ParamT : Parameter, new()
        {
            return ParameterList.GetParameter<ParamT>(parameterName);
        }

        public ValueT GetParameterValue<ValueT>(string parameterName)
        {
            return GetGenericParameter<ValueT>(parameterName).Value;
        }

        public void SetParameterValue<ValueT>(string parameterName, ValueT value)
        {
            ParameterList.SetParameterValue<ValueT>(parameterName, value);
        }

        public Parameter<ValueT> GetGenericStyleParameter<ValueT>(string parameterName)
        {
            return StyleParameterList.GetGenericParameter<ValueT>(parameterName);
        }

        public ParamT GetStyleParameter<ParamT>(string parameterName) where ParamT : Parameter, new()
        {
            return StyleParameterList.GetParameter<ParamT>(parameterName);
        }

        public ValueT GetStyleParameterValue<ValueT>(string parameterName)
        {
            return GetGenericStyleParameter<ValueT>(parameterName).Value;
        }

        public void SetStyleParameterValue<ValueT>(string parameterName, ValueT value)
        {
            StyleParameterList.SetParameterValue<ValueT>(parameterName, value);
        }
        #endregion

        #region Internal Methods
        internal void WriteJSON(JsonTextWriter json)
        {
            json.WriteStartObject();
            {
                json.WritePropertyName("type"); json.WriteValue(Preset.name);

                if (!string.IsNullOrEmpty(Key))
                {
                    json.WritePropertyName("key"); json.WriteValue(Key);
                }

                json.WritePropertyName("props"); WriteProps(json);
            }
            json.WriteEndObject();
        }

        internal void WriteProps(JsonTextWriter json)
        {
            json.WriteStartObject();
            {
                if (ParameterList != null && ParameterList.Parameters != null && ParameterList.Parameters.Count > 0)
                {
                    foreach (Parameter parameter in ParameterList.GetMergedParameters(Preset.ParameterList))
                    {
                        parameter.WriteProp(json);
                    }
                }

                if (StyleParameterList != null && StyleParameterList.Parameters != null && StyleParameterList.Parameters.Count > 0)
                {
                    json.WritePropertyName("style");
                    json.WriteStartObject();
                    {
                        foreach (Parameter parameter in ParameterList.GetMergedParameters(Preset.StyleParameterList))
                        {
                            parameter.WriteProp(json);
                        }
                    }
                    json.WriteEndObject();
                }
            }
            json.WriteEndObject();
        }
        #endregion
    }
}
