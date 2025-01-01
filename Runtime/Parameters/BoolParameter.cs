using System;
using UnityEngine;
using Newtonsoft.Json;

namespace Hackbox.Parameters
{
    [Serializable]
    public class BoolParameter : Parameter<bool>
    {
        public BoolParameter() :
            base()
        {
        }

        public BoolParameter(bool value) :
            base()
        {
            Value = value;
        }

        public BoolParameter(BoolParameter from) :
            base(from)
        {
            Value = from.Value;
        }

        public override bool Value
        {
            get => _value;
            set => _value = value;
        }

        [SerializeField]
        public bool _value;

        public override void WriteProp(JsonTextWriter json)
        {
            json.WritePropertyName(Name);
            json.WriteValue(Value);
        }
    }
}