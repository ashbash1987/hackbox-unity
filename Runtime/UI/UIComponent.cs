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
            Preset = from.Preset;
            ParameterList = new ParameterList(from.ParameterList);
        }

        public string Name = "";
        public Preset Preset = null;
        public ParameterList ParameterList = new ParameterList();

        public Parameter this[int parameterIndex] => ParameterList[parameterIndex];
        public Parameter this[string parameterName] => ParameterList[parameterName];

        private JObject _obj = new JObject();

        public Parameter<T> GetParameter<T>(string parameterName)
        {
            return ParameterList.GetParameter<T>(parameterName);
        }

        public void SetParameterValue<T>(string parameterName, T value)
        {
            ParameterList.SetParameterValue<T>(parameterName, value);
        }

        public JObject GenerateJSON()
        {
            _obj["type"] = Preset.name;
            _obj["props"] = GenerateProps();

            return _obj;
        }

        private JObject GenerateProps()
        {
            JObject props = new JObject();
            foreach (Parameter parameter in ParameterList.Parameters)
            {
                parameter.ApplyValueToJObject(props);
            }

            return props;
        }
    }
}
