using System;
using UnityEngine;
using Newtonsoft.Json.Linq;

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

        public override void ApplyValueToJObject(JObject parent, int version)
        {
            parent[Name] = Value;
        }
    }
}