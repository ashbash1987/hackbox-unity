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

        public Parameter this[int parameterIndex] => ParameterList.Parameters[parameterIndex];
        public Parameter this[string parameterName] => ParameterList.Parameters.Find(x => x.Name == parameterName);

        private JObject _obj = new JObject();

        public T GetParameter<T>(string parameterName) where T : Parameter, new()
        {
            Parameter parameter = this[parameterName];
            if (parameter is T typedParameter)
            {
                return typedParameter;
            }

            if (parameter != null)
            {
                throw new Exception($"Trying to get parameter {parameterName} but using the wrong parameter type. Expected {parameter.GetType().Name}, asking for {typeof(T).Name}");
            }

            typedParameter = new T() { Name = parameterName };
            ParameterList.Parameters.Add(typedParameter);

            return typedParameter;
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
