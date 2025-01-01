using System;
using UnityEngine;
using Newtonsoft.Json;

namespace Hackbox.Parameters
{
    [Serializable]
    public class StringParameter : Parameter<string>
    {
        public StringParameter() :
            base()
        {
        }

        public StringParameter(string value) :
            base()
        {
            Value = value;
        }

        public StringParameter(StringParameter from) :
            base(from)
        {
            Value = from.Value;
        }

        public override string Value
        {
            get => _value;
            set => _value = value;
        }

        [SerializeField]
        public string _value;

        public override void WriteProp(JsonTextWriter json)
        {
            json.WritePropertyName(Name);
            json.WriteValue(Value);
        }
    }
}