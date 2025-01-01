using System;
using UnityEngine;
using Newtonsoft.Json;

namespace Hackbox.Parameters
{
    [Serializable]
    public class ColorParameter : Parameter<Color>
    {
        public ColorParameter() :
            base()
        {
        }

        public ColorParameter(Color value) :
            base()
        {
            Value = value;
        }

        public ColorParameter(ColorParameter from) :
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

        public override void WriteProp(JsonTextWriter json)
        {
            json.WritePropertyName(Name);
            json.WriteValue(Value.ToHTMLString());
        }
    }
}