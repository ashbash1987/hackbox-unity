using System;
using UnityEngine;
using Newtonsoft.Json.Linq;

namespace Hackbox.Parameters
{
    [Serializable]
    public class ParameterListParameter : Parameter<ParameterList>
    {        
        public ParameterListParameter() :
            base()
        {
        }

        public ParameterListParameter(ParameterListParameter from):
            base(from)
        {
            Value = new ParameterList(from.Value);
        }

        public override ParameterList Value
        {
            get => _value;
            set => _value = value;
        }

        [SerializeField]
        public ParameterList _value = new ParameterList();

        public override void ApplyValueToJObject(JObject parent)
        {
            JObject parametersObject = new JObject();
            foreach (Parameter parameter in Value.Parameters)
            {
                parameter.ApplyValueToJObject(parametersObject);
            }

            parent[Name] = parametersObject;
        }
    }
}