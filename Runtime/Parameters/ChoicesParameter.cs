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

            [Tooltip("The text that is shown to the user in the choice button.")]
            public string Label = "";
            [Tooltip("The value that is returned if the user selects this choice button.")]
            public string Value = "";
            [Tooltip("The keys on a keyboard that can be pressed to select this choice button.")]
            public string[] Keys = new string[0];

            public ParameterList StyleParameterList = new ParameterList();

            internal JObject GenerateJSON()
            {
                JObject choiceObject = new JObject();

                choiceObject["label"] = Label;
                choiceObject["value"] = Value;
                choiceObject["keys"] = new JArray(Keys);

                if (Keys != null)
                {
                    choiceObject["keys"] = new JArray(Keys);
                }

                if (StyleParameterList != null && StyleParameterList.Parameters.Count > 0)
                {
                    choiceObject["style"] = GenerateStyleProps();
                }

                return choiceObject;
            }

            private JObject GenerateStyleProps()
            {
                JObject props = new JObject();
                foreach (Parameter parameter in StyleParameterList.Parameters)
                {
                    parameter.ApplyValueToJObject(props);
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

        public override void ApplyValueToJObject(JObject parent)
        {
            parent[Name] = new JArray(Value.Select(x => x.GenerateJSON()).ToArray());
        }
    }
}