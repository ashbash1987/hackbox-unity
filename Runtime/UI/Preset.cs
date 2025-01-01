using System.Collections;
using UnityEngine;
using Newtonsoft.Json;
using Hackbox.Parameters;

namespace Hackbox.UI
{
    [CreateAssetMenu(menuName = "Hackbox/Preset")]
    public class Preset : ScriptableObject, IUIElement
    {
        public enum PresetType
        {
            Text,
            TextInput,
            Buzzer,
            Button,
            Choices,
            Range,
            Sort
        }

        public PresetType Type = PresetType.Text;
        
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

        #region Equatable Implementation
        public override int GetHashCode()
        {
            return GetInstanceID();
        }

        public override bool Equals(object other)
        {
            if (other is Preset otherPreset)
            {
                return GetInstanceID() == otherPreset.GetInstanceID();
            }

            return base.Equals(other);
        }
        #endregion

        #region Public Methods
        public static Preset Create(string name, PresetType type)
        {
            Preset preset = ScriptableObject.CreateInstance<Preset>();
            preset.name = name;
            preset.Type = type;
            return preset;
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
                json.WritePropertyName("type"); json.WriteValue(Type.ToString());
                json.WritePropertyName("props"); WriteProps(json);
            }
            json.WriteEndObject();
        }

        internal void WriteProps(JsonTextWriter json)
        {
            json.WriteStartObject();
            {
                foreach (Parameter parameter in ParameterList.Parameters)
                {
                    parameter.WriteProp(json);
                }

                json.WritePropertyName("style");
                json.WriteStartObject();
                {
                    foreach (Parameter parameter in StyleParameterList.Parameters)
                    {
                        parameter.WriteProp(json);
                    }
                }
                json.WriteEndObject();
            }
            json.WriteEndObject();
        }
        #endregion
    }
}
