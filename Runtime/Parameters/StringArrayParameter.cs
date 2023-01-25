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

        public override void ApplyValueToJObject(JObject parent, int version)
        {
            parent[Name] = new JArray(Value);
        }
    }
}