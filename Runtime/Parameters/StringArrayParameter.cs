using System;
using UnityEngine;
using Newtonsoft.Json.Linq;

namespace Hackbox.Parameters
{
    [Serializable]
    public class StringArrayParameter : Parameter<string[]>
    {
        public StringArrayParameter() :
            base()
        {
        }

        public StringArrayParameter(string[] value) :
            base()
        {
            Value = value;
        }

        public StringArrayParameter(StringArrayParameter from) :
            base(from)
        {
            Value = from.Value;
        }

        public override string[] Value
        {
            get => _value;
            set => _value = value;
        }

        [SerializeField]
        public string[] _value;

        public override void ApplyValueToJObject(JObject parent)
        {
            parent[Name] = new JArray(Value);
        }
    }
}