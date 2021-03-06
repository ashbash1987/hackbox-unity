using Newtonsoft.Json.Linq;
using System;
using UnityEngine;

namespace Hackbox.Parameters
{
    [Serializable]
    public class ColorParameter : Parameter<Color>
    {
        public ColorParameter():
            base()
        {
        }

        public ColorParameter(ColorParameter from):
            base(from)
        {
            Value = from.Value;
        }

        public override Color Value
        {
            get => _value;
            set => _value = value;
        }

        [SerializeField]
        public Color _value;

        public override void ApplyValueToJObject(JObject parent)
        {
            parent[Name] = Value.ToHTMLString();
        }
    }
}