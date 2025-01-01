using System;
using UnityEngine;
using Newtonsoft.Json;

namespace Hackbox.Parameters
{
    [Serializable]
    public class ParameterListParameter : Parameter<ParameterList>
    {        
        public ParameterListParameter() :
            base()
        {
        }

        public ParameterListParameter(ParameterList value) :
            base()
        {
            Value = value;
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
        [SerializeReference]
        public ParameterList _value = new ParameterList();

        public override void WriteProp(JsonTextWriter json)
        {
            json.WritePropertyName(Name);
            json.WriteStartObject();
            {
                foreach (Parameter parameter in Value.Parameters)
                {
                    parameter.WriteProp(json);
                }
            }
            json.WriteEndObject();
        }
    }
}
