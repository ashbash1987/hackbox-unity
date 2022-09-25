using System;
using UnityEngine;
using Newtonsoft.Json.Linq;

namespace Hackbox.Parameters
{
    [Serializable]
    public class BoolParameter : Parameter<bool>
    {
        public BoolParameter():
            base()
        {
        }

        public BoolParameter(BoolParameter from):
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

        public override void ApplyValueToJObject(JObject parent, int version)
        {
            parent[Name] = Value;
        }
    }
}