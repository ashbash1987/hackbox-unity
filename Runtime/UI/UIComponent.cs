using System;
using System.Collections;
using Newtonsoft.Json.Linq;
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

        private JObject _obj = new JObject();

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
        internal JObject GenerateJSON(int version)
        {
            _obj["type"] = Preset.name;

            if (!string.IsNullOrEmpty(Key))
            {
                _obj["key"] = Key;
            }

            _obj["props"] = GenerateProps(version);

            return _obj;
        }

        internal JObject GenerateProps(int version)
        {
            //There's some strange lingering issue with the slicing of style properties, so let's do it here to make sure all is styled correctly
            JObject props = Preset?.GenerateProps(version) ?? new JObject();

            foreach (Parameter parameter in ParameterList.Parameters)
            {
                parameter.ApplyValueToJObject(props, version);
            }

            switch (version)
            {
                case 1:
                    foreach (Parameter parameter in StyleParameterList.Parameters)
                    {
                        parameter.ApplyValueToJObject(props, version);
                    }
                    break;

                default:
                    JObject style = null;
                    if (props.TryGetValue("style", out JToken styleToken))
                    {
                        style = styleToken as JObject;
                    }
                    if (style == null)
                    {
                        style = new JObject();
                        props["style"] = style;
                    }
                    
                    foreach (Parameter parameter in StyleParameterList.Parameters)
                    {
                        parameter.ApplyValueToJObject(style, version);
                    }                    
                    break;
            }

            return props;
        }
        #endregion
    }
}
