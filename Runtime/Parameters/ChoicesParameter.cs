using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;

namespace Hackbox.Parameters
{
    using ChoiceList = List<ChoicesParameter.Choice>;

    [Serializable]
    public class ChoicesParameter : Parameter<ChoiceList>
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
                StyleParameterList = new ParameterList(other.StyleParameterList);
            }

            [Tooltip("The text that is shown to the user in the choice button.")]
            public string Label = "";
            [Tooltip("The value that is returned if the user selects this choice button.")]
            public string Value = "";
            [Tooltip("The keys on a keyboard that can be pressed to select this choice button.")]
            public string[] Keys = new string[0];

            public ParameterList StyleParameterList = new ParameterList();

            internal void WriteJSON(JsonTextWriter json)
            {
                json.WriteStartObject();
                {
                    json.WritePropertyName("label"); json.WriteValue(Label);
                    json.WritePropertyName("value"); json.WriteValue(Value);
                    if (Keys != null)
                    {
                        json.WritePropertyName("keys");
                        json.WriteStartArray();
                        foreach (string key in Keys)
                        {
                            json.WriteValue(key);
                        }
                        json.WriteEndArray();
                    }

                    if (StyleParameterList != null && StyleParameterList.Parameters.Count > 0)
                    {
                        json.WritePropertyName("style");
                        WriteStyleProps(json);
                    }
                }
                json.WriteEndObject();
            }

            private void WriteStyleProps(JsonTextWriter json)
            {
                json.WriteStartObject();
                {
                    foreach (Parameter parameter in StyleParameterList.Parameters)
                    {
                        parameter.WriteProp(json);
                    }
                }
                json.WriteEndObject();
            }
        }

        public ChoicesParameter() :
            base()
        {
            Value = new ChoiceList();
        }

        public ChoicesParameter(ChoiceList value) :
            base()
        {
            Value = value;
        }

        public ChoicesParameter(ChoicesParameter from):
            base(from)
        {
            Value = new ChoiceList(from.Value.Select(x => new Choice(x)));
        }

        public override ChoiceList Value
        {
            get => _value;
            set => _value = value;
        }

        [SerializeField]
        public ChoiceList _value = new ChoiceList();

        public override void WriteProp(JsonTextWriter json)
        {
            json.WritePropertyName(Name);
            json.WriteStartArray();
            {
                foreach (Choice choice in Value)
                {
                    choice.WriteJSON(json);
                }
            }
            json.WriteEndArray();            
        }
    }
}
