using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json.Linq;

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

            public string Label = "";
            public string Value = "";
            public string[] Keys = new string[0];

            public ParameterList StyleParameterList = new ParameterList();

            internal JObject GenerateJSON(int version)
            {
                JObject choiceObject = new JObject();

                choiceObject["label"] = Label;
                choiceObject["value"] = Value;
                choiceObject["keys"] = new JArray(Keys);

                if (Keys != null)
                {
                    choiceObject["keys"] = new JArray(Keys);
                }

                if (StyleParameterList != null && StyleParameterList.Parameters.Count > 0 && version >= 2)
                {
                    choiceObject["style"] = GenerateStyleProps(version);
                }

                return choiceObject;
            }

            private JObject GenerateStyleProps(int version)
            {
                JObject props = new JObject();
                foreach (Parameter parameter in StyleParameterList.Parameters)
                {
                    parameter.ApplyValueToJObject(props, version);
                }

                return props;
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

        public override void ApplyValueToJObject(JObject parent, int version)
        {
            parent[Name] = new JArray(Value.Select(x => x.GenerateJSON(version)).ToArray());
        }
    }
}