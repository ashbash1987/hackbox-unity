using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json.Linq;

namespace Hackbox.Parameters
{
    [Serializable]
    public class ChoicesParameter : Parameter<List<ChoicesParameter.Choice>>
    {
        [Serializable]
        public class Choice
        {
            public Choice()
            {
            }

            public Choice(Choice other)
            {
                Label = other.Label;
                Value = other.Value;
                Keys = new string[other.Keys.Length];
                Array.Copy(other.Keys, Keys, other.Keys.Length);
            }

            public string Label;
            public string Value;
            public string[] Keys;

            public JObject GenerateJSON()
            {
                return JObject.FromObject(new
                {
                    label = Label,
                    value = Value,
                    keys = new JArray(Keys)
                });
            }
        }

        public ChoicesParameter() :
            base()
        {
        }

        public ChoicesParameter(ChoicesParameter from):
            base(from)
        {
            Value = new List<Choice>(from.Value.Select(x => new Choice(x)));
        }

        public override List<ChoicesParameter.Choice> Value
        {
            get => _value;
            set => _value = value;
        }

        [SerializeField]
        public List<ChoicesParameter.Choice> _value;

        public override void ApplyValueToJObject(JObject parent)
        {
            parent[Name] = new JArray(Value.Select(x => x.GenerateJSON()).ToArray());
        }
    }
}