using System;
using UnityEngine;
using Newtonsoft.Json;

namespace Hackbox.Parameters
{
    [Serializable]
    public class IntParameter : Parameter<int>
    {
        public IntParameter() :
            base()
        {
        }

        public IntParameter(int value) :
            base()
        {
            Value = value;
        }

        public IntParameter(IntParameter from) :
            base(from)
        {
            Value = from.Value;
        }

        public override int Value
        {
            get => _value;
            set => _value = value;
        }

        [SerializeField]
        public int _value;

        public override void WriteProp(JsonTextWriter json)
        {
            json.WritePropertyName(Name);
            json.WriteValue(Value);
        }
    }
}