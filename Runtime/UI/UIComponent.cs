using System;
using Newtonsoft.Json.Linq;
using Hackbox.Parameters;

namespace Hackbox.UI
{
    [Serializable]
    public class UIComponent
    {
        public UIComponent()
        {
        }

        public UIComponent(UIComponent from)
        {
            Name = from.Name;
            Key = from.Key;
            Preset = from.Preset;
            ParameterList = new ParameterList(from.ParameterList);
        }

        public string Name = "";
        public string Key = "";
        public Preset Preset = null;

        [StyleParameterList]
        public ParameterList StyleParameterList = new ParameterList();

        [NormalParameterList]
        public ParameterList ParameterList = new ParameterList();

        public Parameter this[string parameterName] => ParameterList[parameterName] ?? StyleParameterList[parameterName];

        private JObject _obj = new JObject();

        private void OnValidate()
        {
            //This is somewhat temporary to get style-based parameters into a specific parameter collection
            for (int parameterIndex = 0; parameterIndex < ParameterList.Parameters.Count; ++parameterIndex)
            {
                Parameter parameter = ParameterList[parameterIndex];
                if (!CommonParameters.NormalParameterLookup.ContainsKey(parameter.Name) &&
                    CommonParameters.StyleParameterLookup.ContainsKey(parameter.Name))
                {
                    StyleParameterList.Parameters.Add(parameter);
                    ParameterList.Parameters.Remove(parameter);
                    parameterIndex--;
                }
            }
        }

        public Parameter<T> GetGenericParameter<T>(string parameterName)
        {
            return ParameterList.GetGenericParameter<T>(parameterName);
        }

        public T GetParameter<T>(string parameterName) where T: Parameter, new()
        {
            return ParameterList.GetParameter<T>(parameterName);
        }

        public void SetParameterValue<T>(string parameterName, T value)
        {
            ParameterList.SetParameterValue<T>(parameterName, value);
        }

        public JObject GenerateJSON(int version)
        {
            _obj["type"] = Preset.name;

            if (!string.IsNullOrEmpty(Key))
            {
                _obj["key"] = Key;
            }

            _obj["props"] = GenerateProps(version);

            return _obj;
        }

        private JObject GenerateProps(int version)
        {
            JObject props = new JObject();
            foreach (Parameter parameter in ParameterList.Parameters)
            {
                parameter.ApplyValueToJObject(props, version);
            }

            return props;
        }
    }
}
